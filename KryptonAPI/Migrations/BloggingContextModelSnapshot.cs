using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using KryptonAPI;

namespace KryptonAPI.Migrations
{
    [DbContext(typeof(BloggingContext))]
    partial class BloggingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("KryptonAPI.Models.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Url");

                    b.HasKey("BlogId");
                });

            modelBuilder.Entity("KryptonAPI.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlogId");

                    b.Property<string>("Content");

                    b.Property<string>("Title");

                    b.HasKey("PostId");
                });

            modelBuilder.Entity("KryptonAPI.Models.Post", b =>
                {
                    b.HasOne("KryptonAPI.Models.Blog")
                        .WithMany()
                        .HasForeignKey("BlogId");
                });
        }
    }
}
