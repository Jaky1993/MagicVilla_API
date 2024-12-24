using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.DTO
{
    //la creazione di VillaNumberCreateDTO e VillaNumberUpdateDTO serve per evitare problemi in futuro se ci sono delle divergenze tra UpdateDTO e createDTO 
    //anche se per ora sono uguali ma meglio farlo ora anche se sono uguali rispetto a dover cambiare le cose in futuro
    public class VillaNumberCreateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }
        [Required]
        public int VillaID { get; set; }
    }
}
