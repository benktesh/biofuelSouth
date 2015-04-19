using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web.Helpers;
using System.Web.Mvc;
using BiofuelSouth.Models;
using BiofuelSouth.ViewModels;


namespace BiofuelSouth.Services
{
    public static class DataService
    {
        public static List<string> GetCounty(String state)
        {
            using (var db = new DatabaseContext())
            {
                
                return  db.County.Where(p => p.State == state).Select(p => p.Name).ToList();
                
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

        public static List<County> GetCountyData(string selectedCategory = "ALL")
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



        public static double GetProductivityPerAcreForCropByGeoId(String category, String geoId)
        {
            var intGeoid = Convert.ToInt32(geoId);
            using (var db = new DatabaseContext())
            {

                var productivity = db.Productivities.Where(p => p.GeoId == intGeoid && p.CropType.Equals(category)).Select(p => p.Yield).FirstOrDefault();
                return productivity;
            }
        }

        public static double GetCostPerAcreForCropByGeoId(String category, String geoId)
        {
            var intGeoid = Convert.ToInt32(geoId);
            using (var db = new DatabaseContext())
            {
                var productivity = db.Productivities.Where(p => p.GeoId == intGeoid && p.CropType.Equals(category)).Select(p => p.Cost).FirstOrDefault();
                return productivity;
            }
        }


        public static Glossary GetGlossary(String term)
        {
            using (var db = new DatabaseContext())
            {

                var result = db.Glossaries.FirstOrDefault(m => m.Term.Equals(
                    term, StringComparison.InvariantCultureIgnoreCase)); 
                return result;
            }
        }


        public static List<Glossary> GetGlossary()
        {
            using (var db = new DatabaseContext())
            {
               
                var result = db.Glossaries.ToList();
               return result;
            }
        }

        public static void SaveFeedback(FeedBack fb)
        {
            
            using (var db = new DatabaseContext())
            {
                db.FeedBacks.Add(fb);
                db.SaveChanges();
            }
        }

        public static IList<String> GetAllTerms(string key)
        {
            using (var db = new DatabaseContext())
            {
               
                if (key == null || key.Equals("") || key.Equals("All"))
                {
                    return db.Glossaries.Select(p => p.Term).ToList();
                }

                
                key = key.Replace("-", null);

                var result = new List<String>();
                foreach (var element in key.ToCharArray())
                {
                    var startWith = element.ToString(CultureInfo.InvariantCulture).ToLower();
                    result.AddRange(db.Glossaries.Select(p => p.Term).Where(d => d.ToLower().StartsWith(startWith)));
                }

                return result;
            }

        }

        public static void SaveGlossary(Glossary gm)
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
                var glossary = db.Glossaries.FirstOrDefault(b => b.Term.ToLower() == gvm.Term.ToLower());

                if (glossary != null)
                {
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

                
            }
        }

        public static List<Glossary> Search(String term)
        {
            using (var db = new DatabaseContext())
            {
                var resultList = new List<Glossary>();
                var result = db.Glossaries.FirstOrDefault(p => p.Term.Equals(term, StringComparison.InvariantCultureIgnoreCase));
                IEnumerable<Glossary> moreResults = new List<Glossary>();
                if (result == null)
                {
                    //Make a new logic

                    result = new Glossary(term.ToLower(), "", "", "");

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
                    IEnumerable<Glossary> evenMoreResults = db.Glossaries.Where(k => k.Keywords.ToLower().Contains(term.ToLower()));

                    //Merge results
                    moreResults = moreResults.Union(evenMoreResults).OrderBy(x => x.Term);

                }


                resultList.AddRange(moreResults);
                
                return resultList;

            }
        }
    }
}