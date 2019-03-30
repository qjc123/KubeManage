﻿// <auto-generated />
using System;
using KubeManage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KubeManage.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity("KubeManage.Entity.Docker.DockerImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Image");

                    b.Property<DateTime>("Time");

                    b.Property<string>("Version");

                    b.HasKey("Id");

                    b.ToTable("DockerImages");
                });

            modelBuilder.Entity("KubeManage.Entity.Manager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.ToTable("Managers");
                });
#pragma warning restore 612, 618
        }
    }
}
