using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProjectDummy.Models;

public partial class FptclubsContext : DbContext
{
    public FptclubsContext()
    {
    }

    public FptclubsContext(DbContextOptions<FptclubsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<ClubMember> ClubMembers { get; set; }

    public virtual DbSet<ClubRequest> ClubRequests { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostInteraction> PostInteractions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskAssignment> TaskAssignments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=SUN-PHAN;uid=sa;pwd=sa;database=FPTClubs;TrustServerCertificate=true;Trusted_Connection=SSPI;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.ClubId).HasName("PK__Clubs__BCAD3DD9E76837C6");

            entity.Property(e => e.ClubId).HasColumnName("club_id");
            entity.Property(e => e.ClubName)
                .HasMaxLength(100)
                .HasColumnName("club_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
        });

        modelBuilder.Entity<ClubMember>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__ClubMemb__CAE49DDD6EBE5379");

            entity.Property(e => e.MembershipId).HasColumnName("membership_id");
            entity.Property(e => e.ClubId).HasColumnName("club_id");
            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("joined_at");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Club).WithMany(p => p.ClubMembers)
                .HasForeignKey(d => d.ClubId)
                .HasConstraintName("FK__ClubMembe__club___4316F928");

            entity.HasOne(d => d.Role).WithMany(p => p.ClubMembers)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__ClubMembe__role___44FF419A");

            entity.HasOne(d => d.User).WithMany(p => p.ClubMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClubMembe__user___440B1D61");
        });

        modelBuilder.Entity<ClubRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__ClubRequ__18D3B90F642177DA");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.ClubName)
                .HasMaxLength(100)
                .HasColumnName("club_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.ClubRequests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClubReque__user___656C112C");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Events__2370F727D6C1A214");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.EventDate)
                .HasColumnType("datetime")
                .HasColumnName("event_date");
            entity.Property(e => e.EventDescription)
                .HasColumnType("text")
                .HasColumnName("event_description");
            entity.Property(e => e.EventTitle)
                .HasMaxLength(255)
                .HasColumnName("event_title");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__created___52593CB8");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__E059842FE2370968");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("is_read");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__user___60A75C0F");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Posts__3ED78766A395B13D");

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending")
                .HasColumnName("status");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Posts__created_b__49C3F6B7");
        });

        modelBuilder.Entity<PostInteraction>(entity =>
        {
            entity.HasKey(e => e.InteractionId).HasName("PK__PostInte__605F8FE626355413");

            entity.Property(e => e.InteractionId).HasColumnName("interaction_id");
            entity.Property(e => e.CommentText)
                .HasColumnType("text")
                .HasColumnName("comment_text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany(p => p.PostInteractions)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__PostInter__post___4D94879B");

            entity.HasOne(d => d.User).WithMany(p => p.PostInteractions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PostInter__user___4E88ABD4");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__760965CC319BD705");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__783254B1D499B73A").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tasks__0492148DB46F1696");

            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DueDate)
                .HasColumnType("datetime")
                .HasColumnName("due_date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending")
                .HasColumnName("status");
            entity.Property(e => e.TaskDescription)
                .HasColumnType("text")
                .HasColumnName("task_description");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tasks__created_b__571DF1D5");
        });

        modelBuilder.Entity<TaskAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PK__TaskAssi__DA8918149E9E4CAB");

            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("assigned_at");
            entity.Property(e => e.MembershipId).HasColumnName("membership_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Membership).WithMany(p => p.TaskAssignments)
                .HasForeignKey(d => d.MembershipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaskAssig__membe__5BE2A6F2");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskAssignments)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__TaskAssig__task___5AEE82B9");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370F995A7F0D");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E6164897924D5").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Users__F3DBC57250E7F37C").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(255)
                .HasColumnName("profile_picture");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
