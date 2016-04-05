using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Remoting.Channels;
using System.Web.Hosting;
using BiofuelSouth.Enum;
using BiofuelSouth.Models;
using BiofuelSouth.Models.Entity;
using BiofuelSouth.ViewModels;
using GlossaryEntity = BiofuelSouth.Models.Entity.GlossaryEntity;

namespace BiofuelSouth.Services
{
    public static class DataService
    {

        public static List<LookUpEntity> GetLookUps(string group = null)
        {
            using (var db = new DatabaseContext())
            {
                if (!string.IsNullOrEmpty(group))
                    return db.LookUps.Where(p => p.LookUpGroup == group).ToList();
                return db.LookUps.ToList();
            }

        }

        public static List<string> GetCounty(String state)
        {
            using (var db = new DatabaseContext())
            {

                return db.County.Where(p => p.State == state).Select(p => p.Name).ToList();

            }
        }

        public static List<double?> GetLatLong(string countyid)
        {
            using (var db = new DatabaseContext())
            {

                var county = db.County.FirstOrDefault(p => p.GeoId == countyid);

                if (county != null)
                {
                    return new List<double?> { county.Lat, county.Lon };
                }

                return null;
            }
        }

        public static CountyEntity GetCountyById(string countyid) //or geoid
        {
            using (var db = new DatabaseContext())
            {

                var county = db.County.FirstOrDefault(p => p.GeoId == countyid);
                return county;
            }
        }

        //TODO 
        /// <summary>
        /// This method needs to call database. For now, a hardcoded value is used.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool VerifyToken(Guid? x)
        {
            return x.Equals(Guid.Parse("1afe36fa-e1f3-406b-9c73-914ec23ec2be"));

        }

        public static List<CountyEntity> GetCountyData(string selectedCategory = "ALL")
        {
            using (var db = new DatabaseContext())
            {
                if (selectedCategory == "ALL")
                {
                    var data = db.County.ToList();
                    return data;

                }
                return db.County.Where(p => p.State == selectedCategory).ToList();
            }
        }


        //based on dickens 2011
        //      the two eucalyptus species(31.9 and 35.5 tons/ac/yr), respectively E.grandis and E. amplifolia
        //loblolly pine (12.9 tons/ac/yr), 
        //hybrid poplar(7.6 tons/ac/yr), 
        //cottonwood(7.2 tons/ac/yr), 
        //sycamore(6.6 tons/ac/yr),
        //slash pine(6.5 tons/ac/yr), 
        //sweetgum(4.2 tons/ac/yr). 


        public static double GetProductivityPerAcreForCropByGeoId(CropType cropType, String geoId)
        {
            var intGeoid = Convert.ToInt32(geoId);
            using (var db = new DatabaseContext())
            {
                if (cropType == CropType.Pine)
                {
                    return 12.9; //Dickens (2011)
                }
                if (cropType == CropType.Poplar)
                {
                    return 7.6;
                }
                if (cropType == CropType.Willow)
                {
                    return 15.0;
                }


                var productivity = db.Productivities.Where(p => p.GeoId == intGeoid && p.CropType == cropType).Select(p => p.Yield).FirstOrDefault();
                return productivity * 0.446; //MG/ha -> t/acre
            }
        }

        public static double GetCostPerAcreForCropByGeoId(CropType cropType, String geoId)
        {

            var intGeoid = Convert.ToInt32(geoId);
            using (var db = new DatabaseContext())
            {
                if (cropType == CropType.Pine || cropType == CropType.Willow || cropType == CropType.Poplar)
                {
                    return 30; //Dickens (2011)
                }

                var productivity = db.Productivities
                    .Where(p => p.GeoId == intGeoid && p.CropType == cropType)
                    .Select(p => p.Cost).FirstOrDefault();
                return productivity / 2.471; //conver to $/ha -> $/acre
            }
        }


        public static GlossaryEntity GetGlossary(String term)
        {
            using (var db = new DatabaseContext())
            {

                var result = db.Glossaries.FirstOrDefault(m => m.Term.Equals(
                    term, StringComparison.InvariantCultureIgnoreCase));
                return result;
            }
        }

        public static GlossaryEntity GetGlossaryById(Guid id)
        {
            using (var db = new DatabaseContext())
            {

                var result = db.Glossaries.FirstOrDefault(m => m.Id == id);
                return result;
            }
        }

        public static void DeleteGlossary(Guid id)
        {
            using (var db = new DatabaseContext())
            {
                db.Glossaries.Remove(db.Glossaries.FirstOrDefault(b => b.Id == id));
                db.SaveChanges();
            }
        }


