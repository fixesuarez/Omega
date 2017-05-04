using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OmegaWebApp.Authentication;
using OmegaWebApp.Services;
using System.Security.Claims;

namespace OmegaWebApp.Controllers
{
    public class HomeController : Controller
    {
        readonly TokenService _tokenService;
        readonly UserService _userService;

        public HomeController( TokenService tokenService, UserService userService )
        {
            _tokenService = tokenService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            ClaimsIdentity identity = User.Identities.FirstOrDefault( i => i.AuthenticationType == "Cookies" );
            if( identity != null )
            {
                Claim cookie = identity.FindFirst( "http://omega.com:guid" );
                if( cookie != null )
                {
                    string guid = cookie.Value;
                    Token token = _tokenService.GenerateToken( guid );
                    IEnumerable<string> providers = _userService.GetAuthenticationProviders( guid ).Result;
                    ViewData["Token"] = token;
                    ViewData["Guid"] = guid;
                    ViewData["Providers"] = providers;
                }
            }
            else
            {
                ViewData["Token"] = null;
                ViewData["Guid"] = null;
                ViewData["Providers"] = null;
            }
            
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [Authorize( ActiveAuthenticationSchemes = CookieAuthentication.AuthenticationScheme )]
        public IActionResult VerySecure()
        {
            return View();
        }
    }
}
