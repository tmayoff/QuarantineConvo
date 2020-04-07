using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuarantineConvo.Models;

namespace QuarantineConvo.Data
{
    public class QuarantineConvoContext : DbContext
    {
        public QuarantineConvoContext (DbContextOptions<QuarantineConvoContext> options)
            : base(options)
        {
        }

        public DbSet<QuarantineConvo.Models.Interest> Interest { get; set; }

        public DbSet<QuarantineConvo.Models.Connection> Connection { get; set; }

        public DbSet<QuarantineConvo.Models.Message> Message { get; set; }

        public DbSet<QuarantineConvo.Models.User> User { get; set; }
    }
}
