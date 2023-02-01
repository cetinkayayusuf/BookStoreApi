using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.DBOperations
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookStoreDbContext(serviceProvider.GetRequiredService<DbContextOptions<BookStoreDbContext>>()))
            {
                if (context.Books.Any())
                    return;
                context.Books.AddRange(
                    new Book
                    {
                        Title = "Start With Why",
                        GenreId = 1,
                        PageCount = 274,
                        PublishDate = new DateTime(2005, 1, 3)
                    },
                    new Book
                    {
                        Title = "Start With Dinner",
                        GenreId = 2,
                        PageCount = 172,
                        PublishDate = new DateTime(2004, 2, 2)
                    }
                );
                context.SaveChanges();
            }
        }
    }
}