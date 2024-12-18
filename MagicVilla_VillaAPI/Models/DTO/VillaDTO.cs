using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.DTO
{
    public class VillaDTO
    {
        //DTO forniscono un wrapper tra le entity del database e ciò che viene esposto dalle API
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        //Se tolgo [ApiController] questi attributi spariscono, per tenerli devo fare un if(ModelState.IsValid)
        public string Name { get; set; }
    }
}
