using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Speurzoekers.Data.Entities.Identity;
using System;

namespace Speurzoekers.Data
{
    public class SpeurzoekersDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public SpeurzoekersDbContext()
            :base()
        {

        }

        public SpeurzoekersDbContext(DbContextOptions options)
            : base(options)
        {

        }
    }
}
