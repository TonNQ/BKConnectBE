using BKConnectBE.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKConnectBE.Model
{
    public class BKConnectContext : DbContext
    {
        public BKConnectContext(DbContextOptions<BKConnectContext> options) : base(options) { }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<FriendRequest> FriendRequests { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Relationship> Relationships { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomInvitation> RoomInvitations { get; set; }
        public virtual DbSet<SchoolYear> SchoolYears { get; set; }
        public virtual DbSet<UploadedFile> UploadedFiles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserOfRoom> UsersOfRoom { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasOne(r => r.Faculty)
                    .WithMany(r => r.Classes)
                    .HasForeignKey(r => r.FacultyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(r => r.Users)
                    .WithOne(r => r.Class)
                    .HasForeignKey(r => r.ClassId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<FriendRequest>(entity =>
            {
                entity.HasOne(r => r.Sender)
                    .WithMany()
                    .HasForeignKey(r => r.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Receiver)
                    .WithMany()
                    .HasForeignKey(r => r.ReceiverId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasOne(e => e.RootMessage)
                    .WithMany(e => e.ReplyMessage)
                    .HasForeignKey(e => e.RootMessageId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.Sender)
                    .WithMany(e => e.SentMessages)
                    .HasForeignKey(e => e.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Room)
                    .WithMany(e => e.Messages)
                    .HasForeignKey(e => e.RoomId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasOne(e => e.User)
                    .WithMany(e => e.RefreshTokens)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Relationship>(entity =>
            {
                entity.HasOne(r => r.User1)
                    .WithMany()
                    .HasForeignKey(r => r.User1Id)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.User2)
                    .WithMany()
                    .HasForeignKey(r => r.User2Id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasMany(r => r.Users)
                    .WithOne(r => r.Role)
                    .HasForeignKey(r => r.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasMany(r => r.UsersOfRoom)
                    .WithOne(r => r.Room)
                    .HasForeignKey(r => r.RoomId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(r => r.UploadedFiles)
                    .WithOne(r => r.Room)
                    .HasForeignKey(r => r.RoomId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(r => r.RoomInvitations)
                    .WithOne(r => r.Room)
                    .HasForeignKey(r => r.RoomId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RoomInvitation>(entity =>
            {
                entity.HasOne(r => r.Sender)
                    .WithMany()
                    .HasForeignKey(r => r.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Receiver)
                    .WithMany()
                    .HasForeignKey(r => r.ReceiverId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(r => r.UsersOfRoom)
                    .WithOne(r => r.User)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}