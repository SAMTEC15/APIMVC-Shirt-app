using WebMVC.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("ShirtsAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7177/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IWebAPIExecuter, WebAPIExecuter>();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
