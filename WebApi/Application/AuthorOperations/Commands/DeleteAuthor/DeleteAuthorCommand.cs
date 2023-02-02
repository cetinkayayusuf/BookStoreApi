using Microsoft.EntityFrameworkCore;
using WebApi.DBOperations;

namespace WebApi.Application.AuthorOperations.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand
    {
        public int AuthorId;
        private readonly IBookStoreDbContext _dbContext;
        public DeleteAuthorCommand(IBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Handle()
        {
            var author = _dbContext.Authors.Include(x => x.Book).SingleOrDefault(x => x.Id == AuthorId);
            if (author is null)
                throw new InvalidOperationException("Author not found");

            if (author.Book.Count() > 0)
                throw new InvalidOperationException("Author has book(s), cannot delete");

            _dbContext.Authors.Remove(author);
            _dbContext.SaveChanges();
        }
    }
}