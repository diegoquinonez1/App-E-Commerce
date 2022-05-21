using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using Aplicacion.Cursos;
using FluentValidation.AspNetCore;
using WebAPI.Middleware;
using Dominio.Seguridad;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Aplicacion.Contratos;
using Seguridad.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CursosOnlineContext>(opt => 
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMediatR(typeof(Consulta.HandlerGetAllCursos).Assembly);

// Add services to the container.
builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
}).AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Creacion>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISystemClock, SystemClock>();
//inyeccion de JWT
builder.Services.AddScoped<IJwtGenerador, JwtGenerador>();
builder.Services.AddScoped<IUsuarioSesion, UsuarioSesion>();

builder.Services.AddAutoMapper(typeof(Consulta.HandlerGetAllCursos));

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MiPalabraSecreta"));
//implementar logica para que no se consuman controllers sin token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    //cuando se lanza un toekn se indica las restricciones que el token va a tener
    //por ejemplo ips, dns, etc
    //por ejemplo desde que ips se receiben peticiones
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        //cualqueir tipo de reequest de un cliente tiene que se validado
        //por la logica dentro del token y pasando dentro del entitycore
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        //alguien con una ip especifica pueda generar un token
        ValidateAudience = false,
        //enviaemos una ip a una ip especifica
        ValidateIssuer = false
    };
});

//configuracion de Core Identity
var usr = builder.Services.AddIdentityCore<Usuario>();
var identityBuilder = new IdentityBuilder(usr.UserType, usr.Services);
//agregar la instancia del intity framework
identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
//administrador de logins de accesos viene de SignInManager desde la clase Usuario
identityBuilder.AddSignInManager<SignInManager<Usuario>>();

var app = builder.Build();

//codigo que va a ejecutar el archivo de migracion
using(var ambiente = app.Services.CreateScope())
{
    var services = ambiente.ServiceProvider;

    try
    {
        var userManager = services.GetRequiredService<UserManager<Usuario>>();
        var context = services.GetRequiredService<CursosOnlineContext>();

        context.Database.Migrate();
        await AdminUsuarios.InsertarUsuario(context, userManager);
    }
    catch (System.Exception e)
    {
        var logging = services.GetRequiredService<ILogger<Program>>();
        logging.LogError("Ocurrio un error en la migracion" + e);
    }
}

//luego se debe ejecutar un comando para aplicar toda la migracion en la base de datos antes de hacerlo
//entra a CD WebAPI/
//dotnet watch run

app.UseMiddleware<ManejadorErrorMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
//https://localhost:7127;

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();