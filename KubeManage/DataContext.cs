using System;
using System.IO;
using System.Reflection;
using KubeManage.Entity.Docker;
using Microsoft.EntityFrameworkCore;

namespace KubeManage
{
    public class DataContext : DbContext
    {
        public DbSet<DockerImage> DockerImages { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                string dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                if (!Directory.Exists(Path.Combine(dir, "db")))
                {
                    Directory.CreateDirectory(Path.Combine(dir, "db"));
                }

                string dbPath = Path.Combine(dir, "db", "data.db");

                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
            else
            {
                optionsBuilder.UseSqlite($"Data Source=data.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DockerImage>().HasKey(t => t.Id);
            modelBuilder.Entity<DockerImage>().Property(t => t.Id).ValueGeneratedOnAdd();
        }
    }
}