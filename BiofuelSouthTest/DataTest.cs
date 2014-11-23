using System.Linq;
using BiofuelSouth.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BiofuelSouth.Models;


namespace BiofuelSouthTest
{
    [TestClass]
    public class DataTest
    {
        [TestMethod]
        public void TestGetCategories()
        {
            //String[] Category = new String[] { "Switchgrass", "Miscangthus", "Poplar", "Willow" };
            var testCategory = "Switchgrass".ToUpper();
            Constants constants = new Constants();
            var category = constants.GetCategory();
            category = category.ConvertAll(c => c.ToUpper());
            Assert.IsTrue(category.Contains(testCategory));
        }

        [TestMethod]
        public void TestCorrectNumberOfCategories()
        {

            //String[] Category = new String[] { "Swtichgrass", "Miscangthus", "Poplar", "Willow" };
            int numberOfCategories = 4;
            Constants constants = new Constants();
            var countCategories = constants.GetCategory();
            Assert.AreEqual(4, countCategories.Count());

        }

        [TestMethod]
        public void TestGetStates()
        {
            Constants constants = new Constants();
            var states = constants.GetState();
            Assert.IsTrue(states.Contains<string>("AL"));
            Assert.IsTrue(states.Contains<string>("AR"));
            Assert.IsTrue(states.Contains<string>("FL"));
            Assert.IsTrue(states.Contains<string>("GA"));
            Assert.IsTrue(states.Contains<string>("LA"));
            Assert.IsTrue(states.Contains<string>("SC"));
            Assert.IsTrue(states.Contains<string>("NC"));
            Assert.IsTrue(states.Contains<string>("KY"));
            Assert.IsTrue(states.Contains<string>("MS"));
            Assert.IsTrue(states.Contains<string>("TN"));
            Assert.IsTrue(states.Contains<string>("VA"));

        }

        [TestMethod]
        public void TestGetStatesMustFail()
        {

            Constants constants = new Constants();
            var states = constants.GetState();
            var containsAl = states.Contains<string>("CA");
            Assert.IsFalse(containsAl);
        }

        [TestMethod]
        public void TestCheckCountyByName()
        {
            
            DataController dc = new DataController();

            Constants constants = new Constants();
            var states = constants.GetState();

            var counties = dc.CountiesForState("AL");

            
            //Check with json
           // Assert.IsTrue(counties.Contains("Autauga"));

        }

        [TestMethod]
        public void TestGetCounties()
        {
            /*
             * State	(No column name)
                AL	67
                FL	67
                GA	159
                KY	120
                MS	82
                NC	100
                SC	46
                TN	95
                VA	134
                WV	55
             * */
            Constants constants = new Constants();
            var counties = constants.GetCounty("AL");
            Assert.IsNotNull(counties);
            Assert.AreEqual(67, counties.Count());

            counties = constants.GetCounty("TN");
            Assert.IsNotNull(counties);
            Assert.AreEqual(95, counties.Count());

            //Assert.IsTrue(states.Contains<string>("AL"));

            //Assert.IsTrue(states.Contains<string>("AR"));
            //Assert.IsTrue(states.Contains<string>("FL"));
            //Assert.IsTrue(states.Contains<string>("GA"));
            //Assert.IsTrue(states.Contains<string>("LA"));
            //Assert.IsTrue(states.Contains<string>("SC"));
            //Assert.IsTrue(states.Contains<string>("NC"));
            //Assert.IsTrue(states.Contains<string>("KY"));
            //Assert.IsTrue(states.Contains<string>("MS"));
            //Assert.IsTrue(states.Contains<string>("TN"));
            //Assert.IsTrue(states.Contains<string>("VA"));

        }
    }
}
