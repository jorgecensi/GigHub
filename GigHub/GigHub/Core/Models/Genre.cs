namespace GigHub.Core.Models
{
    public class Genre
    {
        public byte Id { get; set; } //is byte because I believe that we are not going to have more than 255 genres

        public string Name { get; set; }

    }
}