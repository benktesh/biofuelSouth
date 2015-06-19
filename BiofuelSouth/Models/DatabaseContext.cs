using System.Data.Entity;
using System.Data.Entity.SqlServer;
using BiofuelSouth.Models.Entity;


namespace BiofuelSouth.Models
{
    class DatabaseContext : DbContext
    {

        public DatabaseContext() : base("DefaultConnection")
        {
            var ensureDLLIsCopied = SqlProviderServices.Instance;
        }
        
        public DbSet<Productivity> Productivities { get; set; }
        public DbSet<County> County { get; set; }
        public DbSet<Glossary> Glossaries { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
    
        public DbSet<Input> Inputs { get; set; }
        public DbSet<LookUpEntity> LookUps { get; set; }
    }

 
}
