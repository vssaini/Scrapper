﻿using Firebase.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Scrapper.Application.Abstractions.Logger;
using Scrapper.Models;
using Scrapper.Models.Authentication;
using System.Security.Claims;

namespace Scrapper.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogService _logService;
        private readonly FirebaseAuthProvider _fbAuth;

        public AccountController(ILogService logService, IOptions<Scrapper.Models.Settings.Firebase> firebase)
        {
            _logService = logService;

            // Ref - https://www.freecodespot.com/blog/firebase-in-asp-net-core-mvc/
            _fbAuth = new FirebaseAuthProvider(new FirebaseConfig(firebase.Value.WebApiKey));
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(LoginModel loginModel)
        {
            try
            {
                //create the user
                await _fbAuth.CreateUserWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);

                //log in the new user
                var fbAuthLink = await _fbAuth
                    .SignInWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);

                string token = fbAuthLink.FirebaseToken;
                //saving the token in a session variable
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (FirebaseAuthException ex)
            {
                _logService.LogError(ex, "Error while signing up new user.");

                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(string.Empty, firebaseEx.error.message);
                return View(loginModel);
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            try
            {
                //log in an existing user
                var fbAuthLink = await _fbAuth
                    .SignInWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);

                string token = fbAuthLink.FirebaseToken;
                //save the token to a session variable
                if (token != null)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, loginModel.Email)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    HttpContext.Session.SetString("_UserToken", token);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (FirebaseAuthException ex)
            {
                _logService.LogError(ex, "Error while signing in new user.");

                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(string.Empty, firebaseEx.error.message);
                return View(loginModel);
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("_UserToken");

            return RedirectToAction("Login");
        }
    }
}
