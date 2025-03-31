﻿// <auto-generated />
using System;
using BussinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BussinessObjects.Migrations
{
    [DbContext(typeof(FptclubsContext))]
    [Migration("20250331170749_UpdateNotificationMessage")]
    partial class UpdateNotificationMessage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BussinessObjects.Models.Club", b =>
                {
                    b.Property<int>("ClubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("club_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClubId"));

                    b.Property<string>("ClubName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("club_name");

                    b.Property<byte[]>("Cover")
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("cover");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("description");

                    b.Property<byte[]>("Logo")
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("logo");

                    b.Property<bool>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true)
                        .HasColumnName("status");

                    b.HasKey("ClubId")
                        .HasName("PK__Clubs__BCAD3DD9E76837C6");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("BussinessObjects.Models.ClubMember", b =>
                {
                    b.Property<int>("MembershipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("membership_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MembershipId"));

                    b.Property<int>("ClubId")
                        .HasColumnType("int")
                        .HasColumnName("club_id");

                    b.Property<DateTime?>("JoinedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("joined_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    b.Property<bool>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true)
                        .HasColumnName("status");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("MembershipId")
                        .HasName("PK__ClubMemb__CAE49DDD6EBE5379");

                    b.HasIndex("ClubId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("ClubMembers");
                });

            modelBuilder.Entity("BussinessObjects.Models.ClubRequest", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("request_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RequestId"));

                    b.Property<string>("ClubName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("club_name");

                    b.Property<byte[]>("Cover")
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("cover");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(MAX)")
                        .HasColumnName("description");

                    b.Property<byte[]>("Logo")
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("logo");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("Pending")
                        .HasColumnName("status");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("RequestId")
                        .HasName("PK__ClubRequ__18D3B90F642177DA");

                    b.HasIndex("UserId");

                    b.ToTable("ClubRequests");
                });

            modelBuilder.Entity("BussinessObjects.Models.ClubTask", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("task_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaskId"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int")
                        .HasColumnName("created_by");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime")
                        .HasColumnName("due_date");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("Pending")
                        .HasColumnName("status");

                    b.Property<string>("TaskDescription")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("text")
                        .HasColumnName("task_description");

                    b.HasKey("TaskId")
                        .HasName("PK__Tasks__0492148DB46F1696");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("EventId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("BussinessObjects.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BussinessObjects.Models.Connection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConnectionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("connectAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Connections");
                });

            modelBuilder.Entity("BussinessObjects.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("event_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventId"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime")
                        .HasColumnName("event_date");

                    b.Property<string>("EventDescription")
                        .HasMaxLength(1000)
                        .HasColumnType("text")
                        .HasColumnName("event_description");

                    b.Property<string>("EventTitle")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("event_title");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EventId")
                        .HasName("PK__Events__2370F727D6C1A214");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("BussinessObjects.Models.JoinRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClubId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("UserId");

                    b.ToTable("JoinRequests");
                });

            modelBuilder.Entity("BussinessObjects.Models.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("notification_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<bool?>("IsRead")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("is_read");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("text")
                        .HasColumnName("message");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("NotificationId")
                        .HasName("PK__Notifica__E059842FE2370968");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("BussinessObjects.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("post_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(MAX)")
                        .HasColumnName("content");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int")
                        .HasColumnName("created_by");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("image");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasDefaultValue("Pending")
                        .HasColumnName("status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("PostId")
                        .HasName("PK__Posts__3ED78766A395B13D");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("BussinessObjects.Models.PostReaction", b =>
                {
                    b.Property<int>("ReactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReactionId"));

                    b.Property<bool>("IsLiked")
                        .HasColumnType("bit");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ReactionId")
                        .HasName("PK__Comments");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("BussinessObjects.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("role_name");

                    b.HasKey("RoleId")
                        .HasName("PK__Roles__760965CC319BD705");

                    b.HasIndex(new[] { "RoleName" }, "UQ__Roles__783254B1D499B73A")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BussinessObjects.Models.TaskAssignment", b =>
                {
                    b.Property<int>("AssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("assignment_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AssignmentId"));

                    b.Property<DateTime?>("AssignedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("assigned_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("MembershipId")
                        .HasColumnType("int")
                        .HasColumnName("membership_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TaskId")
                        .HasColumnType("int")
                        .HasColumnName("task_id");

                    b.HasKey("AssignmentId")
                        .HasName("PK__TaskAssi__DA8918149E9E4CAB");

                    b.HasIndex("MembershipId");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskAssignments");
                });

            modelBuilder.Entity("BussinessObjects.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("password");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("profile_picture");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("username");

                    b.HasKey("UserId")
                        .HasName("PK__Users__B9BE370F995A7F0D");

                    b.HasIndex(new[] { "Email" }, "UQ__Users__AB6E6164897924D5")
                        .IsUnique();

                    b.HasIndex(new[] { "Username" }, "UQ__Users__F3DBC57250E7F37C")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BussinessObjects.Models.ClubMember", b =>
                {
                    b.HasOne("BussinessObjects.Models.Club", "Club")
                        .WithMany("ClubMembers")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__ClubMembe__club___4316F928");

                    b.HasOne("BussinessObjects.Models.Role", "Role")
                        .WithMany("ClubMembers")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__ClubMembe__role___44FF419A");

                    b.HasOne("BussinessObjects.Models.User", "User")
                        .WithMany("ClubMembers")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__ClubMembe__user___440B1D61");

                    b.Navigation("Club");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BussinessObjects.Models.ClubRequest", b =>
                {
                    b.HasOne("BussinessObjects.Models.User", "User")
                        .WithMany("ClubRequests")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__ClubReque__user___656C112C");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BussinessObjects.Models.ClubTask", b =>
                {
                    b.HasOne("BussinessObjects.Models.ClubMember", "CreatedByNavigation")
                        .WithMany("Tasks")
                        .HasForeignKey("CreatedBy")
                        .IsRequired()
                        .HasConstraintName("FK__Tasks__created_b__571DF1D5");

                    b.HasOne("BussinessObjects.Models.Event", "Event")
                        .WithMany("Tasks")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedByNavigation");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("BussinessObjects.Models.Comment", b =>
                {
                    b.HasOne("BussinessObjects.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BussinessObjects.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BussinessObjects.Models.Connection", b =>
                {
                    b.HasOne("BussinessObjects.Models.User", "User")
                        .WithMany("Connections")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BussinessObjects.Models.Event", b =>
                {
                    b.HasOne("BussinessObjects.Models.ClubMember", "CreatedByNavigation")
                        .WithMany("Events")
                        .HasForeignKey("CreatedBy")
                        .IsRequired()
                        .HasConstraintName("FK__Events__created___52593CB8");

                    b.Navigation("CreatedByNavigation");
                });

            modelBuilder.Entity("BussinessObjects.Models.JoinRequest", b =>
                {
                    b.HasOne("BussinessObjects.Models.Club", "Club")
                        .WithMany("JoinRequests")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BussinessObjects.Models.User", "User")
                        .WithMany("JoinRequests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Club");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BussinessObjects.Models.Notification", b =>
                {
                    b.HasOne("BussinessObjects.Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__Notificat__user___60A75C0F");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BussinessObjects.Models.Post", b =>
                {
                    b.HasOne("BussinessObjects.Models.ClubMember", "ClubMember")
                        .WithMany("Posts")
                        .HasForeignKey("CreatedBy")
                        .IsRequired()
                        .HasConstraintName("FK__Posts__created_b__49C3F6B7");

                    b.Navigation("ClubMember");
                });

            modelBuilder.Entity("BussinessObjects.Models.PostReaction", b =>
                {
                    b.HasOne("BussinessObjects.Models.Post", "Post")
                        .WithMany("Reactions")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BussinessObjects.Models.User", "User")
                        .WithMany("Reactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BussinessObjects.Models.TaskAssignment", b =>
                {
                    b.HasOne("BussinessObjects.Models.ClubMember", "Membership")
                        .WithMany("TaskAssignments")
                        .HasForeignKey("MembershipId")
                        .IsRequired()
                        .HasConstraintName("FK__TaskAssig__membe__5BE2A6F2");

                    b.HasOne("BussinessObjects.Models.ClubTask", "Task")
                        .WithMany("TaskAssignments")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__TaskAssig__task___5AEE82B9");

                    b.Navigation("Membership");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("BussinessObjects.Models.Club", b =>
                {
                    b.Navigation("ClubMembers");

                    b.Navigation("JoinRequests");
                });

            modelBuilder.Entity("BussinessObjects.Models.ClubMember", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Posts");

                    b.Navigation("TaskAssignments");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("BussinessObjects.Models.ClubTask", b =>
                {
                    b.Navigation("TaskAssignments");
                });

            modelBuilder.Entity("BussinessObjects.Models.Event", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("BussinessObjects.Models.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Reactions");
                });

            modelBuilder.Entity("BussinessObjects.Models.Role", b =>
                {
                    b.Navigation("ClubMembers");
                });

            modelBuilder.Entity("BussinessObjects.Models.User", b =>
                {
                    b.Navigation("ClubMembers");

                    b.Navigation("ClubRequests");

                    b.Navigation("Comments");

                    b.Navigation("Connections");

                    b.Navigation("JoinRequests");

                    b.Navigation("Notifications");

                    b.Navigation("Reactions");
                });
#pragma warning restore 612, 618
        }
    }
}
