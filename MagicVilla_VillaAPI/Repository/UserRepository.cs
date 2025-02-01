using MagicVilla_VillaAPI.DATA;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.LocalUser.FirstOrDefault(U => U.UserName == username);

            if (user == null)
            {
                return true;
            }

            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.LocalUser.FirstOrDefault(U => U.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
            && U.Password == loginRequestDTO.Password);

            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            //if user was found generate JWT token -> quando abbiamo generato un token JWT abbiamo bisongo di una chiave segreta
            //Utilizzando la chiave sefreta il nostor token verrà crittografato, la chiave segreta verrà utilizzata per convalidare
            //se il token è valido o meno perché ognuno può generare il token ma questa chiave segreta sarà usata per autenticare
            //se il token è stato generato dalla nostra API e se quel token è valido o meno, perché nessun altro conoscerà quel segreto
            //Solo la nostra applicazione avrà la chiave segreta. Quando dobbiamo aggiungere dei segreti lo facciamo nelle impostazioni

            //GENERARE IL SECURITY TOKEN

            /*
            Questa riga di codice crea un nuovo oggetto del tipo JwtSecurityTokenHandler, che è una classe presente
            nello spazio dei nomi System.IdentityModel.Tokens.Jwt della libreria .NET.
            Il JwtSecurityTokenHandler è utilizzato per creare, leggere, convalidare e scrivere token di sicurezza 
            JWT (JSON Web Tokens). I JWT sono comunemente usati per l'autenticazione e l'autorizzazione in applicazioni web.
            In parole povere, questa riga di codice inizializza uno strumento che ti permette di lavorare con i token JWT.
            */
            var tokenHandler = new JwtSecurityTokenHandler();

            /*
            Un token di sicurezza è un piccolo pezzo di dati digitale che viene usato per autenticare e autorizzare un utente
            o un'applicazione. Questi token contengono informazioni sulle credenziali dell'utente e altri dati rilevanti
            per la sicurezza. I token di sicurezza vengono spesso utilizzati nei sistemi di autenticazione basati su web,
            per garantire che solo gli utenti autorizzati possano accedere a risorse specifiche.
            Esistono diversi tipi di token di sicurezza, tra cui:
            JWT(JSON Web Token): Un formato compatto, sicuro e basato su standard per la rappresentazione di dichiarazioni
            da condividere tra due parti.Viene spesso utilizzato in applicazioni web per l'autenticazione e l'autorizzazione
            */

            /*
            Using Encoding.ASCII.GetBytes(secretKey) is a good way to convert your secretKey string into a byte array
            that can be used for the symmetric security key in the JwtSecurityTokenHandler
            */
            var key = Encoding.ASCII.GetBytes(secretKey);

            /*
            SecurityTokenDescriptor è una classe della libreria .NET che viene utilizzata per descrivere le proprietà di un 
            token di sicurezza.
            Questa classe contiene informazioni necessarie per creare un token di sicurezza, come ad esempio:
            Dichiarazioni (Claims): Informazioni sull'utente o l'entità a cui il token si riferisce.
            Firma (Signing): Dettagli su come il token sarà firmato per garantirne l'integrità.
            Crittografia (Encryption): Informazioni su come il token sarà crittografato per garantirne la riservatezza.
            Scadenza (Expiration): Data e ora di scadenza del token.
            Auditoria (Audience): Gli identificatori dei destinatari per cui il token è destinato.
            Questi dati vengono utilizzati dal JwtSecurityTokenHandler per creare un token JWT che può essere utilizzato
            per autenticazione e autorizzazione in applicazioni web.
            */
            //DESCRIVE LE PROPRIETà DI UN TOKEN DI SICUREZZA:
            //- CLAIMS
            //- SIGNING
            //- ENCRYPTION
            //- EXPIRATION
            //- AUDIENCE -> Gli identificatori dei destinatari per cui il token è destinato
            var tokenDescriptor = new SecurityTokenDescriptor();

            /*
            CLAIMS ->  ClaimsIdentity è una classe della libreria .NET che rappresenta l'identità di un'entità, come un utente,
            basata su un insieme di dichiarazioni (Claims). Questa classe è parte dello spazio dei nomi System.Security.Claims.
            Una dichiarazione (Claim) è una singola informazione di identità che afferma qualcosa riguardo all'entità,
            come il nome, il ruolo, l'email, ecc. Le identità basate su dichiarazioni sono molto flessibili e possono
            essere utilizzate in scenari di autenticazione e autorizzazione.
            */
            tokenDescriptor.Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            });

            /*
            La proprietà Expires della classe SecurityTokenDescriptor rappresenta la data e l'ora di scadenza
            del token di sicurezza. In altre parole, specifica fino a quando il token sarà considerato valido.
            Dopo la data e l'ora specificate in Expires, il token non sarà più accettato come valido e non potrà
            essere utilizzato per l'autenticazione o l'autorizzazione
            */

            tokenDescriptor.Expires = DateTime.UtcNow.AddDays(7);

            /*
            Imposta le credenziali di firma per il token. Utilizza una chiave simmetrica (SymmetricSecurityKey)
            e specifica l'algoritmo di firma HMAC-SHA256 (SecurityAlgorithms.HmacSha256Signature).
            In pratica, questa configurazione serve a garantire l'integrità e la sicurezza del token,
            firmandolo con una chiave segreta. Solo chi possiede questa chiave può generare un token valido,
            e chiunque riceva il token può verificarne l'integrità utilizzando la stessa chiave.
            */

            /*
            SecurityAlgorithms.HmacSha256Signature è una costante della libreria .NET che rappresenta l'algoritmo
            di firma HMAC-SHA256. Questo algoritmo è utilizzato per garantire l'integrità e l'autenticità dei dati firmati.
            HMAC-SHA256 combina un algoritmo di hash crittografico (SHA-256) con una chiave segreta per generare un
            codice di autenticazione del messaggio (MAC)

            Hashing con SHA-256: SHA-256 è un algoritmo di hash che prende un input di qualsiasi lunghezza e produce
            un output fisso di 256 bit. È ampiamente utilizzato per la sua sicurezza e resistenza alle collisioni
            (cioè due input diversi che producono lo stesso hash)

            Quando diciamo "due input diversi che producono lo stesso hash", ci riferiamo a una situazione in cui due dati distinti
            generano lo stesso valore di hash. Questo fenomeno è noto come collisione. In un buon algoritmo di hash,
            come SHA-256, le collisioni sono estremamente rare e difficili da ottenere intenzionalmente.
            Per fare un esempio semplice, supponiamo di avere due file diversi. Se applichiamo l'algoritmo di hash a ciascun file,
            otteniamo due valori hash. In un algoritmo di hash sicuro, questi valori hash dovrebbero essere unici per ogni file.
            Se entrambi i file producono lo stesso valore hash, significa che abbiamo una collisione.
            Le collisioni sono problematiche perché minano l'integrità e la sicurezza del sistema che utilizza l'algoritmo di hash.
            Ecco perché i moderni algoritmi di hash sono progettati per ridurre al minimo la probabilità di collisioni.
            */
            tokenDescriptor.SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            /*
            Questa riga di codice crea un token di sicurezza utilizzando il metodo CreateToken dell'oggetto JwtSecurityTokenHandler
            (tokenHandler) e il descrittore del token (tokenDescriptor). In pratica, combina tutte le informazioni 
            contenute nel SecurityTokenDescriptor, come le dichiarazioni (Claims), la chiave di firma,
            la data di scadenza, ecc., per generare un token JWT.
            */
            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO();

            //tokenHandler.WriteToken(token);

            //Questo metodo converte il token JWT(token) in una stringa.
            //La stringa rappresenta il token JWT in formato leggibile(ad esempio, una lunga stringa di caratteri).
            loginResponseDTO.Token = tokenHandler.WriteToken(token);
            loginResponseDTO.User = user;

            return loginResponseDTO;
        }

        public async Task<LocalUser> Register(RegistrationRequestDTO registerRequestDTO)
        {
            LocalUser user = new LocalUser();

            user.UserName = registerRequestDTO.UserName;
            user.Name = registerRequestDTO.Name;
            user.Password = registerRequestDTO.Password;
            user.Role = registerRequestDTO.Role;

            _db.LocalUser.Add(user);

            await _db.SaveChangesAsync();

            user.Password = "";

            return user;
        }
    }
}
