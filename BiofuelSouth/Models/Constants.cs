using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BiofuelSouth.Services;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BiofuelSouth.Models
{
    public class Constants
    {
        

        

        public List<string> GetCategory()
        {
            //TODO could change this to make database driven call.
            String[] Category = new String[] { "Switchgrass", "Miscanthus", "Poplar", "Willow" };
            return Category.ToList();
        }

        public List<string> GetState()
        {
            String[] State = new String[] { "TN", "AL", "LA", "AR", "FL", "GA", "KY", "MS", "NC", "SC", "VA" };  
            //TODO could change this to make database driven call.

            return State.ToList();
        }

        public IEnumerable<string> GetCounty(String state)
        {
            //TODO could change this to make database driven call.
            
            return DataService.GetCounty(state);
            
        }
    }
}