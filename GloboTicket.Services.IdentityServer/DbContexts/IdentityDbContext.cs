using GloboTicket.Services.IdentityServer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.Services.IdentityServer.DbContexts
{
    public class IdentityDbContext:DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
    }
}
