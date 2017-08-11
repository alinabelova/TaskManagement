namespace TaskManagement.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTaskDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTask",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Updated = c.DateTime(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Executors = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        PlannedHours = c.Time(nullable: false, precision: 7),
                        ActualExecutionTime = c.Time(precision: 7),
                        FinishedDate = c.DateTime(),
                        UserTaskId = c.Int(),
                        SubTasksPlannedHoursTicks = c.Long(),
                        SubTasksActualExecutionTimeTicks = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserTask", t => t.UserTaskId)
                .Index(t => t.UserTaskId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTask", "UserTaskId", "dbo.UserTask");
            DropIndex("dbo.UserTask", new[] { "UserTaskId" });
            DropTable("dbo.UserTask");
        }
    }
}
