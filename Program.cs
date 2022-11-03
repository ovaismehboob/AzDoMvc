using AzDoMVCApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<AzDoService>(); 




// Add services to the container.
builder.Services.AddControllersWithViews(); 

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "ALLOW-FROM https://dev.azure.com");
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Project}/{action=Index}/{id?}");

app.Run();
