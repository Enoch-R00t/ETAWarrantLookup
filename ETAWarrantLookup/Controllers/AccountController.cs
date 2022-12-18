using ETAWarrantLookup.Data;
using ETAWarrantLookup.Models;
using ETAWarrantLookup.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ETAWarrantLookup.Controllers
{

    public class Account : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<Account> _logger;
        private readonly IConfiguration _configuration;
        private readonly ETADbContext _dbContext;
        //private readonly EmailHelper _emailHelper;

        public Account(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ILogger<Account> logger, 
            ETADbContext dbContext/*, EmailHelper emailHelper*/) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _dbContext = dbContext;
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

                    if (roles.Contains("admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                    // Does the user have a valid subscription?
                    if (!SubscriptionCurrent(user.Id))
                    {
                        return RedirectToAction("Payment", "Account");
                    }

                    // Get subscriptione expiration to use in Layout
                    var subscriptionExpires = SubscriptionExpires(user.Id);
                    if (subscriptionExpires != null)
                    {
                        HttpContext.Session.SetString("subscriptionExpires", ((DateTime)subscriptionExpires).ToShortDateString());
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
        [Authorize]
        public async Task<IActionResult> Payment()
        {
            PaymentOptionsViewModel vm = new PaymentOptionsViewModel();

            // Get logged in users id to use for the ApplicationUserId for the new subscription
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
           
            if (userId == null)
            {
                RedirectToAction("Index", "Home");
            }

            // Create a new unique Id for the transaction token and store in the db
            Guid refToken = Guid.NewGuid();

            try
            {
                _dbContext.Subscriptions.Add(
                   new Subscription
                   {
                       ReferenceToken = refToken,
                       ApplicationUserId = userId
                   });

                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, null);
            }

            // build up the redirect url for the credit card processor         
            var localIpAddress = _configuration.GetSection("LocalHostConfiguration").GetChildren().FirstOrDefault(config => config.Key == "IPAddress").Value;
            var localProtocol = _configuration.GetSection("LocalHostConfiguration").GetChildren().FirstOrDefault(config => config.Key == "Protocol").Value;

            vm.referenceToken = refToken;
            
            vm.redirectUrl = string.Format("{0}{1}{2}{3}{4}", localProtocol, "://", localIpAddress, "/Account/PaymentRedirect?refToken=", refToken);

            // build up the payment processor url
            var paymentUrl = _configuration.GetSection("PaymentSite").GetChildren().FirstOrDefault(config => config.Key == "Url").Value;

            vm.paymentUrl = paymentUrl;

            vm.paymentOptions = _dbContext.PaymentOptions.ToList();

            return View(vm);
        }

        /// <summary>
        /// Responsible for handling the POST from the payment site for credit card transactions
        /// </summary>
        /// <returns></returns>
        [EnableCors("CORSPolicy")]
        [IgnoreAntiforgeryToken]
       // [AllowAnonymous]
        [HttpPost]
        //[Route("PaymentRedirect")]
        public IActionResult PaymentRedirect()
        {
            // This is the format that the response occurs in
            //[0]: { [uniq_id, d892bee3-a8c8 - 4e06 - 8131 - d821db957c19]}
            //[1]: { [ref_id, 879293975]}
            //[2]: { [auth_code, 565724]}
            //[3]: { [amount, 100]}

            var refToken = HttpContext.Request.Query["refToken"];

            var keys = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());

            if(keys.ContainsKey("error"))
            {
                //log the error
                _logger.LogError("Credit card processing failed for ReferenceToken " + refToken + " error returned: " + keys["error"].ToString(), null);

                //redirect to error page
               return View("Error", new AccountPaymentErrorViewModel { Error = keys["error"].ToString() });
            }

            try
            {
                // go get the record from the db
                var subscription = _dbContext.Subscriptions.Where(m => m.ReferenceToken == Guid.Parse(refToken)).FirstOrDefault();

                // update and store
                subscription.ReferenceId = keys["ref_id"].ToString();
                subscription.AuthorizationCode = keys["auth_code"].ToString();
                subscription.PaymentAmount = decimal.Parse(keys["amount"].ToString()); 
                subscription.PaymentDate = DateTime.Now;

                var expDate = _dbContext.PaymentOptions.Where(m => m.Price == subscription.PaymentAmount).FirstOrDefault().TimeFrame;
                subscription.PaymentExpirationDate = DateTime.Now.AddDays(expDate);

                _dbContext.SaveChanges();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, null);
            }


            return RedirectToAction("Search", "Warrant");
        }

        /// <summary>
        /// Determines if a user has a valid subscription record in the database
        /// </summary>
        /// <param name="ApplicationUserId"></param>
        /// <returns>True if and false if not</returns>
        private bool SubscriptionCurrent(string ApplicationUserId)
        {
            var subscriptions = _dbContext.Subscriptions.Where(m => m.ApplicationUserId == ApplicationUserId).ToList();

            foreach(var subscription in subscriptions)
            {
                if(subscription.ReferenceId != null && subscription.AuthorizationCode != null && subscription.PaymentAmount != null)
                {
                    if(((DateTime)subscription.PaymentExpirationDate) > DateTime.Now)
                     {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines the expiration date of the users subscription
        /// </summary>
        /// <param name="ApplicationUserId"></param>
        /// <returns>DateTime value containing the expiration date</returns>
        private DateTime? SubscriptionExpires(string ApplicationUserId)
        {
            var subscriptions = _dbContext.Subscriptions.Where(m => m.ApplicationUserId == ApplicationUserId).ToList();

            foreach (var subscription in subscriptions)
            {
                if (subscription.ReferenceId != null && subscription.AuthorizationCode != null && subscription.PaymentAmount != null)
                {
                    if (((DateTime)subscription.PaymentExpirationDate) > DateTime.Now)
                    {
                        return (DateTime)subscription.PaymentExpirationDate;
                    }
                }
            }

            return null;
        }
    }
}
