using WebApi.DBOperations;

namespace WebApi.Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommand
    {
        public int AuthorId;
        public UpdateAuthorModel Model;
        private readonly IBookStoreDbContext _dbContext;
        public UpdateAuthorCommand(IBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Handle()
        {
            var author = _dbContext.Authors.SingleOrDefault(x => x.Id == AuthorId);
            if (author is null)
                throw new InvalidOperationException("Author not found");

            author.FirstName = string.IsNullOrEmpty(Model.FirstName?.Trim()) ? author.FirstName : Model.FirstName;
            author.LastName = string.IsNullOrEmpty(Model.LastName?.Trim()) ? author.LastName : Model.LastName;
            author.BirthDate = Model.BirthDate == default ? author.BirthDate : Model.BirthDate;

            _dbContext.SaveChanges();
        }
    }

    public class UpdateAuthorModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}