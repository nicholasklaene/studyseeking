﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using api.Data;

#nullable disable

namespace api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("api.Models.Application", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<short>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<string>("Subdomain")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)")
                        .HasColumnName("subdomain");

                    b.HasKey("Id");

                    b.ToTable("applications", (string)null);
                });

            modelBuilder.Entity("api.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<short>("ApplicationId")
                        .HasColumnType("smallint");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("label");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("api.Models.CategoryHasSuggestedTag", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    b.Property<string>("TagLabel")
                        .HasColumnType("text")
                        .HasColumnName("tag_label");

                    b.HasKey("CategoryId", "TagLabel");

                    b.HasIndex("TagLabel");

                    b.ToTable("category_has_suggested_tag", (string)null);
                });

            modelBuilder.Entity("api.Models.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("Preview")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("preview");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("title");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("post", (string)null);
                });

            modelBuilder.Entity("api.Models.PostHasTag", b =>
                {
                    b.Property<long>("PostId")
                        .HasColumnType("bigint")
                        .HasColumnName("post_id");

                    b.Property<string>("TagLabel")
                        .HasColumnType("text")
                        .HasColumnName("tag_label");

                    b.HasKey("PostId", "TagLabel");

                    b.HasIndex("TagLabel");

                    b.ToTable("post_has_tag", (string)null);
                });

            modelBuilder.Entity("api.Models.Tag", b =>
                {
                    b.Property<string>("Label")
                        .HasColumnType("text")
                        .HasColumnName("label");

                    b.HasKey("Label");

                    b.ToTable("tags", (string)null);
                });

            modelBuilder.Entity("api.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("email");

                    b.HasKey("Username");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("api.Models.Category", b =>
                {
                    b.HasOne("api.Models.Application", "Application")
                        .WithMany("Categories")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("api.Models.CategoryHasSuggestedTag", b =>
                {
                    b.HasOne("api.Models.Category", "Category")
                        .WithMany("CategoryHasSuggestedTags")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Models.Tag", "Tag")
                        .WithMany("CategoryHasSuggestedTags")
                        .HasForeignKey("TagLabel")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("api.Models.Post", b =>
                {
                    b.HasOne("api.Models.Category", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Models.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("api.Models.PostHasTag", b =>
                {
                    b.HasOne("api.Models.Post", "Post")
                        .WithMany("PostHasTags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Models.Tag", "Tag")
                        .WithMany("PostHasTags")
                        .HasForeignKey("TagLabel")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("api.Models.Application", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("api.Models.Category", b =>
                {
                    b.Navigation("CategoryHasSuggestedTags");

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("api.Models.Post", b =>
                {
                    b.Navigation("PostHasTags");
                });

            modelBuilder.Entity("api.Models.Tag", b =>
                {
                    b.Navigation("CategoryHasSuggestedTags");

                    b.Navigation("PostHasTags");
                });

            modelBuilder.Entity("api.Models.User", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
