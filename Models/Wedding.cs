using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingProj.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId { get; set; }

        [Required(ErrorMessage = "Is required!")]
        [MinLength(3, ErrorMessage = "Must be greater than 3!")]
        [Display(Name = "Wedder1 Name")]
        public string Wedder1Name { get; set; }

        [Required(ErrorMessage = "Is required!")]
        [MinLength(3, ErrorMessage = "Must be greater than 3!")]
        [Display(Name = "Wedder2 Name")]
        public string Wedder2Name { get; set; }

        [Required(ErrorMessage = "Is required!")]
        [MinLength(3, ErrorMessage = "Must be greater than 3!")]
        [Display(Name = "Wedding Address")]
        public string WeddingAddress { get; set; }

        [Required(ErrorMessage = "Is required!")]
        [DataType(DataType.Date)]
        [Display(Name = "Wedding Date")]
        public DateTime WeddingDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<UserWeddingRSVP> RSVP { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string WeddingName()
        {
            return Wedder1Name + " & " + Wedder2Name;
        }
    }
}