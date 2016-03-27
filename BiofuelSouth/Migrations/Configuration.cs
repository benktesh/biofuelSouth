
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using BiofuelSouth.Models;
using BiofuelSouth.Models.Entity;

namespace BiofuelSouth.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BiofuelSouth.Models.DatabaseContext";
        }

        protected override void Seed(DatabaseContext context)
        {
            //Clean up all empty glossaries
            var allEmpty = context.Glossaries.Where(m => m.Term.Length == 0);
            context.Glossaries.RemoveRange(allEmpty);
            context.SaveChanges();

            

            var glossaries = context.Glossaries.Where(m => m.Id == null);
            foreach (var g in glossaries)
            {
                g.Id = Guid.NewGuid();
                
            }


           
            context.SaveChanges();

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

            var x = new LookUpEntity();
            x.Name = @"Tarp Cost $/sq ft";
            x.Label = @"$ 0.15/sq ft";
            x.Value = Convert.ToString(0.15);
            x.LookUpGroup = LookupGroup.StorageCostParameter;
            x.SortOrder = 1;

            context.LookUps.AddOrUpdate(

                new LookUpEntity
                {
                    Id = Guid.NewGuid(),
                    Name = @"Tarp Cost $/sq ft",
                    Label = @"$ 0.15/sq ft",
                    Value = Convert.ToString(0.15),
                    LookUpGroup = LookupGroup.StorageCostParameter,
                    Description = @"Storing switchgrass",
                    SortOrder = 1
                },
                 new LookUpEntity
                {
                    Id = Guid.NewGuid(),
                    Name = @"Pallet Cost $/sq ft",
                    Label = @"$ 0.25/sq ft",
                    Value = Convert.ToString(0.15),
                    LookUpGroup = LookupGroup.StorageCostParameter,
                    Description = @"Storing switchgrass",
                    SortOrder = 1
                },

                      new LookUpEntity
                {
                    Id = Guid.NewGuid(),
                    Name = @"Gravel Cost $/sq ft",
                    Label = @"$ 0.75/sq ft",
                    Value = Convert.ToString(0.15),
                    LookUpGroup = LookupGroup.StorageCostParameter,
                    Description = @"Storing switchgrass",
                    SortOrder = 1
                },

                      new LookUpEntity
                      {
                          Id = Guid.NewGuid(),
                          Name = @"Labor Cost $/sq ft",
                          Label = @"$ 10/sq ft",
                          Value = Convert.ToString(0.15),
                          LookUpGroup = LookupGroup.StorageCostParameter,
                          Description = @"Storing switchgrass",
                          SortOrder = 1
                      },
                           new LookUpEntity
                           {
                               Id = Guid.NewGuid(),
                               Name = @"Land Cost $/sq ft",
                               Label = @"$ 10/sq ft",
                               Value = Convert.ToString(0.15),
                               LookUpGroup = LookupGroup.StorageCostParameter,
                               Description = @"Storing switchgrass",
                               SortOrder = 1
                           }


                );
            context.LookUps.AddOrUpdate(x);
            context.SaveChanges();

            context.Database.ExecuteSqlCommand(

                @"


DELETE FROM GLOSSARIES;

SET IDENTITY_INSERT  [Glossaries] ON
INSERT [dbo].[GlossaryEntities] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Test Alternative Fuels', N'Biofuel; Fuel; Fossil Fuel', N'Fuels that do not involve fossil fuels. Examples: solar power, wind power, and biomass', NULL, N'http://bioenergy-midlands.org/why-bioenergy/glossary-of-bioenergy-terms/', 1)
INSERT [dbo].[GlossaryEntities] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Test Biochemical Conversion', N'Conversion; biofuel', N'The use of biological processes, usually microorganisms or enzymes, to convert organic materials into usable products such as biofuels', NULL, N'http://www.aig.com/ncglobalweb/internet/US/en/files/Chartis_BiofuelsGlossary_tcm295-179858.pdf', 2)
"

                );


        }

        

        /*
        public void SeedGlossary()
        {

            Sql(@"


DELETE FROM GLOSSARIES;

SET IDENTITY_INSERT  [Glossaries] ON
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Alternative Fuels', N'Biofuel; Fuel; Fossil Fuel', N'Fuels that do not involve fossil fuels. Examples: solar power, wind power, and biomass', NULL, N'http://bioenergy-midlands.org/why-bioenergy/glossary-of-bioenergy-terms/', 1)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Biochemical Conversion', N'Conversion; biofuel', N'The use of biological processes, usually microorganisms or enzymes, to convert organic materials into usable products such as biofuels', NULL, N'http://www.aig.com/ncglobalweb/internet/US/en/files/Chartis_BiofuelsGlossary_tcm295-179858.pdf', 2)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Biodiesel', N'Bioenergy; biofuel; biomass', N'A biodegradable fuel made from organic matter (usually oils or fats) that can be used in a diesel engine, such as a vehicle', NULL, N'http://bioenergy-midlands.org/why-bioenergy/glossary-of-bioenergy-terms/', 3)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Bioenergy', N'Biofuel; energy; ', N'Energy made from renewable biomass sources, such as agricultural crops, plant and animal wastes, and other organic materials. May be in the form of liquid fuels, electricity, heat, and gas', NULL, N'http://www.aig.com/ncglobalweb/internet/US/en/files/Chartis_BiofuelsGlossary_tcm295-179858.pdf', 4)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Biofuel', N'fuel; fossil fuel; energy; ethanol;', N'Biomass converted to liquid or gaseous fuels such as ethanol, methanol, methane, and hydrogen', NULL, N'http://www.nrel.gov/biomass/glossary.html', 5)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Biomass', N'forest; residues; ', N'The total amount of organic matter present in an organism, population, ecosystem or given area', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=1096&n=1&s=5&t=2', 6)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Bioproducts', N'energy; biomass', N'Bioproducts are fuels, chemicals, materials, or electric power or heat produced from biomass. Including any energy, commercial or industrial product (other than food or feed) that utilizes biological products or renewable domestic agricultural (plant, animal, and marine) or forestry materials.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=994&n=1&s=5&t=2', 7)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Biorefinery ', N'Biochemical conversion;', N'A facility that integrates biomass conversion processes and equipment to produce fuels, power, and chemicals from biomass', NULL, N'http://www.nrel.gov/biomass/biorefinery.html', 8)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Biosystems Engineering', N'Biochemical conversion; engineering; biofuel;', N'The analysis, design, and control of biologically-based systems for the sustainable production and processing of food and biological materials and the efficient utilization of natural and renewable resources in order to enhance human health in harmony with the environment', NULL, N'http://www.egr.msu.edu/~alocilja/Teaching/Principles%20of%20BE%20Book%208-12-2013.pdf', 9)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Byproducts', N'Bioenergy;', N'Material, other than the principal product, generated as a consequence of an industrial process or as a breakdown product in a living system', NULL, N'http://macd.org/assets/downloads/envirothon/Biofuel_Student_Glossary.pdf', 10)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Cellulose', N'Bioenergy', N'A principle component of wood; it is made of linked glucose (sugar) molecules that strengthens the cell walls of most plants', NULL, N'http://macd.org/assets/downloads/envirothon/Biofuel_Student_Glossary.pdf', 11)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Conservation', N'natural resources;', N'The protection, preservation, or restoration of natural resources such as forests, soil, water and wildlife', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=6179&n=1&s=5&t=2', 12)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Consumer', N'enterpreneurship', N'(Economics) A person or organization that uses a commodity or service', NULL, N'http://dictionary.reference.com/browse/consumer', 13)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Conversion', N'Biochemical Conversion; ethanol; refinary;', N'The change of energy from one form to another', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=3020&n=1&s=5&t=2', 14)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Costs of Production', N'Cost; Bioenergy; Biomass', N'A cost incurred by a business when manufacturing a good or producing a service. Production costs combine raw material and labor. To figure out the cost of production per unit, the cost of production is divided by the number of units produce. ', NULL, N'http://www.investopedia.com/terms/p/production-cost.asp', 15)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Cottonwood', N'Biomass; biofuel; crop', N'A poplar hardwood tree species that is used for the research and production of biofuels. It is prized for its fast-growing and high production of biomass. Common cottonwoods used for biofuel include the black cottonwood and the Eastern cottonwood. ', NULL, N'http://hardwoodbiofuels.org/wp-content/uploads/2014/02/PoplarforBiofuelsInfosheet.pdf', 16)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'E20', N'fuel; fossil fuel; energy; ethanol;', N'Fuel made of 20% ethanol and 80% gasoline', NULL, NULL, 17)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Energy Crop', N'biofuel; biomass; cellulose', N'Crops grown specifically to provide the raw materials for energy production, including food crops such as corn and sugarcane and nonfood crops such as poplar trees and switchgrass.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=3022&n=1&s=5&t=2', 18)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Entrepreneurship', N'consumer', N'The organization, management, and assumption of risks of a business or enterprise, usually implying an element of change or challenge and a new opportunity. ', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=3058&n=1&s=5&t=2', 19)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Ethanol', N'fuel; fossil fuel; energy;', N'Clean renewable fuel made from grain or other biomass sources and used to describe biologically processed ethyl alcohol that has been combined with a type of denaturant (to render undrinkable). Primarily used in vehicles as fuel or fuel additive', NULL, N'http://www.aig.com/ncglobalweb/internet/US/en/files/Chartis_BiofuelsGlossary_tcm295-179858.pdf', 20)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Eucalyptus', N'Crop; biomass', N'Eucalypts are the world’s most widely planted hardwood trees. Their outstanding diversity, adaptability and growth have made them a global renewable resource of fibre and energy.', NULL, N'http://www.nature.com/nature/journal/v510/n7505/full/nature13308.html', 21)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Extension', N'Consumer', N'A common feature of the administrative structure of rural areas and these extension services have the responsibility, in partnership with the farmers, of directing programmes and projects for change. ', NULL, N'http://www.fao.org/docrep/t0060e/T0060E03.htm', 22)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Feedstock', N'Crop; biomass; miscanthus; cottonwood; poplar; switchgrass; residue; eucalyptus;', N'Any raw materials (biomass or plant material) used to produce fuel. ', NULL, N'http://www.altranex.com/glossary/', 23)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Fiber', N'crop; biomass; cellulose', N'Fibers are long cells with thick walls and tapering ends. The cell wall often contain lignin and cellulose. They are dead at maturity and function as support tissue in plant stems and roots. They come from the outer portion of the stem of fibrous plants such as flax, hemp, and jute, or from the leaves of plants such as cattail, agave, and yucca. Fibers can be spun into filaments, thread, or rope; be chemically modified to create a composite material (e.g., rayon or cellophane); or matted into sheets as with paper. ', NULL, N'http://www.fs.fed.us/wildflowers/ethnobotany/fibers.shtml', 24)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Forestland (nonindustrial private)', N'consumer', N'Forest land owned by a private individual, group or corporation not part of the timber industry', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=6326&n=1&s=5&t=2', 25)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Forestry', N'crop; biofuel;', N'Generally, a profession embracing the science, business, and art of creating, conserving, and managing forest, and forest lands for the continuing use of their resources, materials, and other forest products.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=ForestEng&l=60&w=336&n=1&s=5&t=2', 26)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Fossil Fuels', N'fuel; energy;', N'A carbon or hydrocarbon fuel formed in the ground from the remains of dead plants and animals. It takes milliions of years to form fossil fuels. Examples are: oil, natural gas, and coal.', NULL, N'http://macd.org/assets/downloads/envirothon/Biofuel_Student_Glossary.pdf', 27)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Hardwood', N'cellulose; crop', N'Generally one of the botanical groups of trees that have vessels or pores and broad leaves, in contrast to the conifers or softwoods. The term has no reference to the actual hardness of the wood. ', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=4375&n=1&s=5&t=2', 28)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Harvesting', N'crop', N'Removing merchantable trees (contrasts with cuttings, which remove immature trees.)', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=ForestEng&l=60&w=377&n=1&s=5&t=2', 29)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Herbaceous', N'cellulose; crop', N'Refers to a plant that has a non-woody stem and which dies back at the end of the growing season', NULL, N'http://www.biology-online.org/dictionary/Herbaceous', 30)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Hybrid', N'crop', N'Heterozygous offspring of two genetically different parents', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=4641&n=1&s=5&t=2', 31)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Logistics', N'consumer', N'The flow of funds, goods, and information between source and consumer', NULL, N'http://www.ups.com/content/us/en/bussol/browse/article/what-is-logistics.html', 32)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Metrics', N'productivity; cost', N'Quantifiable measure that is used to track and assess the status of a specific business process', NULL, N'http://www.klipfolio.com/resources/articles/what-are-business-metrics', 33)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Miscanthus', N'crop; cellulose; biomass', N'Miscanthus (commonly known as Elephant Grass) is a high yielding energy crop that grows over 3 metres tall, resembles bamboo and produces a crop every year without the need for replanting. The rapid growth, low mineral content, and high biomass yield of Miscanthus increasingly make it a favourite choice as a biofuel, outperforming maize (corn) and other alternatives', NULL, N'http://www.recrops.com/miscanthus', 34)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Natural Resource', N'forestry;', N'Useful raw materials that we get from the Earth. They occur naturally, which means that humans cannot make natural resources. Instead, we use and modify natural resources in ways that are beneficial to us. Examples: air, water, sunlight, plants, and animals. ', NULL, N'http://education-portal.com/academy/lesson/what-are-natural-resources-definition-lesson-quiz.html', 35)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Non-renewable Resource', N'fossil fuel; energy;', N'A non-renewable energy source is one that cannot be replaced as it is used. Fossil fuels (oil, coal, and natural gas) are not renewable because they form at such a slow rate. ', NULL, N'http://macd.org/assets/downloads/envirothon/Biofuel_Student_Glossary.pdf', 36)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Poplar', N'crop; cellulose; biomass', N'Deciduous hardwood species of trees that is used in biofuel production as well as for wood, veneer, and environmental benefits. Poplars are harvested for bioenergy because the of the ability of the species to grow quickly, to produce a high amount of biomass in a short period of time, and to easily grow in various regions. Popular poplar trees used in biofuel production include: cottonwoods and hybrid poplars', NULL, N'http://www.extension.org/pages/70456/poplar-populus-spp-trees-for-biofuel-production#.VIpIwIe3I40', 37)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Productivity ', N'yield; biomass', N'Output per unit of labor input (Economic term)', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=5111&n=1&s=5&t=2', 38)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Raw Materials', N'biomass; producticity; yield;', N'A good sold for production or consumption just as it is found in nature, examples include: crude oil, coal, copper,  rough diamonds, wheat, coffee beans, or cotton', NULL, N'http://epp.eurostat.ec.europa.eu/statistics_explained/index.php/Glossary:Raw_material', 39)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Renewable Energy ', N'energy; biomass', N'An energy resource that can be replaced as it is used. Examples include: solar, wind, geothermal, hydro, and biomass', NULL, N'http://macd.org/assets/downloads/envirothon/Biofuel_Student_Glossary.pdf', 40)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Renewable Resource', N'crop', N'Naturally occurring raw material that comes from a limitless or cyclical source such as the sun, wind, water (hydroelectricity), or trees', NULL, N'http://www.epa.gov/osw/education/quest/gloss1a.htm', 41)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Residue', N'crop; cellulose; biomass', N'Any organic matter left in the field after the harvest of a crop, e.g. leaves, stalks, stubble, roots, hulls', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=2226&n=1&s=5&t=2', 42)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Return On Investment', N'productivity; cost', N'ROI is a performance measure used to evaluate the efficiency of an investment. To calculate the
benefit (return) of an investment is divided by the cost of the investment; the result is expressed as a
percentage or a ratio. ROI = (gain from investment – cost of investment)/cost of investment', NULL, N'http://blogs.extension.org/bioen1/files/2009/11/BIOENglossaryFINALv1.pdf', 43)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Short Rotation Woody Crops', N'crops; biomass', N'Woody crops such as willows, poplars, Robinia and Eucalyptus with coppicing abilities as well as lignocellulosic (biomass composed primarily of cellulose, hemicelluloses and lignin) crops such as reed canary grass, Miscanthus and switch grass. Coppicing means that the crops are fast growing and can be cut down to a low stump (or stool) when they are dormant in winter and go on to produce many new stems in the following growing season. ', NULL, N' http://www.okepscor.org/glossary-bioenergy-terms/glossary-bioenergy-terms & http://www.biomassenergycentre.org.uk/portal/page?_pageid=75,18092&_dad=portal&_schema=PORTAL & http://www.ieabioenergytask43.org/Task_30_Web_Site/index.htm', 44)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Silviculture', N'forestry;', N'The science, art, and practice of establishing and tending forest stands to produce forest stands with the desired composition, constitution, and growth rate.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=8114&n=1&s=5&t=2', 45)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Softwood', N'crop; cellulose; biomass', N'Botanical grouping of trees that are usually evergreen and have needlelike or scalelike leaves. Also known as conifers and coniferous trees. Also the wood produced from such trees. The term softwood does not refer to the hardness of the wood.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=ForestEng&l=60&w=814&n=1&s=5&t=2', 46)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Sorghum ', N'crop; cellulose; biomhighs', N'Sorghum is a grass that functions as a grain, forage, or sugar crop. It is a high-energy plant that is valued for biofuel production because of its adaptability and high yields. ', NULL, N'http://sorghumgrowers.com/sorghum-101/', 47)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Stakeholder', N'consumer', N'Individuals, groups or organizations that have an interest in or are affected by the activities of government, business, or other organization.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=8333&n=1&s=5&t=2', 48)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Sustainability', N'crop; cellulose; biomass', N'Sustainability is the overarching concept of meeting the needs of the present without compromising the ability of future generations to meet their needs ', NULL, N'http://www.sustainabilityconsortium.org/glossary/#s', 49)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Supply Chain', N'productivity; cost;', N'The network of firms that bring products to market, from companies that produce raw materials to retailers and others that deliver finished products to consumers. Economic value is added through the coordinated management of the flow of physical goods and associated information at each stage of the chain.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?s=1&n=1&y=0&l=60&k=glossary&t=2&w=supply+chain', 50)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Switchgrass', N'crop; cellulose; biomass', N'A North American native perennial warm-season grass which is used to develop biofuels because of its high yield and ability to easily grow from the seed with little agricultural help. ', NULL, N'http://www.extension.org/pages/26635/switchgrass-panicum-virgatum-for-biofuel-production#.VIpWlYe3I40', 51)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Synthetic Fuels', N'fuel; fossil fuel; energy;', N'Synthetic gas or synthetic oil. Fuel that is artificially made as contrasted to that which is found in nature', NULL, N'http://www.energy.ca.gov/glossary/glossary-s.html', 52)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Thermochemical Conversion', N'biorefinary; conversion', N'The transformation of complex organic material into light crude oil using pressure and heat', NULL, N'http://biofuel.org.uk/glossary.html', 53)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Watershed', N'crop; cellulose; biomlands', N'The land area that drains water to a particular stream, river, or lake. It is a land feature that can be identified by tracing a line along the highest elevations between two areas on a map, often a ridge.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=9287&n=1&s=5&t=2', 54)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source], [ID]) VALUES (N'Yield', N'crop; productivity;', N'The amount of a crop produced in a given time or from a given place. The amount of product output recovered from a quantity of raw material input in forest product industries. Also, the estimate in forest mensuration of the amount of wood that may be harvested from a particular type of forest stand by species, site, stocking, and management regime at various ages.', NULL, N'http://www2.kenyon.edu/projects/farmschool/addins/glossary.htm & http://agclass.nal.usda.gov/mtwdk.exe?k=ForestEng&l=60&w=958&n=1&s=5&t=2', 55)

SET IDENTITY_INSERT  [Glossaries] OFF



");
        }

        */
    }


}
