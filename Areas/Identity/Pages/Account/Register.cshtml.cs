﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace dotnet_project.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
       // private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;


        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
           // IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        //private async Task initAdmin()
        //{
        //    bool x = await _roleManager.roleexistsasync("admin");
        //    if (!x)
        //    {
        //        // first we create admin rool    
        //        var role = new IdentityRole();
        //        role.Name = "admin";
        //        await _roleManager.createasync(role);
        //        //here we create a admin super user who will maintain the website                   
        //        var user = new identityuser { username = "administrator" };
        //        // user.username = "administrator";
        //        var userpwd = "pass1!";
        //        var chkuser = await _usermanager.createasync(user, userpwd);
        //        //add default user to role admin    
        //        if (chkuser.succeeded) await _usermanager.addtoroleasync(user, "admin");
        //    }
        //}

        //private async Task initAdmin()
        //{
        //    bool x = await _roleManager.RoleExistsAsync("Admin");
        //    if (!x)
        //    {
        //        // first we create Admin rool    
        //        var role = new IdentityRole();
        //        role.Name = "Admin";
        //        await _roleManager.CreateAsync(role);
        //        //Here we create a Admin super user who will maintain the website                   
        //        var user = new IdentityUser { UserName = "administrator"};
        //        //user.UserName = "administrator";
        //        var userPWD = "Pass1!";
        //        var chkUser = await _userManager.CreateAsync(user, userPWD);
        //        //Add default User to Role Admin    
        //        if (chkUser.Succeeded) await _userManager.AddToRoleAsync(user, "Admin");
        //    }
        //}

        public class InputModel
        {
            [Required]
            [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Username};
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //Dodanie Roli usera
                    var roleExist = await _roleManager.RoleExistsAsync("User");
                    if (!roleExist)
                    {
                        //create the roles and seed them to the database
                        var roleResult = await _roleManager.CreateAsync(new IdentityRole("User"));
                    }
                    await _userManager.AddToRoleAsync(user, "User");
                    //Dodanie Roli Admina
                    roleExist = await _roleManager.RoleExistsAsync("Admin");
                    if (!roleExist)
                    {
                        //create the roles and seed them to the database
                        var roleResult = await _roleManager.CreateAsync(new IdentityRole("Admin"));
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }



                  //  var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                   // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                   // var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                     //   pageHandler: null,
                      //  values: new { area = "Identity", userId = user.Id, code = code },
                       // protocol: Request.Scheme);


                   // await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                     //   $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

  //                  if (_userManager.Options.SignIn.RequireConfirmedAccount)
    //                {

//                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                   // }
                   // else
      //              {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
        //            }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
