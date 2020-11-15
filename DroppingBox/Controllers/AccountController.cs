using System.Collections.Generic;
using System.Threading.Tasks;
using DroppingBox.Models;
using DroppingBox.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DroppingBox.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserRepository iUserRepository;

        public AccountController(IUserRepository iUserRepository)
        {
            this.iUserRepository = iUserRepository;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await iUserRepository.GetByEmail(model.Email);

                if (user != null)
                {                                        
                    if (BCrypt.Net.BCrypt.Verify(model.Password, user.Password)) {
                        HttpContext.Session.SetString("loggedInUser", user.Email);
                        return Redirect(model?.ReturnUrl ?? "/Home/Index");
                    }
                }

            }

            ModelState.AddModelError("", "Invalid name or password");
            return View(model);
        }

        [AllowAnonymous]

        public ActionResult Logout()
        {
            HttpContext.Session.SetString("loggedInUser", "");
            return RedirectToAction("Login", "Account");
        }

        // signup actions
        [AllowAnonymous]
        [HttpGet("Signup")]
        public ActionResult Signup()
        {
            SignupViewModel model = new SignupViewModel();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost("Signup")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signup(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await iUserRepository.Exists(model.Email))
                {
                    User newUser = new User
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                        Files = new List<File>()
                    };

                    try
                    {
                        await iUserRepository.Create(newUser);

                        return RedirectToAction(nameof(Login));
                    }
                    catch
                    {
                        return View();
                    }
                }
                ModelState.AddModelError(string.Empty, "Error: The email address already exists");
                return View(model);
            }
            return View(model);
        }

    }
}
