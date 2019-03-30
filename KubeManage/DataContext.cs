using System;
using System.IO;
using System.Reflection;
using KubeManage.Entity;
using KubeManage.Entity.Docker;
using KubeManage.Util;
using Microsoft.EntityFrameworkCore;

namespace KubeManage
{
    public class DataContext : DbContext
    {
        public DbSet<DockerImage> DockerImages { get; set; }
        
        public DbSet<Manager> Managers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                string dir = PathHelper.BinPath;

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

            var dockerImage = modelBuilder.Entity<DockerImage>();
            dockerImage.HasKey(t => t.Id);
            dockerImage.Property(t => t.Id).ValueGeneratedOnAdd();

            var manager = modelBuilder.Entity<Manager>();
            manager.HasKey(t => t.Id);
            manager.Property(t => t.Id).ValueGeneratedOnAdd();
        }
    }
}