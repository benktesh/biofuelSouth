using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BiofuelSouth.Models;


namespace BiofuelSouth.Services
{
    public static class DataService
    {
        public static List<string> GetCounty(String state)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var counties = db.County.Where(p => p.State == state).Select(p => p.Name).ToList();
                return counties;
            }
        }


        public static double GetProductivityPerAcreForCropByGeoId(String category, String geoId)
        {
            int intGeoid = Convert.ToInt32(geoId);
            using (DatabaseContext db = new DatabaseContext())
            {

                var productivity = db.Productivities.Where(p => p.GeoId == intGeoid && p.CropType.Equals(category)).Select(p => p.Yield).FirstOrDefault();
                return productivity;
            }
        }

        public static double GetCostPerAcreForCropByGeoId(String category, String geoId)
        {
            int intGeoid = Convert.ToInt32(geoId);
            using (DatabaseContext db = new DatabaseContext())
            {
                var productivity = db.Productivities.Where(p => p.GeoId == intGeoid && p.CropType.Equals(category)).Select(p => p.Cost).FirstOrDefault();
                return productivity;
            }
        }

        public static List<Glossary> GetGlossary()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var resultList = new List<Glossary>();
                var result = db.Glossaries.ToList();
               return result;
            }
        }
    

        public static List<Glossary> Search(String term)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var resultList = new List<Glossary>();
                var result = db.Glossaries.FirstOrDefault(p => p.term.Equals(term, StringComparison.InvariantCultureIgnoreCase));
                IEnumerable<Glossary> moreResults = new List<Glossary>();
                IEnumerable<Glossary> evenMoreResults = new List<Glossary>();
                if (result == null)
                {
                    //Make a new logic

                    result = new Glossary(term.ToLower(), "", "", "");

                    result.counter = 1;
                    result.description = "TBI";//TO BE IMPLEMENTED
                    result.keywords = "TBI";
                    result.source = "TBI";
                    db.Glossaries.Add(result);
                    db.SaveChanges();
                    resultList.Add(result);
                   
                }
                else
                {
                    if (result.counter == null)
                    result.counter = 0;
                    result.counter = result.counter + 1;
                    db.Entry(result).State = EntityState.Modified;
                    db.SaveChanges();

              
                    resultList.Add(result);
                    //add to result the records for which we have keywords.
                    //1. get keywords for the term -Done
                    //2. parse keywords into list  - Done
                    //3. find all the records where keywords are 'terms'
                    char[] delimiters = new[] { ',', ';' };

                    String kw = result.keywords;
                    if (kw != null)
                    {
                        IList<String> keywords =
                        result.keywords.ToLower().Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
                        moreResults = db.Glossaries.Where(k => keywords.Contains(k.term.ToLower()));
 
                    }
         
                    //add to more results

                    //1. find all the records where term is keyword
                    evenMoreResults = db.Glossaries.Where(k => k.keywords.ToLower().Contains(term.ToLower()));

                    //Merge results
                    moreResults = moreResults.Union(evenMoreResults).OrderBy(x => x.term);

                }


                resultList.AddRange(moreResults);
                
                return resultList;

            }
        }
    }
}