using System;
using System.Collections.Generic;

#nullable disable

namespace SecretSanta.Models
{
    public partial class SelectedSanta
    {
        public int SantaId { get; set; }
        public int SelectedSantaId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
