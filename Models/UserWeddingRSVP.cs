using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingProj.Models
{
    public class UserWeddingRSVP
    {
        [Key]
        public int RSVPId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User User { get; set; }
        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; }

    }
}