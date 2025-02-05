﻿using AutoMapper;
using MagicVilla_VillaAPI.DATA;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _mapper = mapper;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(U => U.UserName == username);

            if (user == null)
            {
                return true;
            }

            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(U => U.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            /*
            Il JwtSecurityTokenHandler è utilizzato per creare, leggere, convalidare e scrivere token di sicurezza 
            JWT (JSON Web Tokens). I JWT sono comunemente usati per l'autenticazione e l'autorizzazione in applicazioni web.
            INIZIALIZZO LO STRUMENTO CHE MI PERMETTE DI LAVORARE CON I TOKEN    
            */
            var tokenHandler = new JwtSecurityTokenHandler();

            /*
            Using Encoding.ASCII.GetBytes(secretKey) is a good way to convert your secretKey string into a byte array
            that can be used for the symmetric security key in the JwtSecurityTokenHandler
            */
            var key = Encoding.ASCII.GetBytes(secretKey);

            //SecurityTokenDescriptor è una classe della libreria .NET che viene utilizzata per descrivere le proprietà
            //di un token di sicurezza
            var tokenDescriptor = new SecurityTokenDescriptor();

            /*
            CLAIMS ->  ClaimsIdentity è una classe della libreria .NET che rappresenta l'identità di un'entità, come un utente,
            basata su un insieme di dichiarazioni (Claims)
            */
            tokenDescriptor.Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault())
            });

            /*
            La proprietà Expires della classe SecurityTokenDescriptor rappresenta la data e l'ora di scadenza
            del token di sicurezza. In altre parole, specifica fino a quando il token sarà considerato valido.
            */
            tokenDescriptor.Expires = DateTime.UtcNow.AddDays(7);

            /*
            Imposta le credenziali di firma per il token. Utilizza una chiave simmetrica (SymmetricSecurityKey)
            e specifica l'algoritmo di firma HMAC-SHA256 (SecurityAlgorithms.HmacSha256Signature).
            In pratica, questa configurazione serve a garantire l'integrità e la sicurezza del token, firmandolo con una chiave segreta.
            Solo chi possiede questa chiave può generare un token valido,
            e chiunque riceva il token può verificarne l'integrità utilizzando la stessa chiave.
            SecurityAlgorithms.HmacSha256Signature è una costante della libreria .NET che rappresenta l'algoritmo di firma HMAC-SHA256.
            Questo algoritmo è utilizzato per garantire l'integrità e l'autenticità dei dati firmati.
            HMAC-SHA256 combina un algoritmo di hash crittografico (SHA-256) con una chiave segreta per generare un codice di autenticazione del messaggio (MAC)
            Hashing con SHA-256: SHA-256 è un algoritmo di hash che prende un input di qualsiasi lunghezza e produce un output fisso di 256 bit. È ampiamente utilizzato per la sua sicurezza e resistenza alle collisioni
            (cioè due input diversi che producono lo stesso hash) 
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

            //Questo metodo converte il token JWT(token) in una stringa.
            //La stringa rappresenta il token JWT in formato leggibile(ad esempio, una lunga stringa di caratteri).
            loginResponseDTO.Token = tokenHandler.WriteToken(token);
            loginResponseDTO.User = _mapper.Map<UserDTO>(user);

            return loginResponseDTO;
        }

        public async Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new ApplicationUser();

            user.UserName = registrationRequestDTO.UserName;
            user.Email = registrationRequestDTO.UserName;
            user.NormalizedEmail = registrationRequestDTO.UserName.ToUpper();
            user.Name = registrationRequestDTO.Name;

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);

                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        //JWT.IO per vedere il token
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }

                    await _userManager.AddToRoleAsync(user, "admin");

                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == registrationRequestDTO.UserName);

                    return _mapper.Map<UserDTO>(userToReturn);
                }
            }
            catch (Exception ex)
            {

            }

            return new UserDTO();
        }
    }
}
