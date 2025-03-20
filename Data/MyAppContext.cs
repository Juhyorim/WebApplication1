using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.limeboard;

namespace WebApplication1.Data
{
    //main bridge that connects our project with the db
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //limeboard
            modelBuilder.Entity<Member>()
                .Property(m => m.JoinAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Board>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            //item, category, client
            modelBuilder.Entity<ItemClient>().HasKey(ic => new
            {
                ic.ItemId,
                ic.ClientId
            });

            modelBuilder.Entity<ItemClient>().HasOne(i => i.Item).WithMany(ic => ic.ItemClients).HasForeignKey(i => i.ItemId);
            modelBuilder.Entity<ItemClient>().HasOne(c => c.Client).WithMany(ic => ic.ItemClients).HasForeignKey(c => c.ClientId);

            // 관계 설정
            modelBuilder.Entity<Item>()
                .HasOne(i => i.SerialNumber)
                .WithOne(s => s.Item)
                .HasForeignKey<SerialNumber>(s => s.ItemId);

            // 먼저 Item 데이터를 추가하되, SerialNumberId는 null로 시작
            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 8, Name = "microphone", Price = 40 }
            );

            // 그 다음 SerialNumber 데이터 추가
            modelBuilder.Entity<SerialNumber>().HasData(
                new SerialNumber { Id = 11, Name = "MIC150", ItemId = 8 }
            );

            modelBuilder.Entity<Category>().HasData(
                    new Category { Id = 1, Name = "Electronics" },
                    new Category { Id = 2, Name = "Books" }
                );

            base.OnModelCreating(modelBuilder);
        }

        //limeboard
        public DbSet<Member> Members {  get; set; }
        public DbSet<Board> Boards { get; set; }

        //item, category, client
        public DbSet<Item> Items { get; set; }
        public DbSet<SerialNumber> SerialNumbers { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ItemClient> ItemClients { get; set; }

    }
}
