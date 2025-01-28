using Api.Acesso;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace Api.Controllers
{
    public class OAuthController : Controller
    {
        

        //[HttpGet]
        //[Route("colaborador/login-facebook")]
        //public IActionResult LoginFacebook()
        //{
        //    var authenticationProperties = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action("LoginCallbackGoogle", "Colaboradores"));
        //    return Challenge(authenticationProperties, "Facebook");
        //}

        //[HttpGet]
        //[Route("colaborador/login-callback-facebook")]
        //public async Task<IActionResult> LoginCallbackFacebook()
        //{
        //    var info = await _signInManager.GetExternalLoginInfoAsync();

        //    if (info == null)
        //    {
        //        return RedirectToAction("Login"); // Redireciona para uma página de erro ou de login
        //    }

        //    var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        //    if (user == null)
        //    {
        //        // Se o usuário não existir, você pode criar um novo usuário ou retornar erro
        //        user = new ApplicationUser { UserName = info.Principal.FindFirstValue(ClaimTypes.Email), Email = info.Principal.FindFirstValue(ClaimTypes.Email) };
        //        await _userManager.CreateAsync(user);
        //    }

        //    var token = TokenService.GenerateJwtToken(user); // Gera o token JWT

        //    // Retorna o token JWT para o cliente
        //    return Ok(new { token });
        //}

        [HttpGet]
        [Route("colaborador/login-google")]
        public IActionResult LoginGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Route("colaborador/login-callback-google")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result?.Principal == null)
                return RedirectToAction("Login");

            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims
                .Select(claim => new
                {
                    claim.Type,
                    claim.Value
                });

            // Exibir os dados do usuário autenticado
            return Json(claims);
        }

        // Logout
        
        public async Task<IActionResult> LogoutGoogle()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        [HttpGet]
        [Route("colaborador/login-facebook")]
        public IActionResult LoginFacebook()
        {
            var redirectUrl = Url.Action("FacebookResponse", "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Route("colaborador/login-callback-facebook")]
        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result?.Principal == null)
                return RedirectToAction("Login");

            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims
                .Select(claim => new
                {
                    claim.Type,
                    claim.Value
                });

            // Exibir os dados do usuário autenticado
            return Json(claims);
        }

        // Logout
        public async Task<IActionResult> LogoutFaceboook()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
