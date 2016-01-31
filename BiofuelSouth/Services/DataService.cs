﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using BiofuelSouth.Enum;
using BiofuelSouth.Models;
using BiofuelSouth.Models.Entity;
using BiofuelSouth.ViewModels;

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
                return productivity*0.446; //MG/ha -> t/acre
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

                var productivity = db.Productivities.Where(p => p.GeoId == intGeoid && p.CropType == cropType).Select(p => p.Cost).FirstOrDefault();
                return productivity/2.471 ; //conver to $/ha -> $/acre
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

        public static Glossary GetGlossaryById(Guid id)
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


        public static List<Glossary> GetGlossary(int? count = null)
        {
            using (var db = new DatabaseContext())
            {
               
	            if (count != null)
	            {
		            return new List<Glossary>(db.Glossaries.OrderByDescending(p => p.Counter).Take(20));
	            }
				return db.Glossaries.ToList();
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
                    
                    var data = db.Glossaries.Select(p => new   { Key = p.Term, Value = p.Counter ?? 0})
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
                                    Value =Math.Min(300, (int)(1 + 2* (pair.Value - mn) / (mx - mn)) * 100),  
                                }).ToList();
            }

        }

        public class GlossaryTransferMmodel
        {
            public int Value { get; set; }
            public string Key { get; set; }
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