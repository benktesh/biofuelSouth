using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.DynamicData;
using BiofuelSouth.Models;


namespace BiofuelSouth.Models
{
    class DatabaseContext : DbContext
    {

        public DatabaseContext() : base("DefaultConnection")
        {
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        
        
        }
        
        public DbSet<Productivity> Productivities { get; set; }
        public DbSet<County> County { get; set; }
        public DbSet<Glossary> Glossaries { get; set; }

        public System.Data.Entity.DbSet<BiofuelSouth.Models.Input> Inputs { get; set; }
    }


}
