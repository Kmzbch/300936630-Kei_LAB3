using System.Threading.Tasks;
using DroppingBox.Models;
using DroppingBox.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DroppingBox.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // field
        private IUserRepository iUserRepository;

        // constructors
        public AccountController(IUserRepository iUserRepository)
        {
            this.iUserRepository = iUserRepository;
        }

        // login actions
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
                    HttpContext.Session.SetString("loggedInUser", user.Email);
                    
                    if (user.Password.Equals(model.Password)) {
                        return Redirect(model?.ReturnUrl ?? "/Box/Index");
                    }
                }

            }

            ModelState.AddModelError("", "Invalid name or password");
            return View(model);
        }

        // signup actions
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
