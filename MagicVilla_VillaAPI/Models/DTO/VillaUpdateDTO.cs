using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.DTO
{
    public class VillaUpdateDTO
    {
        //DTO forniscono un wrapper tra le entity del database e ciò che viene esposto dalle API
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        //Se tolgo [ApiController] questi attributi spariscono, per tenerli devo fare un if(ModelState.IsValid)
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [Required]
        public int Sqft { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
