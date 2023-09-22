using ChatFriend.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatFriend.Model;

public class BKConnectContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Account> Accounts { get; set; }

    public BKConnectContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasOne(user => user.Account)
            .WithOne(account => account.User)
            .HasForeignKey<Account>(account => account.UserId);
    }
}
