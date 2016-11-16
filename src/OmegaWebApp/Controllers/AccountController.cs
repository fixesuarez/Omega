using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmegaWebApp.Authentication;
using OmegaWebApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Authentication;
using System.Security.Claims;

namespace OmegaWebApp.Controllers
{
    public class AccountController : Controller
    {
        readonly UserService _userService;
        readonly TokenService _tokenService;
        readonly Random _random;

        public AccountController( UserService userService, TokenService tokenService )
        {
            _userService = userService;
            _tokenService = tokenService;
            _random = new Random();
        }
        
        [HttpGet]
        [AllowAnonymous]
        [Route( "Account/ExternalLogin" )]
        public IActionResult ExternalLogin( [FromQuery] string provider )
        {
            // Note: the "provider" parameter corresponds to the external
            // authentication provider choosen by the user agent.
            if (string.IsNullOrWhiteSpace( provider ))
            {
                return BadRequest();
            }

            if (!HttpContext.IsProviderSupported( provider ))
            {
                return BadRequest();
            }

            // Instruct the middleware corresponding to the requested external identity
            // provider to redirect the user agent to its own authorization endpoint.
            // Note: the authenticationScheme parameter must match the value configured in Startup.cs
            string redirectUri = Url.Action( nameof( ExternalLoginCallback ), "Account" );
            return Challenge( new AuthenticationProperties { RedirectUri = redirectUri }, provider );
        }

        [HttpGet]
        [Authorize( ActiveAuthenticationSchemes = CookieAuthentication.AuthenticationScheme )]
        public IActionResult ExternalLoginCallback()
        {
            return RedirectToAction( nameof( Authenticated ) );
        }

        [HttpGet]
        [Authorize( ActiveAuthenticationSchemes = CookieAuthentication.AuthenticationScheme )]
        public IActionResult Authenticated()
        {
            string userId = User.FindFirst( ClaimTypes.NameIdentifier ).Value;
            string email = User.FindFirst( ClaimTypes.Email ).Value;
            Token token = _tokenService.GenerateToken( userId, email );
            IEnumerable<string> providers = (IEnumerable<string>) _userService.GetAuthenticationProviders( email );
            ViewData["BreachPadding"] = GetBreachPadding(); // Mitigate BREACH attack. See http://www.breachattack.com/
            ViewData["Token"] = token;
            ViewData["Email"] = email;
            ViewData["NoLayout"] = true;
            ViewData["Providers"] = providers;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login( string returnUrl = null )
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register( string returnUrl = null )
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.Authentication.SignOutAsync( CookieAuthentication.AuthenticationScheme );
            return RedirectToAction( "Index", "Home" );
        }

        IActionResult RedirectToLocal( string returnUrl )
        {
            if (Url.IsLocalUrl( returnUrl ))
            {
                return Redirect( returnUrl );
            }
            else
            {
                return RedirectToAction( nameof( HomeController.Index ), "Home" );
            }
        }

        string GetBreachPadding()
        {
            byte[] data = new byte[_random.Next( 64, 256 )];
            _random.NextBytes( data );
            return Convert.ToBase64String( data );
        }
    }
}
