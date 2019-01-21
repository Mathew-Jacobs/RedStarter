using System;
using System.Collections.Generic;
using System.Text;

namespace RedBadgeStarter.Database.DataContract.Authorization.RAOs
{
    public class ReceivedExistingUserRAO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }
}
