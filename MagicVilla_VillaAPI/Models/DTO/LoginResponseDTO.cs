namespace MagicVilla_VillaAPI.Models.DTO
{
    public class LoginResponseDTO
    {
        //LoginResponseDTO avrà tutti i dati dell'utente
        public UserDTO User { get; set; }

        //Utilizzato per autenticare e convalidare l'identità dell'utente
        public string Token { get; set; }
        public string Role  { get; set; }
    }
}
