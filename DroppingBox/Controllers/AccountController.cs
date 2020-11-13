using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroppingBox.Models;
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
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await iUserRepository.GetByEmail(model.Email);

                if (user != null)
                {
                    if (user.Password.Equals(model.Password)) {
                        return Redirect(model?.ReturnUrl ?? "/Box/Index");
                    }
                }

            }

            ModelState.AddModelError("", "Invalid name or password");
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signup(SignupModel model)
        {
            if (ModelState.IsValid)
            {
                if(! await iUserRepository.Exists(model.Email))
                {
                    User newUser = new User
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Password = model.Password,
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
