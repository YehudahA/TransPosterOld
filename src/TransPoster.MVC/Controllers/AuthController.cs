﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TransPoster.MVC.Models.Auth;

namespace TransPoster.MVC.Controllers
{
    public class AuthController : Controller
    {
        private const string LoginAttemptsSessionName = "_LoginAttempts";

        private readonly ILogger<AuthController> _logger;
        private readonly IStringLocalizer<AuthController> _localizer;

        public AuthController(ILogger<AuthController> logger, IStringLocalizer<AuthController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public IActionResult Login()
        {
            ViewData["loginAttempts"] = HttpContext.Session.GetInt32(LoginAttemptsSessionName) ?? 0;
            ViewData["LoginText"] = _localizer["Login"].Value;
            _logger.LogInformation("This is the login credentials {Login}", _localizer["Login"].Value);

            return View();
        }

        [HttpPost]
        public IActionResult SubmitLogin(LoginCredentials body)
        {
            _logger.LogInformation("This is the login credentials {Email}, {Password}, {RememberMe}", body.Email, body.Password, body.RememberMe);

            // for failed attempts
            var loginAttempts = HttpContext.Session.GetInt32(LoginAttemptsSessionName) ?? 0;
            HttpContext.Session.SetInt32(LoginAttemptsSessionName, loginAttempts + 1);
            _logger.LogInformation("Session key: {sessionKey}", loginAttempts + 1);

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
    }
}