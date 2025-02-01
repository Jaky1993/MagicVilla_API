namespace MagicVilla_VillaAPI.Models.DTO
{
    public class RegistrationRequestDTO
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        //Ruolo con cui si registra l'utente
        public string Role { get; set; }

        //Quando la registrazione è andata a buon fine torneremo Status 200 OK ciè che tutto è andato a buon fine
        //Quindi non è necessario creare un DTO di risposta alla registrazione
    }
}
