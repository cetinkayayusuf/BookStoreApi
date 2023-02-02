using WebApi.DBOperations;
using WebApi.Entities;

namespace TestSetup
{
    public static class Authors
    {
        public static void AddAuthors(this BookStoreDbContext context)
        {
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
        }
    }
}