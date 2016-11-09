using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmegaWebApp.Authentication;
using OmegaWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public AccountController( UserService userService )
        {
            _userService = userService;
        }

        [HttpGet]
        [AllowAnonymous]
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

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login( LoginViewModel model, string returnUrl = null )
        //{
        //    if (ModelState.IsValid)
        //    {
        //        User user = _userGateway.FindByEmail( model.Email );
        //        if (user == null || _passwordHasher.VerifyHashedPassword( user.Password, model.Password ) != PasswordVerificationResult.Success)
        //        {
        //            ModelState.AddModelError( string.Empty, "Invalid login attempt." );
        //            ViewData["ReturnUrl"] = returnUrl;
        //            return View( model );
        //        }
        //        List<Claim> claims = new List<Claim>
        //        {
        //            new Claim( ClaimTypes.Email, model.Email, ClaimValueTypes.String ),
        //            new Claim( ClaimTypes.NameIdentifier, user.UserId.ToString(), ClaimValueTypes.String )
        //        };
        //        ClaimsIdentity identity = new ClaimsIdentity( claims, "Cookies", ClaimTypes.Email, string.Empty );
        //        ClaimsPrincipal principal = new ClaimsPrincipal( identity );
        //        await HttpContext.Authentication.SignInAsync( CookieAuthentication.AuthenticationScheme, principal );
        //        return RedirectToLocal( returnUrl );
        //    }

        //    return View( model );
        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register( string returnUrl = null )
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public IActionResult Register( RegisterViewModel model, string returnUrl = null )
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (_userGateway.FindByEmail( model.Email ) != null)
        //        {
        //            ModelState.AddModelError( string.Empty, "An account with this email already exists." );
        //            return View( model );
        //        }
        //        _userGateway.Create( model.Email, _passwordHasher.HashPassword( model.Password ) );
        //        return RedirectToLocal( returnUrl );
        //    }

        //    return View( model );
        //}

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
