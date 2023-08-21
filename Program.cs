using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using Newtonsoft;
using DevExpress.XtraReports.Web.Extensions;
using ReportDesignerServerSide;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
// Register reporting services in an application's dependency injection container.
builder.Services.AddDevExpressControls();
// Use the AddMvcCore (or AddMvc) method to add MVC services.
builder.Services.AddMvc();

builder.Services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowCorsPolicy", builder => {
        // Allow all ports on local host.
        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowCorsPolicy");

app.UseAuthorization();

app.UseDevExpressControls();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
