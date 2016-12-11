using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BL.DTOs.UserAccount;
using BL.Facades;
using PL.Helpers.Auth;

namespace PL.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        #region Dependencies

        public SignInManager SignInManager { get; set; }
        public UserFacade UserFacade { get; set; }

        #endregion

        // GET: Account
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Register", new UserRegistrationDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegistrationDTO model)
        {
            if (ModelState.IsValid)
            {
                if (model.BirthDate.Year < 1900)
                {
                    ModelState.AddModelError(nameof(model.BirthDate),@"Enter valid and possible date.");
                    return View("Register", model);
                }
                try
                {
                    bool success;
                    var accountId = UserFacade.RegisterUser(model, out success);
                    if (!success)
                    {
                        ModelState.AddModelError(nameof(model.Email), @"Account with this email address already exists");
                        return View("Register", model);
                    }

                    SignInManager.SignIn(accountId, false);
                    return RedirectToAction("Index", "Home");
                }
                catch (ValidationException ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            return View("Register", model);
        }

        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (returnUrl != null)
            {
                TempData["ErrorMessageTitle"] = "Not authorized: ";
                TempData["ErrorMessage"] = "Login to continue";
            }
            return View("Login", new UserLoginDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginDTO model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var accountId = UserFacade.AuthenticateUser(model);

                if (!accountId.Equals(Guid.Empty))
                {
                    SignInManager.SignIn(accountId, model.RememberMe);

                    TempData["MessageTitle"] = "Login successful";
                    TempData["Message"] = "";

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", @"Invalid Username or Password");
            }
            return View("Login", model);
        }

        public ActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                SignInManager.SignOut();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}