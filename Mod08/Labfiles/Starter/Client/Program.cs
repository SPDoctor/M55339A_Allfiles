var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    "SchoolRoute",
    "{controller}/{action}/{id?}",
    new { controller = "Department", action = "Index" },
    new { id = "[0-9]+" }
    );
app.Run();