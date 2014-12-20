namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notsure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Glossaries",
                c => new
                    {
                        term = c.String(nullable: false, maxLength: 128),
                        keywords = c.String(),
                        description = c.String(),
                        counter = c.Int(),
                    })
                .PrimaryKey(t => t.term);

            Sql(@"

INSERT [dbo].[Glossaries] ([TERM], [KEYWORDS], [DESCRIPTION], [COUNTER]) VALUES (N'Bingo', N'Bingo; Dog; Famer;', N'Bingo was a dog', NULL)
;
INSERT [dbo].[Glossaries] ([TERM], [KEYWORDS], [DESCRIPTION], [COUNTER]) VALUES (N'Good Evening', N'Greeting; Hello', N'Grood evening is a greeting', NULL)
;
INSERT [dbo].[Glossaries] ([TERM], [KEYWORDS], [DESCRIPTION], [COUNTER]) VALUES (N'Hello', N'Greeting; Good Evening', N'Greeting', NULL)
;


");
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Glossaries");
        }
    }
}
