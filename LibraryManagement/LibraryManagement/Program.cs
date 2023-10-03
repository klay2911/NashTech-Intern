
using LibraryManagement.Controllers;
using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<ILibraryRepository<Book>, BookRepository>();
builder.Services.AddScoped<ILibraryRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookBorrowingRequestRepository, BookBorrowingRequestRepository>();
builder.Services.AddScoped<ILibraryRepository<BookBorrowingRequestDetails>, BookBorrowingRequestDetailsRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookBorrowingRequestService, BookBorrowingRequestService>();
builder.Services.AddScoped<IBookBorrowingRequestDetailsService, BookBorrowingRequestDetailsService>();

builder.Services.AddDbContext<LibraryContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });
builder.Services.AddSession();
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
// app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

// public class JwtMiddleware
// {
//     private readonly RequestDelegate _next;
//     private readonly AccountController _yourService; // replace with your actual service that has the ValidateToken method
//
//     public JwtMiddleware(RequestDelegate next, AccountController yourService)
//     {
//         _next = next;
//         _yourService = yourService;
//     }
//
//     public async Task Invoke(HttpContext context)
//     {
//         var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
//
//         if (token != null)
//         {
//             var claimsPrincipal = _yourService.ValidateToken(token);
//
//             if (claimsPrincipal != null)
//             {
//                 context.User = claimsPrincipal;
//             }
//         }
//
//         await _next(context);
//     }
// }
