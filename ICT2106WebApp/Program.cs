var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Register application services
builder.Services.AddScoped<ICT2106WebApp.mod2grp6.Text.IFormatText, ICT2106WebApp.mod2grp6.Text.TextManager>();

// Register template services
builder.Services.AddScoped<ICT2106WebApp.mod2grp6.Template.TemplateGateway>();
builder.Services.AddScoped<ICT2106WebApp.mod2grp6.Template.TemplateManager>();
builder.Services.AddScoped<ICT2106WebApp.mod2grp6.DocumentManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();