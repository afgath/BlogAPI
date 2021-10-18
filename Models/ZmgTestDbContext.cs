using System;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace zmgTestBack.Models
{
    public partial class ZmgTestDbContext : DbContext
    {
        public ZmgTestDbContext()
        {
        }

        public ZmgTestDbContext(DbContextOptions<ZmgTestDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersRole> UsersRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comments");

                entity.Property(e => e.CommentId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("comment_id");

                entity.Property(e => e.Contents)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("contents");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.CreationUserId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("creation_user");

                entity.Property(e => e.Dislikes)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("dislikes");

                entity.Property(e => e.IsReview).HasColumnName("is_review");

                entity.Property(e => e.Likes)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("likes");

                entity.Property(e => e.PostId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("post_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comments_posts_FK");
                //comments_users_FK
                entity.HasOne(d => d.CreationUser)
                    .WithMany(p => p.CommentCreationUsers)
                    .HasForeignKey(d => d.CreationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comments_users_FK");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("posts");

                entity.Property(e => e.PostId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("post_id");

                entity.Property(e => e.Contents)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("contents");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.CreationUserId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("creation_user");

                entity.Property(e => e.Dislikes)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("dislikes");

                entity.Property(e => e.Likes)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("likes");

                entity.Property(e => e.ModificationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modification_date");

                entity.Property(e => e.ModificationUserId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("modification_user");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.Views)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("views");

                entity.HasOne(d => d.CreationUser)
                    .WithMany(p => p.PostCreationUsers)
                    .HasForeignKey(d => d.CreationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("posts_users_FK");

                entity.HasOne(d => d.ModificationUser)
                    .WithMany(p => p.PostModificationUsers)
                    .HasForeignKey(d => d.ModificationUserId)
                    .HasConstraintName("posts_users_2_FK");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.RoleId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("role_id");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("role_name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.UserId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<UsersRole>(entity =>
            {
                entity.HasKey(e => e.UsersRolesId);

                entity.ToTable("users_roles");

                entity.Property(e => e.UsersRolesId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("users_roles_id");

                entity.Property(e => e.RoleId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("role_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UsersRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("users_roles_roles_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("users_roles_users_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
