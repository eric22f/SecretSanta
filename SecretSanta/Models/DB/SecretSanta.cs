using System;
using System.Collections.Generic;

#nullable disable

namespace SecretSanta.Models
{
    public partial class SecretSanta
    {
        public int SantaId { get; set; }
        public string FullName { get; set; }
        public string AddressAndNotes { get; set; }
        public bool Selected { get; set; }
        public string UserCode { get; set; }
        public int TeamId { get; set; }
        public bool SentGift { get; set; }
    }
}
