using WebApi.DBOperations;
using WebApi.Entities;

namespace TestSetup
{
    public static class Books
    {
        public static void AddBooks(this BookStoreDbContext context)
        {
            context.Books.AddRange(
                new Book
                {
                    Title = "Start With Why",
                    GenreId = 1,
                    PageCount = 274,
                    PublishDate = new DateTime(2005, 1, 3),
                    AuthorId = 1
                },
                new Book
                {
                    Title = "Start With Dinner",
                    GenreId = 2,
                    PageCount = 172,
                    PublishDate = new DateTime(2004, 2, 2),
                    AuthorId = 2
                },
                new Book
                {
                    Title = "Start With Breakfast",
                    GenreId = 1,
                    PageCount = 1232,
                    PublishDate = new DateTime(2003, 2, 2),
                    AuthorId = 2
                }
            );
        }
    }
}