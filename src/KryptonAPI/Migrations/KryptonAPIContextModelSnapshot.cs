using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using KryptonAPI.Data;

namespace KryptonAPI.Migrations
{
    [DbContext(typeof(KryptonAPIContext))]
    partial class KryptonAPIContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20896");

            modelBuilder.Entity("KryptonAPI.Data.Models.JobScheduler.Job", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedUTC");

                    b.Property<byte[]>("FinalResult");

                    b.Property<DateTime>("ModifiedUTC");

                    b.Property<long>("StatusId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("KryptonAPI.Data.Models.JobScheduler.JobItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedUTC");

                    b.Property<long>("JobId");

                    b.Property<string>("JsonResult");

                    b.Property<DateTime>("ModifiedUTC");

                    b.Property<long>("StatusId");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("StatusId");

                    b.ToTable("JobItems");
                });

            modelBuilder.Entity("KryptonAPI.Data.Models.JobScheduler.Status", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedUTC");

                    b.Property<DateTime>("ModifiedUTC");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("KryptonAPI.Data.Models.JobScheduler.Job", b =>
                {
                    b.HasOne("KryptonAPI.Data.Models.JobScheduler.Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KryptonAPI.Data.Models.JobScheduler.JobItem", b =>
                {
                    b.HasOne("KryptonAPI.Data.Models.JobScheduler.Job")
                        .WithMany()
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KryptonAPI.Data.Models.JobScheduler.Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
