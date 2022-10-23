using LeagueApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueApp.Domain.DTOS
{
    public class BaseResponse
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class UserResponse : BaseResponse
    {
        public List<string> Roles { get; set; }
        public User User { get; set; }
    }
}
