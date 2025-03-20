var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICT2106WebApp.mod2grp6.Text.IFormatText, ICT2106WebApp.mod2grp6.Text.TextManager>();
builder.Services.AddScoped<ICT2106WebApp.mod2grp6.Template.TemplateManager>();

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

app.Run();
