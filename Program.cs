using A2.Data;
using A2.Handler;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, A2AuthHandler>("BasicAuthentication", null);

        builder.Services.AddAuthorization(options => { options.AddPolicy("IsRegisteredUser", policy => policy.RequireClaim("RegisteredUser")); });
        builder.Services.AddAuthorization(options => { options.AddPolicy("IsOrganizer", policy => policy.RequireClaim("IsOrganizer")); });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("HasAuthority", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(claim => claim.Type == ClaimTypes.RegisteredUser || claim.Type == ClaimTypes.IsOrganizer)
                )
            );
        });

        builder.Services.AddDbContext<A2DbContext>(options => options.UseSqlite(builder.Configuration["WebAPIConnection"]));

        builder.Services.AddScoped<IA2Repo, A2Repo>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}