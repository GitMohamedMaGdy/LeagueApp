using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueApp.Domain.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual Image PlayerLogo { get; set; }
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}
