using System;
using System.Collections.Generic;
using System.Text;
using RedBadgeStarter.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace RedBadgeStarter.Database
{
    public class RBStarterContext : DbContext
    {
        public RBStarterContext(DbContextOptions<RBStarterContext> options) : base(options) { }

        public DbSet<UserEntity> UserTableAccess { get; set; }
    }
}
