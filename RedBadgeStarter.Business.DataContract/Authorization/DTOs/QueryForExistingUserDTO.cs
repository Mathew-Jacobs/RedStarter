﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RedBadgeStarter.Business.DataContract.Authorization.DTOs
{
    public class QueryForExistingUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
