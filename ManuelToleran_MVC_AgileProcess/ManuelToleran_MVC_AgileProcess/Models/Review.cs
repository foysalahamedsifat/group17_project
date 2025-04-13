using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManuelToleran_MVC_AgileProcess.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }

        public string? User { get; set; }

        public DateTime Date { get; set; }

        // Foreign Key
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }
    }
}
