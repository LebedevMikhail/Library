using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        [Column("name")]
        [Required]
        public string Name { get; set; }
        [Column("minAge")]
        [Required]
        public int MinAge { get; set; }
        public Genre(int genreId, string name, int minAge)
        {
            GenreId = genreId;
            Name = name;
            MinAge = minAge;
        }

        public override bool Equals(object obj)
        {
            return obj is Genre genre &&
                   GenreId == genre.GenreId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GenreId, Name, MinAge);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

}
