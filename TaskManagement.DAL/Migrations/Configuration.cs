using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using TaskManagement.DAL.Models;

namespace TaskManagement.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TaskManagement.DAL.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TaskManagement.DAL.DataContext context)
        {
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
            TimeSpan time = new TimeSpan(8, 0, 0);

            context.UserTasks.AddOrUpdate(t => t.Title,
                new UserTask()
                {
                    Title = "Task 1",
                    PlannedHours = time,
                    Description = "test",
                    Executors = "Belova",
                    Status = Statuses.Appointed,
                    UserTasks = new ObservableCollection<UserTask>
                {
                    new UserTask() {Title = "Task 1.2", PlannedHours = time, Description = "description", Executors = "Ivanov", Status = Statuses.Completed, SubTasksActualExecutionTime = new TimeSpan(28, 0, 0)
                } }
                });
          context.SaveChanges();
        }
    }
}
