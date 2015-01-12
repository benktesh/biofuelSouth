namespace BiofuelSouth.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class _glossaryUpdate : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Alternative Fuels', NULL, N'Fuels that do not involve fossil fuels. Examples: solar power, wind power, and biomass', NULL, N'http://bioenergy-midlands.org/why-bioenergy/glossary-of-bioenergy-terms/')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Biochemical Conversion', NULL, N'The use of biological processes, usually microorganisms or enzymes, to convert organic materials into usable products such as biofuels', NULL, N'http://www.aig.com/ncglobalweb/internet/US/en/files/Chartis_BiofuelsGlossary_tcm295-179858.pdf')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Biodiesel', NULL, N'A biodegradable fuel made from organic matter (usually oils or fats) that can be used in a diesel engine, such as a vehicle', NULL, N'http://bioenergy-midlands.org/why-bioenergy/glossary-of-bioenergy-terms/')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Bioenergy', NULL, N'Energy made from renewable biomass sources, such as agricultural crops, plant and animal wastes, and other organic materials. May be in the form of liquid fuels, electricity, heat, and gas', NULL, N'http://www.aig.com/ncglobalweb/internet/US/en/files/Chartis_BiofuelsGlossary_tcm295-179858.pdf')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Biofuel', NULL, N'Biomass converted to liquid or gaseous fuels such as ethanol, methanol, methane, and hydrogen', NULL, N'http://www.nrel.gov/biomass/glossary.html')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Biomass', NULL, N'The total amount of organic matter present in an organism, population, ecosystem or given area', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=1096&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Bioproducts', NULL, N'Bioproducts are fuels, chemicals, materials, or electric power or heat produced from biomass. Including any energy, commercial or industrial product (other than food or feed) that utilizes biological products or renewable domestic agricultural (plant, animal, and marine) or forestry materials.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=994&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Biorefinery ', NULL, N'A facility that integrates biomass conversion processes and equipment to produce fuels, power, and chemicals from biomass', NULL, N'http://www.nrel.gov/biomass/biorefinery.html')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Biosystems Engineering', NULL, N'The analysis, design, and control of biologically-based systems for the sustainable production and processing of food and biological materials and the efficient utilization of natural and renewable resources in order to enhance human health in harmony with the environment', NULL, N'http://www.egr.msu.edu/~alocilja/Teaching/Principles%20of%20BE%20Book%208-12-2013.pdf')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Byproducts', NULL, N'Material, other than the principal product, generated as a consequence of an industrial process or as a breakdown product in a living system', NULL, N'http://macd.org/assets/downloads/envirothon/Biofuel_Student_Glossary.pdf')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Cellulose', NULL, N'A principle component of wood; it is made of linked glucose (sugar) molecules that strengthens the cell walls of most plants', NULL, N'http://macd.org/assets/downloads/envirothon/Biofuel_Student_Glossary.pdf')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Conservation', NULL, N'The protection, preservation, or restoration of natural resources such as forests, soil, water and wildlife', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=6179&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Consumer', NULL, N'(Economics) A person or organization that uses a commodity or service', NULL, N'http://dictionary.reference.com/browse/consumer')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Conversion', NULL, N'The change of energy from one form to another', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=3020&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Costs of Production', NULL, N'A cost incurred by a business when manufacturing a good or producing a service. Production costs combine raw material and labor. To figure out the cost of production per unit, the cost of production is divided by the number of units produce. ', NULL, N'http://www.investopedia.com/terms/p/production-cost.asp')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Cottonwood', NULL, N'A poplar hardwood tree species that is used for the research and production of biofuels. It is prized for its fast-growing and high production of biomass. Common cottonwoods used for biofuel include the black cottonwood and the Eastern cottonwood. ', NULL, N'http://hardwoodbiofuels.org/wp-content/uploads/2014/02/PoplarforBiofuelsInfosheet.pdf')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'E20', NULL, N'Fuel made of 20% ethanol and 80% gasoline', NULL, NULL)
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Energy Crop', NULL, N'Crops grown specifically to provide the raw materials for energy production, including food crops such as corn and sugarcane and nonfood crops such as poplar trees and switchgrass.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=3022&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Entrepreneurship', NULL, N'The organization, management, and assumption of risks of a business or enterprise, usually implying an element of change or challenge and a new opportunity. ', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=3058&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Ethanol', NULL, N'Clean renewable fuel made from grain or other biomass sources and used to describe biologically processed ethyl alcohol that has been combined with a type of denaturant (to render undrinkable). Primarily used in vehicles as fuel or fuel additive', NULL, N'http://www.aig.com/ncglobalweb/internet/US/en/files/Chartis_BiofuelsGlossary_tcm295-179858.pdf')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Eucalyptus', NULL, N'Eucalypts are the world�s most widely planted hardwood trees. Their outstanding diversity, adaptability and growth have made them a global renewable resource of fibre and energy.', NULL, N'http://www.nature.com/nature/journal/v510/n7505/full/nature13308.html')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Extension', NULL, N'A common feature of the administrative structure of rural areas and these extension services have the responsibility, in partnership with the farmers, of directing programmes and projects for change. ', NULL, N'http://www.fao.org/docrep/t0060e/T0060E03.htm')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Feedstock', NULL, N'Any raw materials (biomass or plant material) used to produce fuel. ', NULL, N'http://www.altranex.com/glossary/')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Fiber', NULL, N'Fibers are long cells with thick walls and tapering ends. The cell wall often contain lignin and cellulose. They are dead at maturity and function as support tissue in plant stems and roots. They come from the outer portion of the stem of fibrous plants such as flax, hemp, and jute, or from the leaves of plants such as cattail, agave, and yucca. Fibers can be spun into filaments, thread, or rope; be chemically modified to create a composite material (e.g., rayon or cellophane); or matted into sheets as with paper. ', NULL, N'http://www.fs.fed.us/wildflowers/ethnobotany/fibers.shtml')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Forestland (nonindustrial private)', NULL, N'Forest land owned by a private individual, group or corporation not part of the timber industry', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=6326&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Forestry', NULL, N'Generally, a profession embracing the science, business, and art of creating, conserving, and managing forest, and forest lands for the continuing use of their resources, materials, and other forest products.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=ForestEng&l=60&w=336&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Fossil Fuels', NULL, N'A carbon or hydrocarbon fuel formed in the ground from the remains of dead plants and animals. It takes milliions of years to form fossil fuels. Examples are: oil, natural gas, and coal.', NULL, N'http://macd.org/assets/downloads/envirothon/Biofuel_Student_Glossary.pdf')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Hardwood', NULL, N'Generally one of the botanical groups of trees that have vessels or pores and broad leaves, in contrast to the conifers or softwoods. The term has no reference to the actual hardness of the wood. ', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=4375&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Harvesting', NULL, N'Removing merchantable trees (contrasts with cuttings, which remove immature trees.)', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=ForestEng&l=60&w=377&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Herbaceous', NULL, N'Refers to a plant that has a non-woody stem and which dies back at the end of the growing season', NULL, N'http://www.biology-online.org/dictionary/Herbaceous')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Hybrid', NULL, N'Heterozygous offspring of two genetically different parents', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=4641&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Logistics', NULL, N'The flow of funds, goods, and information between source and consumer', NULL, N'http://www.ups.com/content/us/en/bussol/browse/article/what-is-logistics.html')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Metrics', NULL, N'Quantifiable measure that is used to track and assess the status of a specific business process', NULL, N'http://www.klipfolio.com/resources/articles/what-are-business-metrics')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Miscanthus', NULL, N'Miscanthus (commonly known as Elephant Grass) is a high yielding energy crop that grows over 3 metres tall, resembles bamboo and produces a crop every year without the need for replanting. The rapid growth, low mineral content, and high biomass yield of Miscanthus increasingly make it a favourite choice as a biofuel,�outperforming maize (corn) and other alternatives', NULL, N'http://www.recrops.com/miscanthus')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Natural Resource', NULL, N'Useful raw materials that we get from the Earth. They occur naturally, which means that humans cannot make natural resources. Instead, we use and modify natural resources in ways that are beneficial to us. Examples: air, water, sunlight, plants, and animals. ', NULL, N'http://education-portal.com/academy/lesson/what-are-natural-resources-definition-lesson-quiz.html')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Non-renewable Resource', NULL, N'A non-renewable energy source is one that cannot be replaced as it is used. Fossil fuels (oil, coal, and natural gas) are not renewable because they form at such a slow rate. ', NULL, N'http://macd.org/assets/downloads/envirothon/Biofuel_Student_Glossary.pdf')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Poplar', NULL, N'Deciduous hardwood species of trees that is used in biofuel production as well as for wood, veneer, and environmental benefits. Poplars are harvested for bioenergy because the of the ability of the species to grow quickly, to produce a high amount of biomass in a short period of time, and to easily grow in various regions. Popular poplar trees used in biofuel production include: cottonwoods and hybrid poplars', NULL, N'http://www.extension.org/pages/70456/poplar-populus-spp-trees-for-biofuel-production#.VIpIwIe3I40')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Productivity ', NULL, N'Output per unit of labor input (Economic term)', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=5111&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Raw Materials', NULL, N'A good sold for production or consumption just as it is found in nature, examples include: crude oil, coal, copper,  rough diamonds, wheat, coffee beans, or cotton', NULL, N'http://epp.eurostat.ec.europa.eu/statistics_explained/index.php/Glossary:Raw_material')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Renewable Energy ', NULL, N'An energy resource that can be replaced as it is used. Examples include: solar, wind, geothermal, hydro, and biomass', NULL, N'http://macd.org/assets/downloads/envirothon/Biofuel_Student_Glossary.pdf')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Renewable Resource', NULL, N'Naturally occurring raw material that comes from a limitless or cyclical source such as the sun, wind, water (hydroelectricity), or trees', NULL, N'http://www.epa.gov/osw/education/quest/gloss1a.htm')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Residue', NULL, N'Any organic matter left in the field after the harvest of a crop, e.g. leaves, stalks, stubble, roots, hulls', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=2226&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Return On Investment', NULL, N'ROI is a performance measure used to evaluate the efficiency of an investment. To calculate the
benefit (return) of an investment is divided by the cost of the investment; the result is expressed as a
percentage or a ratio. ROI = (gain from investment � cost of investment)/cost of investment', NULL, N'http://blogs.extension.org/bioen1/files/2009/11/BIOENglossaryFINALv1.pdf')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Short Rotation Woody Crops', NULL, N'Woody crops such as willows, poplars, Robinia and Eucalyptus with coppicing abilities as well as lignocellulosic (biomass composed primarily of cellulose, hemicelluloses and lignin) crops such as reed canary grass, Miscanthus and switch grass. Coppicing means that the crops are fast growing and can be cut down to a low stump (or stool) when they are dormant�in winter and�go on to produce many new stems in the following growing season. ', NULL, N' http://www.okepscor.org/glossary-bioenergy-terms/glossary-bioenergy-terms & http://www.biomassenergycentre.org.uk/portal/page?_pageid=75,18092&_dad=portal&_schema=PORTAL & http://www.ieabioenergytask43.org/Task_30_Web_Site/index.htm')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Silviculture', NULL, N'The science, art, and practice of establishing and tending forest stands to produce forest stands with the desired composition, constitution, and growth rate.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=8114&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Softwood', NULL, N'Botanical grouping of trees that are usually evergreen and have needlelike or scalelike leaves. Also known as conifers and coniferous trees. Also the wood produced from such trees. The term softwood does not refer to the hardness of the wood.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=ForestEng&l=60&w=814&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Sorghum ', NULL, N'Sorghum is a grass that functions as a grain, forage, or sugar crop. It is a high-energy plant that is valued for biofuel production because of its adaptability and high yields. ', NULL, N'http://sorghumgrowers.com/sorghum-101/')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Stakeholder', NULL, N'Individuals, groups or organizations that have an interest in or are affected by the activities of government, business, or other organization.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=8333&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Sustainability', NULL, N'Sustainability is the overarching concept of meeting the needs of the present without compromising the ability of future generations to meet their needs ', NULL, N'http://www.sustainabilityconsortium.org/glossary/#s')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Supply Chain', NULL, N'The network of firms that bring products to market, from companies that produce raw materials to retailers and others that deliver finished products to consumers. Economic value is added through the coordinated management of the flow of physical goods and associated information at each stage of the chain.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?s=1&n=1&y=0&l=60&k=glossary&t=2&w=supply+chain')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Switchgrass', NULL, N'A North American native perennial warm-season grass which is used to develop biofuels because of its high yield and ability to easily grow from the seed with little agricultural help. ', NULL, N'http://www.extension.org/pages/26635/switchgrass-panicum-virgatum-for-biofuel-production#.VIpWlYe3I40')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Synthetic Fuels', NULL, N'Synthetic gas or synthetic oil. Fuel that is artificially made as contrasted to that which is found in nature', NULL, N'http://www.energy.ca.gov/glossary/glossary-s.html')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Thermochemical Conversion', NULL, N'The transformation of complex organic material into light crude oil using pressure and heat', NULL, N'http://biofuel.org.uk/glossary.html')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Watershed', NULL, N'The land area that drains water to a particular stream, river, or lake. It is a land feature that can be identified by tracing a line along the highest elevations between two areas on a map, often a ridge.', NULL, N'http://agclass.nal.usda.gov/mtwdk.exe?k=glossary&l=60&w=9287&n=1&s=5&t=2')
INSERT [dbo].[Glossaries] ([term], [keywords], [description], [counter], [source]) VALUES (N'Yield', NULL, N'The amount of a crop produced in a given time or from a given place. The amount of product output recovered from a quantity of raw material input in forest product industries. Also, the estimate in forest mensuration of the amount of wood that may be harvested from a particular type of forest stand by species, site, stocking, and management regime at various ages.', NULL, N'http://www2.kenyon.edu/projects/farmschool/addins/glossary.htm & http://agclass.nal.usda.gov/mtwdk.exe?k=ForestEng&l=60&w=958&n=1&s=5&t=2')
");
        }
        
        public override void Down()
        {
            Sql(@"
                Delete FROM Glossaries;
");
        }
    }
}
