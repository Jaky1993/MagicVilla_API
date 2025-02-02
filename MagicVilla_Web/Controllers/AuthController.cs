using MagicVilla_Utility;
using MagicVilla_VillaAPI.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MagicVilla_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO obj = new();

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            APIResponse response = await _authService.LoginAsync<APIResponse>(obj);

            if (response != null && response.IsSucces)
            {
                LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                //crea un'istanza di ClaimsIdentity utilizzando lo schema di autenticazione basato sui cookie
                /*
                La classe ClaimsIdentity in .NET rappresenta un'entità utente (come un utente autenticato) e i
                relativi "claim" associati a tale utente. I claim sono semplici asserzioni su un utente, come il nome,
                il ruolo, l'età, ecc., che possono essere utilizzati per prendere decisioni di autorizzazione e autenticazione
                */
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, model.User.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, model.User.Role));

                /*
                La riga var principal = new ClaimsPrincipal(identity); crea un'istanza di ClaimsPrincipal utilizzando
                l'identità (ClaimsIdentity) che hai creato in precedenza.
                Un ClaimsPrincipal rappresenta l'utente autenticato con una collezione di identità
                (solitamente una sola, ma può contenere più identità). In questo caso, stai creando un ClaimsPrincipal 
                con un'unica identità che contiene i claim dell'utente
                */
                var principal = new ClaimsPrincipal(identity);

                //Questa riga di codice autentica l'utente utilizzando lo schema di autenticazione basato sui cookie e
                //le informazioni del ClaimsPrincipal appena creato
                //HttpContext.SignInAsync: Questo metodo firma (autentica) l'utente all'interno del contesto HTTP attuale.
                //CookieAuthenticationDefaults.AuthenticationScheme: Specifica lo schema di autenticazione che verrà utilizzato
                //per l'autenticazione. In questo caso, stiamo utilizzando l'autenticazione basata sui cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                /*
                Quando questa riga di codice viene eseguita, il framework ASP.NET Core crea un cookie di autenticazione
                contenente i dati dell'utente e lo invia al client. Il cookie verrà inviato con le richieste successive
                dal client, permettendo al server di identificare l'utente autenticato e di accedere ai suoi claim.
                In altre parole, questa riga di codice finalizza il processo di login, rendendo l'utente autenticato
                per la sessione corrente 
                */

                /*
                HttpContext.Session: Rappresenta la sessione HTTP corrente, che consente di memorizzare e recuperare dati
                durante la durata di una sessione utente.
                SetString: È un metodo che memorizza una stringa nella sessione utilizzando una chiave specifica.
                In questo caso, stiamo utilizzando SD.SessionToken come chiave.
                SD.SessionToken: È la chiave utilizzata per identificare il valore memorizzato nella sessione.
                Probabilmente è una costante o una proprietà definita nella classe SD.
                model.Token: Il valore che viene salvato nella sessione. In questo caso, è il token dell'utente memorizzato
                nel modello model.
                In sintesi, questa riga di codice salva il token dell'utente nella sessione utilizzando la chiave SD.SessionToken.
                Questo permette di recuperare facilmente il token in altre parti dell'applicazione durante la durata della sessione utente.
                */
                HttpContext.Session.SetString(SD.SessionToken, model.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.ErrorMessage.FirstOrDefault());

                return View(obj);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDTO obj)
        {
            APIResponse response = await _authService.RegisterAsync<APIResponse>(obj);

            if (response != null && response.IsSucces)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            HttpContext.Session.SetString(SD.SessionToken, "");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
