using BusinessLogic.Services;
using BusinessObject.Entity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddSession();
builder.Services.AddSignalR();
var apiBase = builder.Configuration["ApiBaseUrls:SchoolMedicalApi"];

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(apiBase!);
});
// Add Authentication
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("MedicalStaff", policy => policy.RequireRole("Admin", "SchoolNurse", "Manager"));
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapGet("/", context =>
{
    context.Response.Redirect("/auth/login");
    return Task.CompletedTask;
});
app.Run();
