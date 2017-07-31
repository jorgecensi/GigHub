using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    public class Genre
    {
        public byte Id { get; set; } //is byte because I believe that we are not going to have more than 255 genres

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

    }
}