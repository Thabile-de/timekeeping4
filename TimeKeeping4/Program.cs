using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TimeKeeping4.Data;
using TimeKeeping4.Model;
using TimeKeeping4.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TimeKeeping4Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TimeKeeping4Context") ?? throw new InvalidOperationException("Connection string 'TimeKeeping4Context' not found.")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["AWSCognito:Authority"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false
    };
});

// Add services to the container.

//IServiceCollection serviceCollection = builder.Services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));
builder.Services.AddCognitoIdentity();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
     app.UseHsts();   

}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
// Shows UseCors with CorsPolicyBuilder.
app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:3000")
            .SetIsOriginAllowedToAllowWildcardSubdomains()
           .WithMethods("GET", "POST")
           .AllowAnyHeader()
           .AllowCredentials();
 });

app.UseAuthentication();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
