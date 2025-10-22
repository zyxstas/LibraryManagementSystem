using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class DataService
    {
        public List<Author> Authors { get; set; } = new List<Author>();

        public List<Book> Books { get; set; } = new List<Book>();

        private int _nextAuthorId = 1;
        private int _nextBookId = 1;

        public DataService()
        {
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            var author1 = new Author { Id = GetNextAuthorId(), Name = "Айзек Азимов", DateOfBirth = new DateTime(1920, 1, 2) };
            var author2 = new Author { Id = GetNextAuthorId(), Name = "Артур Конан Дойл", DateOfBirth = new DateTime(1859, 5, 22) };
            var author3 = new Author { Id = GetNextAuthorId(), Name = "Александр Дюма", DateOfBirth = new DateTime(1802, 7, 24) };

            Authors.AddRange(new[] { author1, author2, author3 });

            Books.AddRange(new[]
            {
                new Book { Id = GetNextBookId(), Title = "Конец Вечности", PublishedYear = 1955, AuthorId = author1.Id },
                new Book { Id = GetNextBookId(), Title = "Основание", PublishedYear = 1951, AuthorId = author1.Id },
                new Book { Id = GetNextBookId(), Title = "Этюд в багровых тонах", PublishedYear = 1887, AuthorId = author2.Id },
                new Book { Id = GetNextBookId(), Title = "Собака Баскервилей", PublishedYear = 1902, AuthorId = author2.Id },
                new Book { Id = GetNextBookId(), Title = "Граф Монте-Кристо", PublishedYear = 1844, AuthorId = author3.Id },
                new Book { Id = GetNextBookId(), Title = "Три мушкетера", PublishedYear = 1844, AuthorId = author3.Id }
            });
        }

        public int GetNextAuthorId() => _nextAuthorId++;
        public int GetNextBookId() => _nextBookId++;
    }
}