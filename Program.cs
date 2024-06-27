using CoreSample;
//using CoreSample.Entities;
using CoreSample.Extensions;
using CoreSample.Model;
using CoreSample.Model.Interface;
using CoreSample.Model.Repository;
using CoreSample.Model.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


builder.Services.AddDbContext<rentDBContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("CoreSampleCon"));
                });

builder.Services.AddDbContext<patientDBContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("CoreSampleCon2"));
                });


builder.Services.AddDbContext<DatabaseContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection")));



builder.Services.AddTransient<IRent, RentService>();
builder.Services.AddTransient<IDept, DeptService>();
builder.Services.AddTransient<IPatientDetails, PatientDetailsService>();

//builder.Services.ConfigureSQLcontext(


builder.Services.AddTransient<IEmployees, EmployeeRepository>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});


//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

//    // Add support for XML comments
//    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//    options.IncludeXmlComments(xmlPath);
//});

//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Core Sample", Version = "v1" });

//    // Add support for XML comments
//    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var commentsFileName = "Comments" + ".XML";
//    var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, commentsFileName);
//    options.IncludeXmlComments(xmlPath);
//});

builder.Services.AddAuthentication(opt =>
{

    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;




        options.TokenValidationParameters = new TokenValidationParameters
        {
            //ValidateIssuer = true,
            //ValidateAudience = true,
            //ValidateLifetime = true,
            //ValidateIssuerSigningKey = true,
            //ValidIssuer = "http://localhost:7053",
            //ValidAudience = "http://localhost:7053",
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("supersecretkey@12345678910supersecretkey"))



            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

        };
    });


builder.Services.AddSwaggerGen(cns =>
{
    // .......
    cns.SwaggerDoc("v1", new OpenApiInfo { Title = "Core Sample", Version = "v1" });
    List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
    foreach (string fileName in xmlFiles)
    {
        string xmlFilePath = Path.Combine(AppContext.BaseDirectory, fileName);
        if (File.Exists(xmlFilePath))
            cns.IncludeXmlComments(xmlFilePath, includeControllerXmlComments: true);
    }
});
builder.Services.AddCors();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
                      builder =>
                      {
                          builder.WithOrigins("https://localhost:4200", "http://localhost:7053")
                          .AllowAnyHeader()
                          .AllowAnyOrigin()
                          .AllowAnyMethod();
                          //builder.WithOrigins("http://example.com",
                          //                    "http://www.contoso.com");
                      });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

//builder.Services.AddExceptionHandler<GlobalExceptionHandlerGlobal>();
//builder.Services.AddProblemDetails();
//app.UseMiddleware<GlobalExceptionHandlerGlobal>();

//var builderobj = WebApplication.CreateBuilder(args);
//var appobj = builder.Build();

//appobj.Use(async (context, next) =>
//{
//    // Do work that can write to the Response.
//    await next.Invoke();
//    // Do logging or other work that doesn't write to the Response.
//});



//appobj.Run(async context =>
//{
//    await context.Response.WriteAsync("Hello ");
//});

//appobj.Run();



//if (appobj.Environment.IsDevelopment())
//{
//    appobj.UseDeveloperExceptionPage();
//    // appobj.UseDatabaseErrorPage();
//}
//else
//{
//    appobj.UseExceptionHandler("/Error");
//    appobj.UseHsts();
//}
//appobj.UseHttpsRedirection();
//appobj.UseStaticFiles();
//appobj.UseCookiePolicy();
//appobj.UseRouting();
//appobj.UseAuthentication();
//appobj.UseAuthorization();
//appobj.UseSession();
//appobj.MapRazorPages();

//app.UseCustom1Middleware();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Core Sample v1");
    options.RoutePrefix = string.Empty; // Set the Swagger UI at the root URL
});

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<rentDBContext>(options =>
//    options.UseSqlServer(
//        configuration.GetConnectionString("CoreSampleCon")));



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseCors();
app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapControllers();

//app.MapRazorPages();
//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync(
//        $"CurrentCulture.DisplayName: {CultureInfo.CurrentCulture.DisplayName}");


//    //await context.Response.WriteAsync(
//    //   $"CurrentCulture.DisplayName2: {"TEST"}");


//});

app.Run();

//app.UseMyMiddleware();
