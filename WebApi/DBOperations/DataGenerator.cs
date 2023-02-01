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

                context.Authors.AddRange(
                                    new Author
                                    {
                                        FirstName = "Cem1",
                                        LastName = "Cem2",
                                        BirthDate = new DateTime(2005, 1, 3)
                                    },
                                    new Author
                                    {
                                        FirstName = "Cam1",
                                        LastName = "Cam2",
                                        BirthDate = new DateTime(2001, 1, 1)
                                    }
                                );
                context.Genres.AddRange(
                    new Genre
                    {
                        Name = "Scifi"
                    },
                    new Genre
                    {
                        Name = "Life"
                    }
                );

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
                context.SaveChanges();
            }
        }
    }
}