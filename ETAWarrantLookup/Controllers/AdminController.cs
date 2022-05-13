using ETAWarrantLookup.Data;
using ETAWarrantLookup.Models;
using ETAWarrantLookup.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;

using System.Diagnostics;

namespace ETAWarrantLookup.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private ETADbContext _dbContext;
      

        public AdminController(ILogger<AdminController> logger, ETADbContext dbContext )
        {
            _logger = logger;
            _dbContext = dbContext;
            
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Index()
        {
            // get all the user records
            List<ApplicationUserViewModel> userList = new List<ApplicationUserViewModel>();

            var dbSet = _dbContext.Users;

            //convert to viewmodel
            foreach (var user in dbSet)
            {
                ApplicationUserViewModel appUser = new ApplicationUserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    BusinessName = user.BusinessName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    LockoutEnd = user.LockoutEnd,
                    LockoutEnabled = user.LockoutEnabled,
                    AccessFailedCount = user.AccessFailedCount
                };

                userList.Add(appUser);
            }     
   
            return View(userList);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
