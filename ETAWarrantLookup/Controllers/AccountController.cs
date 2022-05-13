using ETAWarrantLookup.Models;
using ETAWarrantLookup.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace ETAWarrantLookup.Controllers
{

    public class Account : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly EmailHelper _emailHelper;

        public Account(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager/*, EmailHelper emailHelper*/) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            //_emailHelper = emailHelper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // Resolve the user via their email
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    // Get the roles for the user
                    var roles = await _userManager.GetRolesAsync(user);

                    if(roles.Contains("admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    return RedirectToAction("Search", "Warrant");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]  
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    BusinessName = model.BusinessName,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };


                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var confirmationLink = Url.Action("ConfirmEmail", "Email", new { token, email = user.Email }, Request.Scheme);
                    //EmailHelper emailHelper = new EmailHelper();
                    //bool emailResponse = emailHelper.SendEmail(user.Email, confirmationLink);

                    //if (emailResponse)
                    //    return RedirectToAction("Index");
                    //else
                    //{
                    //    // log email failed 
                    //}

                    return RedirectToAction("Payment");
                    //return RedirectToAction("Login");

                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Payment()
        {
            return View();
        }

    }
}
