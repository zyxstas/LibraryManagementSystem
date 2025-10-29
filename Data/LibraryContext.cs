using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Айзек Азимов", DateOfBirth = new DateTime(1920, 1, 2) },
                new Author { Id = 2, Name = "Артур Конан Дойл", DateOfBirth = new DateTime(1859, 5, 22) },
                new Author { Id = 3, Name = "Александр Дюма", DateOfBirth = new DateTime(1802, 7, 24) }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Я, Робот", PublishedYear = 1950, AuthorId = 1 },
                new Book { Id = 2, Title = "Основание", PublishedYear = 1951, AuthorId = 1 },
                new Book { Id = 3, Title = "Стальные пещеры", PublishedYear = 1954, AuthorId = 1 },
                new Book { Id = 4, Title = "Этюд в багровых тонах", PublishedYear = 1887, AuthorId = 2 },
                new Book { Id = 5, Title = "Знак четырех", PublishedYear = 1890, AuthorId = 2 },
                new Book { Id = 6, Title = "Собака Баскервилей", PublishedYear = 1902, AuthorId = 2 },
                new Book { Id = 7, Title = "Три мушкетера", PublishedYear = 1844, AuthorId = 3 },
                new Book { Id = 8, Title = "Граф Монте-Кристо", PublishedYear = 1844, AuthorId = 3 },
                new Book { Id = 9, Title = "Королева Марго", PublishedYear = 1845, AuthorId = 3 }
            );
        }
    }
}