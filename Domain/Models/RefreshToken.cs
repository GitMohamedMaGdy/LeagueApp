using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueApp.Domain.Models
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string ClientId { get; set; }
        public DateTime? IssuedUtc { get; set; }
        public DateTime? ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
    }
}