        public static IEnumerable<GlossaryEntity> GetGlossary(int? count = null)
        {
            using (var db = new DatabaseContext())
            {

                if (count != null)
                {
                    return new List<GlossaryEntity>(db.Glossaries.OrderByDescending(p => p.Counter).Take(count.Value)).ToList();
                }
                return db.Glossaries.ToList();
            }
        }

        public static void SaveFeedback(FeedBackEntity fb)
        {

            using (var db = new DatabaseContext())
            {
                db.FeedBacks.Add(fb);
                db.SaveChanges();
            }
        }

        public static List<GlossaryTransferMmodel> GetAllTerms(string key)
        {
            using (var db = new DatabaseContext())
            {
                var mx = db.Glossaries.Max(p => p.Counter);
                var mn = db.Glossaries.Min(p => p.Counter);
                if (mx - mn <= 0)
                {
                    mx = 1;
                    mn = 0;
                }

                if (key == null || key.Equals("") || key.Equals("All"))
                {

                    var data = db.Glossaries.Select(p => new { Key = p.Term, Value = p.Counter ?? 0 })
                        .OrderBy(m => m.Key)
                        .ToList();


                    return
                        data.Select(
                            pair =>
                                new GlossaryTransferMmodel
                                {
                                    Key = pair.Key,
                                    Value = Math.Min(300, (int)(1 + 2 * (pair.Value - mn) / (mx - mn)) * 100),
                                }).ToList();
                };


                key = key.Replace("-", null);

                new Dictionary<string, int>();
                var data1 = new List<GlossaryTransferMmodel>();
                foreach (var element in key.ToCharArray())
                {
                    if (string.IsNullOrEmpty(element.ToString())) continue;
                    var startWith = element.ToString(CultureInfo.InvariantCulture).ToLower();


                    var resultList = db.Glossaries.Where(d => d.Term.ToLower().StartsWith(startWith))
                        .Select(p => new GlossaryTransferMmodel { Key = p.Term, Value = p.Counter ?? 0 });

                    data1.AddRange(resultList);

                }

                //mx = data1.Max(p => p.Value);
                //mn = data1.Min(p => p.Value);
                //if (mx - mn <= 0)
                //{
                //    mx = 1;
                //    mn = 0;
                //}

                return data1.Select(
                            pair =>
                                new GlossaryTransferMmodel
                                {
                                    Key = pair.Key,
                                    Value = Math.Min(300, (int)(1 + 2 * (pair.Value - mn) / (mx - mn)) * 100),
                                }).ToList();
            }

        }

        public class GlossaryTransferMmodel
        {
            public int Value { get; set; }
            public string Key { get; set; }
        }

        public static void SaveGlossary(GlossaryEntity gm)
        {

            using (var db = new DatabaseContext())
            {
                gm.Created = DateTime.UtcNow;
                db.Glossaries.Add(gm);
                db.SaveChanges();

            }

        }

        public static void UpdateGlossary(GlossaryViewModel gvm)
        {
            using (var db = new DatabaseContext())
            {
                var glossary = db.Glossaries.FirstOrDefault(b => b.Id == gvm.MId);

                if (glossary != null)
                {
                    glossary.Term = gvm.Term;
                    glossary.Keywords = gvm.Keywords;
                    glossary.Description = gvm.Description;
                    glossary.Source = gvm.Source;
                    glossary.ModifiedBy = gvm.AdminToken.ToString();
                    glossary.Modified = DateTime.UtcNow;

                    db.Glossaries.AddOrUpdate(glossary);
                    db.SaveChanges();

                }

            }
        }

        public static void DeleteGlossary(string term)
        {
            using (var db = new DatabaseContext())
            {
                db.Glossaries.Remove(db.Glossaries.FirstOrDefault(b => b.Term.Equals(term, StringComparison.InvariantCultureIgnoreCase)));
                db.SaveChanges();

            }
        }

