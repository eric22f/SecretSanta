using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Models
{
    public class MySecretSanta
    {
        public string UserCode { get; set; }
        [Required]
        [MaxLength(75)]
        [Display(Name = "Full Name")]
        public string SantaFullName { get; set; }
        public string MySantaFullName { get; set; }
        [MaxLength(2000)]
        [Display(Name = "Your address plus any gift hints or gift topics")]
        public string Address { get; set; }
        public string InfoMsg { get; set; }
        public bool IsViewedBySecretSanta { get; set; }
        public bool IsGiftSent { get; set; }
        public string ApiKey { get; set; }
    }
}
