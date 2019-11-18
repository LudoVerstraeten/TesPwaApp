using System;

namespace Speurzoekers.Common.Domain.User
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string[] Roles { get; set; }
    }
}