        public static List<GlossaryEntity> Search(String term)
        {
            using (var db = new DatabaseContext())
            {
                var resultList = new List<GlossaryEntity>();
                var result = db.Glossaries.FirstOrDefault(p => p.Term.Equals(term, StringComparison.InvariantCultureIgnoreCase));
                IEnumerable<GlossaryEntity> moreResults = new List<GlossaryEntity>();
                if (result == null)
                {
                    //Make a new logic

                    result = new GlossaryEntity(term.ToLower(), "", "", "");

                    result.Counter = 1;
                    result.Description = "TBI";//TO BE IMPLEMENTED
                    result.Keywords = "TBI";
                    result.Source = "TBI";
                    db.Glossaries.Add(result);
                    db.SaveChanges();
                    resultList.Add(result);

                }
                else
                {
                    if (result.Counter == null)
                        result.Counter = 0;
                    result.Counter = result.Counter + 1;
                    db.Entry(result).State = EntityState.Modified;
                    db.SaveChanges();


                    resultList.Add(result);
                    //add to result the records for which we have keywords.
                    //1. get keywords for the term -Done
                    //2. parse keywords into list  - Done
                    //3. find all the records where keywords are 'terms'
                    char[] delimiters = { ',', ';' };

                    var kw = result.Keywords;
                    if (kw != null)
                    {
                        IList<String> keywords =
                        result.Keywords.ToLower().Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
                        moreResults = db.Glossaries.Where(k => keywords.Contains(k.Term.ToLower()));

                    }

                    //add to more results

                    //1. find all the records where term is keyword
                    IEnumerable<GlossaryEntity> evenMoreResults = db.Glossaries.Where(k => k.Keywords.ToLower().Contains(term.ToLower()));

                    //Merge results
                    moreResults = moreResults.Union(evenMoreResults).OrderBy(x => x.Term);

                }


                resultList.AddRange(moreResults);

                return resultList;

            }
        }

        #region Factsheet

