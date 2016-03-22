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
        
        public DbSet<ProductivityEntity> Productivities { get; set; }
        public DbSet<CountyEntity> County { get; set; }
        public DbSet<Entity.GlossaryEntity> Glossaries { get; set; }
        public DbSet<FeedBackEntity> FeedBacks { get; set; }
    
        //public DbSet<Input> Inputs { get; set; }
        public DbSet<LookUpEntity> LookUps { get; set; }
    }

 
}
