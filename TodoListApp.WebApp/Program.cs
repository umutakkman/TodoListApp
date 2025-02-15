using TodoListApp.Services.Implementations;
using TodoListApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<ITodoListWebApiService, TodoListWebApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["TodoListApiBaseUrl"]);
});

builder.Services.AddHttpClient<ITaskItemWebApiService, TaskItemWebApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["TodoListApiBaseUrl"]);
});

builder.Services.AddHttpClient<ITagWebApiService, TagWebApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["TodoListApiBaseUrl"]);
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