        public static FactsheetViewModel GetFactsheetViewModel(CropType crop)
        {
            var model = new FactsheetViewModel { CropType = crop };
            if (crop == CropType.Switchgrass)
            {
                model.Title = "Switchgrass";
                model.SubTitle = "Panicum virgatum";
                model.Description =
                    @"Switchgrass(Panicum virgatum) is a warm - season perennial grass native to the United States.Its rapid growth rate, abundant biomass and adaptation to different sites varieties have made it a prime candidate for development as a biofuel feedstock.Switchgrass can grow up to 10 feet in height and creates root networks that on can extend between 3 - 10 feet belowground.Most of its growth occurs between April and October, and it produces a new flush of tillers every spring.Tolerant of drought, low soil pH, and low nutrient levels, switchgrass has proven to be both hardy and low - maintenance, and can grow on more marginal lands.";
                model.GeographicDistribution =
                    @"Switchgrass can be found natively in almost all regions of the United States.It is most abundant in the tallgrass prairies of the Great Plains regions east of the Rocky Mountains and extends north to Canada and south to Mexico.Although switchgrass is more common east of the Rocky Mountains, it can be found in the western U.S. in all states except for California, Washington, and Oregon. 
                    <br /> <br />Switchgrass has a number of cultivars that each thrive in different climatic regions and altitudes, with some varieties characterized as upland or lowland.Alamo and Kanlo cultivars are lowland varieties, which are more suitable for the South. Lowland cultivars grow taller with heights of 9 - 10 feet(vs.upland cultivar heights of 5 - 6 feet).These lowland varieties produce more biomass per acre then the upland varieties as well.These cultivars are able to produce superior yields in areas with milder winters and high annual rainfall, most notably across much of the Southeast.Switchgrass can grow on a variety of soil types and can thrive even on soil that is too erodible for the production of traditional crops, such as corn.";
                model.ProductionProcess = @"Managing for switchgrass is a relatively simple process given the low degree of maintenance required and the ability to use common farm equipment for site preparation and harvest.Switch grass does well on moderately well - to well - drained soils with a soil pH at 5.0 or above, and medium fertility levels.Switchgrass can be planted using no - till or conventional tillage methods. Planting should begin when soil temperatures are consistently above 65°F, as early as mid - March and up to mid-June for the Southeast. Seed should be spread at a rate of 5 - 6 pounds of pure live seed per acre onto firmed seedbeds using either seed drills or broadcast spreaders. If a soil test shows low levels of phosphorous and potassium, then fertilizer containing phosphorus, potassium, and micronutrients(but not nitrogen) can aid in the establishment and early growth of switchgrass stands. Nitrogen fertilizer can later be added after successful establishment of the plants to further supplement growth; if nitrogen is added too early, it can stimulate weed growth and hinder the establishment of the switchgrass.Because the risk of weeds outcompeting the switchgrass seedlings is so high, herbicides should be applied both before and after planting, and the site should be monitored often to identify any weed issues.
                    <br /> <br />
                    The establishment of switchgrass can often take several months, and peak production may not be reached until the third year after planting, which is when it reaches maturity.After becoming completely established, switchgrass stands can produce consistent yields throughout its ten to twenty-year lifespan.Switchgrass should typically be harvested in the fall from as early as October (for a poorly drained field) through mid-February in the South. Typically, the switchgrass is mowed in the first part of the harvest period, and allowed to “cure,” to a desirable moisture content before being baled.  Managing moisture content is a key component of switchgrass harvesting.It is recommended to cut switchgrass to a minimum height of six inches, and within the range of 6 - 10 inches, in order to maintain stand density and prevent damage to the crown of the plant.";
                model.Yield = @"Switchgrass yields are dependent on both climate and management intensity.Optimum yields can be reached after three years, as the first year of growth includes stand establishment and controlling weeds. Within the first year, a 30 % yield is expected, rising to a 70 % yield in the second year and a 100 % yield in the third year.Choosing the right cultivar plant in any given region is also very important in maximizing yields. Information on where to find the right cultivar can be found by contacting county extension agents or state cooperative extension offices.Switchgrass seeds of various cultivars, including Alamo, are available commercially from numerous agriculture supply companies.";
                model.Conclusion = @"Switchgrass has excellent potential as a feedstock for bioenergy.Its fast growth and harvest rate, abundant biomass and ability to grown on a wide range of sites make it a desirable crop for bioenergy.For more information on growing switchgrass in the Southeastern United States, please refer to the Guidebook for Sustainable Production Practices of Switchgrass in the Southeastern U.S.";

                model.FactsForQuickReference = new Dictionary<string, string>();
                model.FactsForQuickReference.Add("Growing cycle",
                    "Grows as a perennial grass, harvested at minimum height of six inches, then regrows from root crown");
                model.FactsForQuickReference.Add("Planting stock", "Commercially available seed");
                model.FactsForQuickReference.Add("Age at first harvest", "Harvesting is recommended  after the third year");
                model.FactsForQuickReference.Add("Yields per Acre", "7 to 12 tons per acre(after the third year)");
                model.FactsForQuickReference.Add("Heating Value", "8014 BTU / pound");

                model.PreparedBy = "Connor Mcdonald, Leslie Boby and Bill Hubbard.";
                model.References = new List<String>();
                model.References.Add("Drinnon, D., McCord, J., Goddard, K.and Walton, J. 2012.Guidebook for the Sustainable Production Practices of Switchgrass in the Southeastern U.S.");
                model.References.Add("Jacobson, M. 2013.Penn State Extension, Renewable and Alternative Energy Fact Sheet - NewBio Energy Crop Profile: Switchgrass.<a href='http://extension.psu.edu/publications/ee0080'>http://extension.psu.edu/publications/ee0080.</a>");
                model.References.Add("USDA - NRCS. Planting and Managing Switchgrass as a Biomass Energy Crop. <a href='www.nrcs.usda.gov/Internet/FSE_DOCUMENTS/stelprdb1042293.pdf'>www.nrcs.usda.gov/Internet/FSE_DOCUMENTS/stelprdb1042293.pdf.</a>");
            }
            else if (crop == CropType.Miscanthus)
            {
                model.Title = "Miscanthus";
                model.SubTitle = "Miscanthus sp.";
                model.Description =
                    "<i>Miscanthus</i> is a genus comprised of twelve perennial grass species that are native to Asia and were " +
                    "introduced to the United States as ornamental plants in the nineteenth century. " +
                    "This tall reed, or canelike plant, is a close relative to sugarcane and has the ability to withstand " +
                    "cold conditions and poor soils. Its growth potential and high yield make it a prime candidate for biomass production. " +
                    "Giant miscanthus (<span><i>Miscanthus giganteus</i></span>), " +
                    "the species most commonly used for bioenergy, is actually a " +
                    "sterile hybrid of two species, <span><i>Miscanthus sinensis</i></span>" +
                    " and <span><i>Miscanthus sacchariflorus</i></span>. " +
                    "Giant miscanthus can be pressed into fuel pellets or biomass logs " +
                    "for combustion and can be used for cellulosic biofuel production " +
                    "as a feedstock. Additionally, it has high lignocellulose yields in " +
                    "comparison to other biomass crops, which makes it desirable as a " +
                    "biomass feedstock. <br /><br />" +

                    "The plant is drought-tolerant but fares better under wetter conditions and is actually ideal for soils that " +
                    "are too wet for other crops, such as corn or soybeans. Giant miscanthus’s roots can grow deep below the ground " +
                    "surface, reaching down to 8 feet, and they break up hard soils and improve drainage. The hybridized giant miscanthus " +
                    "is sterile, so there are no concerns about invasiveness from seed. However, some other varieties of miscanthus can be invasive, " +
                    "so it is important to ensure that the giant miscanthus species is selected when looking for planting stock.";

                model.GeographicDistribution = "Giant miscanthus is not widely found across the United States currently. It has been used occasionally " +
                                               "as an ornamental plant since the 1930s in the United States; however, it has been planted more widely in Europe. " +
                                               "Based on how well it does in Europe (from Italy in the south to Denmark in the north) it is likely that giant miscanthus " +
                                               "will be productive over a wide range of temperate regions including the southern US. It does not fare well in arid climates.";
                model.GrowthRate =
                    "A full crop can be expected after two to three growing seasons, and the first harvest can occur as soon as the second year. " +
                    "Conventional hay or silage harvesting equipment can be used. Giant miscanthus can be harvested annually for approximately 15-20 years. " +
                    "The plant reaches heights of up to 12 feet and can regrow from the rhizome each year. Giant miscanthus is typically harvested in the late" +
                    " winter or early spring to allow nutrients to translocate from the above ground portion of the plant, into the crown and rhizomes. " +
                    "These nutrients can then be used by the plant the following year.";

                model.Yield =
                    "Biomass yields for giant miscanthus average as much as 8 to 12 tons per acre per year, making it among the highest-yielding perennial energy crops. " +
                    "However, there is not very much published data on giant miscanthus yields that are specific to the United States. Stalks can be as long as 9 " +
                    "feet at harvest, which makes for abundant biomass. The relationship between harvestable yields and fertilization is not clear. Productivity is " +
                    "typically higher on more fertile soils, but sites with poorer soils can still produce higher yields if other environmental conditions, " +
                    "such as temperature, are favorable. Giant miscanthus will grow on marginal sites, but, as with most crops, yields will be " +
                    "reduced on nutrient-poor lands; however, overall production can still be considered high in comparison to other perennial grasses. " +
                    "Some small research trials in the Midwestern United States suggest that giant miscanthus could yield more than twice the amount of biomass " +
                    "as switchgrass grown in the same area. ";

                model.Conclusion =
                    "Giant miscanthus has great potential in the southeastern United States as a biomass feedstock. " +
                    "Its fast growth rate and abundant biomass growth on all types of land make it a desirable bioenergy crop. " +
                    "There are some constraints for its establishment, such as difficulties in sourcing planting materials and in " +
                    "planting the rhizomes.Despite these challenges, the economic potential returns for giant miscanthus over " +
                    "20 years are favorable compared to other energy crops.";
                model.PreparedBy = "Leslie Boby, Bill Hubbard and Connor McDonald.";
                model.AdaptedFrom = "Jacobson, M. 2013.Renewable and Alternative Energy Fact Sheet: NewBio Energy Crop Profile: Giant Miscanthus." +
                                    "<a href='http://extension.psu.edu/publications/ee0079/view'>http://extension.psu.edu/publications/ee0079/view.</a>";
                model.OtherSources = new List<string>
                {

                    "eXtension Farm Energy COP.'Miscanthus (Miscanthus x gigan- teus) for Biofuel Production. " +
                    "<a href='www.extension.org/pages/26625/miscanthus-miscanthus-x-giganteus-for-biofuel-production'>www.extension.org/pages/26625/miscanthus-miscanthus-x-giganteus-for-biofuel-production.</a>",

                    "Jacobson, M. 2013.Renewable and Alternative Energy Fact Sheet: NewBio Energy Crop Profile: Giant Miscanthus.<a href='http://extension.psu.edu/publications/ee0079/view'>http://extension.psu.edu/publications/ee0079/view.</a>",
                    "Williams, M.J.and Douglas, J.July 2011.Planting and Managing Giant Miscanthus as a Biomass Energy Crop.USDA NRCS Plant Materials Program, Technical Note No. 4."
                };
            
                

            }
            else if (CropType.Willow == crop)
            {
                model.Title = "Willow";
                model.SubTitle = "Willow sp.";
            }
            else if (CropType.Poplar == crop)
            {
                model.Title = "Poplar";
                model.SubTitle = "Poplar sp.";
            }
            else if (CropType.Pine == crop)
            {
                model.Title = "Pine";
                model.SubTitle = "Pinus sp.";
            }

            //check if pdf can be geneated

            var fileName = string.Format("{0}.pdf", crop.ToString()).ToLower();
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Files"), fileName);

            model.CanPDFFactsheet = System.IO.File.Exists(path); 
            
            return model; 

        }

        #endregion
    }
}