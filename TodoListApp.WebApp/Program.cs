using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Implementations;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApp.Email;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    _ = options.UseSqlServer(builder.Configuration.GetConnectionString("TodoListDb")
!);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddHttpClient<ITodoListWebApiService, TodoListWebApiService>(client =>
{
    var baseUrl = builder.Configuration["TodoListApiBaseUrl"];
    if (string.IsNullOrEmpty(baseUrl))
    {
        throw new InvalidOperationException("TodoListApiBaseUrl configuration is missing or empty.");
    }

    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient<ITaskItemWebApiService, TaskItemWebApiService>(client =>
{
    var baseUrl = builder.Configuration["TodoListApiBaseUrl"];
    if (string.IsNullOrEmpty(baseUrl))
    {
        throw new InvalidOperationException("TodoListApiBaseUrl configuration is missing or empty.");
    }

    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient<ITagWebApiService, TagWebApiService>(client =>
{
    var baseUrl = builder.Configuration["TodoListApiBaseUrl"];
    if (string.IsNullOrEmpty(baseUrl))
    {
        throw new InvalidOperationException("TodoListApiBaseUrl configuration is missing or empty.");
    }

    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient<ICommentWebApiService, CommentWebApiService>(client =>
{
    var baseUrl = builder.Configuration["TodoListApiBaseUrl"];
    if (string.IsNullOrEmpty(baseUrl))
    {
        throw new InvalidOperationException("TodoListApiBaseUrl configuration is missing or empty.");
    }

    client.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");

    _ = app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
