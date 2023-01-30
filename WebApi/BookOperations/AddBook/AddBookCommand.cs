using WebApi.DBOperations;

namespace WebApi.BookOperations.AddBook
{
    public class AddBookCommand
    {
        public AddBookModel Model { get; set; }
        private readonly BookStoreDbContext _dbContext;
        public AddBookCommand(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Handle()
        {
            var book = _dbContext.Books.SingleOrDefault(x => x.Title == Model.Title);

            if (book is not null)
                throw new InvalidOperationException("Book exist");

            book = new Book();
            book.Title = Model.Title;
            book.GenreId = Model.GenreId;
            book.PageCount = Model.PageCount;
            book.PublishDate = Model.PublishDate;

            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();
        }
    }

    public class AddBookModel
    {
        public string Title { get; set; }
        public int GenreId { get; set; }
        public int PageCount { get; set; }
        public DateTime PublishDate { get; set; }
    }
}