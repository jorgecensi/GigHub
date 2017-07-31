namespace GigHub.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PopulateGenresTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO GENRES (Id, Name) VALUES (1,'Techno')");
            Sql("INSERT INTO GENRES (Id, Name) VALUES (2,'Deep House')");
            Sql("INSERT INTO GENRES (Id, Name) VALUES (3,'Rock')");
            Sql("INSERT INTO GENRES (Id, Name) VALUES (4,'House')");
        }

        public override void Down()
        {
            Sql("DELETE FROM GENRES WHERE Id IN (1,2,3,4)");
        }
    }
}
