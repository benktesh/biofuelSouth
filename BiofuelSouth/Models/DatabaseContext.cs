using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using BiofuelSouth.Models;


namespace BiofuelSouth.Controllers
{
    class DatabaseContext : DbContext
    {

        public DatabaseContext() : base("DefaultConnection")
        {
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        public DbSet<Productivity> Productivity { get; set; }
        public DbSet<County> County { get; set; }

        public System.Data.Entity.DbSet<BiofuelSouth.Models.Input> Inputs { get; set; }
    }
}
