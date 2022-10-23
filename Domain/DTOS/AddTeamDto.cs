using LeagueApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueApp.Domain.DTOS
{
    public class AddTeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryName { get; set; }
        public DateTime FoundationDate { get; set; }
        public string CoachName { get; set; }
        public virtual Image TeamLogo { get; set; }
        public IEnumerable<Player> Players { get; set; }


    }
}
