using BooksMVC.Data;
using BooksMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace BooksMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsersContext _context;
        public AccountController(UsersContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                User? user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email &&
                u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email);
                    return RedirectToAction(nameof(Index),
                    "Home");
                }
                ModelState.AddModelError(""
                ,
                "Incorrect login and (or) password!");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    User userToAdd = new User
                    {
                        Email = model.Email,
                        Password = model.Password,
                    };
                    _context.Users.Add(userToAdd);
                    await _context.SaveChangesAsync();
                    await Authenticate(model.Email);
                    return RedirectToAction(nameof(Index),
                    "Home");
                }
                ModelState.AddModelError(""
                ,
                "Incorrect login and (or) password!");
            }
            return View(model);
        }
        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(
            claims: claims,
            authenticationType: "ApplicationCookie"
            ,
            nameType: ClaimsIdentity.DefaultNameClaimType,
            roleType: ClaimsIdentity.DefaultRoleClaimType
            );
            await HttpContext.SignInAsync(
            scheme: CookieAuthenticationDefaults.AuthenticationScheme,
            principal: new ClaimsPrincipal(id)
            );
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
            scheme: CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login),"Account");
        }
    }
}
