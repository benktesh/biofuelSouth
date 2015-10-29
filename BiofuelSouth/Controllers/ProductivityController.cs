using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BiofuelSouth.Models;


namespace BiofuelSouth.Controllers
{
    [System.Web.Mvc.RoutePrefix("api/productivity")]
    public class ProductivityController : ApiController

    {
        private DatabaseContext db = new DatabaseContext();

        /**
         * Tis is used to seed the data
         * namespace BiofuelSouth.Migrations
    {
        using BiofuelSouth.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

        internal sealed class Configuration : DbMigrationsConfiguration<BiofuelSouth.Controllers.DbContext>
        {
            public Configuration()
            {
                AutomaticMigrationsEnabled = true;
            }

            protected override void Seed(BiofuelSouth.Controllers.DbContext context)
            {
                //  This method will be called after migrating to the latest version.

                //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
                //  to avoid creating duplicate seed data. E.g.
                //
                //    context.People.AddOrUpdate(
                //      p => p.FullName,
                //      new Person { FullName = "Andrew Peters" },
                //      new Person { FullName = "Brice Lambson" },
                //      new Person { FullName = "Rowan Miller" }
                //    );
                //
                 var counties = new List<County>
                {
                        new County { Id =1, CountyCode=1, Lat=36.1048456, Lon=-84.195796, Name="Anderson", State="TN"},
                        new County { Id =2, CountyCode=3, Lat=35.5108249, Lon=-86.4500419, Name="Bedford", State="TN"},
                        new County { Id =3, CountyCode=5, Lat=35.1750149, Lon=-84.652623, Name="Benton", State="TN"}
                };

                counties.ForEach(s => context.County.AddOrUpdate(p => p.Id, s));
                context.SaveChanges();


                var productivities = new List<Productivity>                
                {
                    new Productivity { Id = 1, CountyId=counties.Single(s => s.Id ==1).Id, CropType = "Forest Residue", Yield = 100},
                    new Productivity { Id = 2, CountyId=counties.Single(s => s.Id ==1).Id, CropType = "Switch Grass", Yield = 50},
                    new Productivity { Id = 3, CountyId=counties.Single(s => s.Id ==1).Id, CropType = "Crop Residue", Yield = 10},
                    new Productivity { Id = 4, CountyId=counties.Single(s => s.Id ==1).Id, CropType = "Poplar", Yield = 75},

                    new Productivity { Id = 5, CountyId=counties.Single(s => s.Id ==2).Id, CropType = "Forest Residue", Yield = 110},
                    new Productivity { Id = 6, CountyId=counties.Single(s => s.Id ==2).Id, CropType = "Switch Grass", Yield = 55},
                    new Productivity { Id = 7, CountyId=counties.Single(s => s.Id ==2).Id, CropType = "Crop Residue", Yield = 12.5m},
                    new Productivity { Id = 8, CountyId=counties.Single(s => s.Id ==2).Id, CropType = "Poplar", Yield = 75m}
                };
                productivities.ForEach(s => context.Productivity.AddOrUpdate(p => p.Id, s));
                context.SaveChanges();
            }

        
        }
    }

             * 
             * /


            static List<County> counties = new List<County>  
                {
                    new County { Id =1, CountyCode=1, Lat=36.1048456, Lon=-84.195796, Name="Anderson", State="TN"},
                    new County { Id =2, CountyCode=3, Lat=35.5108249, Lon=-86.4500419, Name="Bedford", State="TN"},
                    new County { Id =3, CountyCode=5, Lat=35.1750149, Lon=-84.652623, Name="Benton", State="TN"}
                };


            static Productivity[] productivity = new Productivity[] 
            {
                new Productivity { Id = 1, CountyId=1, CropType = "Forest Residue", Yield = 100},
                new Productivity { Id = 2, CountyId=1, CropType = "Switch Grass", Yield = 50},
                new Productivity { Id = 3, CountyId=1, CropType = "Crop Residue", Yield = 10},
                new Productivity { Id = 4, CountyId=1, CropType = "Poplar", Yield = 75},

                new Productivity { Id = 5, CountyId=2, CropType = "Forest Residue", Yield = 110},
                new Productivity { Id = 6, CountyId=2, CropType = "Switch Grass", Yield = 55},
                new Productivity { Id = 7, CountyId=2, CropType = "Crop Residue", Yield = 12.5m},
                new Productivity { Id = 8, CountyId=2, CropType = "Poplar", Yield = 75m}
            };

        */

        // GET api/Productivity
        public IEnumerable<Productivity> GetProductivities()
        {
            //Debug.WriteLine(db.Productivity);
            return db.Productivities;
        }
        

        // GET api/Productivity
      /*
        public IEnumerable<Productivity> GetAllProductivity()
        {
            return db.Productivity;
           // var x = db.Productivity;
           // Console.WriteLine(x.ToString());
            //return db.Productivity;
            //return new Productivity[] { new Productivity { Id = 4, CountyId = 2, CropType = "Forest Residue", Yield = 110 } };
        }
        */

        // GET api/Productivity/5
        [System.Web.Http.Route("{id:int}")]
        [ResponseType(typeof(Productivity))]
        public IEnumerable<Productivity> GetProductivity(int id)
            //Have not found async implemenation solution yet.
            //public async Task<IEnumerable<Productivity>> GetProductivity(int id)
        {
            var countyProductivity = db.Productivities.Where(p => p.GeoId == id);
            return countyProductivity;
            /*
            Productivity productivity = await db.Productivity.FindAsync(id);
            if (productivity == null)
            {
                return NotFound();
            }

            db.Productivity.f
            return db.Productivity;
             */
             
             
        }

        // PUT api/Productivity1/5

        public async Task<IHttpActionResult> PutProductivity(int id, Productivity productivity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productivity.Id)
            {
                return BadRequest();
            }

            db.Entry(productivity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductivityExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Productivity
      
        [ResponseType(typeof(Productivity))]
        public async Task<IHttpActionResult> PostProductivity(Productivity productivity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Productivities.Add(productivity);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = productivity.Id }, productivity);
        }

        // DELETE api/Productivity1/5
        [ResponseType(typeof(Productivity))]
        public async Task<IHttpActionResult> DeleteProductivity(int id)
        {
            var productivity = await db.Productivities.FindAsync(id);
            if (productivity == null)
            {
                return NotFound();
            }

            db.Productivities.Remove(productivity);
            await db.SaveChangesAsync();

            return Ok(productivity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductivityExists(int id)
        {
            return db.Productivities.Count(e => e.Id == id) > 0;
        }
    }
}