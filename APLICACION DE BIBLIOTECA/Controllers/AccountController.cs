using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using APLICACION_DE_BIBLIOTECA.Models;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Authorization;

namespace APLICACION_DE_BIBLIOTECA.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)

        {
            return View(new LoginViewModel
            {
                ReturnUrl =returnUrl
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Buscar al usuario por nombre de usuario o email
            var user = await _userManager.FindByNameAsync(model.UserNameOrEmail)
                ?? await _userManager.FindByEmailAsync(model.UserNameOrEmail);

            if (user is not null)
            {
               
                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName!,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true
                );

                if (result.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "La cuenta esta bloqueada temporalmente por demasiados intentos fallidos");
                    return View(model);
                }
            }

            ModelState.AddModelError(string.Empty, "Las credenciales introducidas no son correctas");
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new IdentityUser
            {
                UserName = model.UserName, 
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(user.Email) && user.Email.ToLower().Contains("admin"))
                {
                    await _userManager.AddToRoleAsync(user, "Administrador");
                }
            
                else if (!string.IsNullOrEmpty(user.Email) && user.Email.ToLower().Contains("bibliotecario"))
                {
                    await _userManager.AddToRoleAsync(user, "Bibliotecario");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "Usuario");
                }

               
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

           
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]

        public ActionResult AccessDenied()
        {
            return View();
        }


    }
}
