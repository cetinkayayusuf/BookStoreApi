using WebApi.DBOperations;
using WebApi.Common;

namespace WebApi.BookOperations.GetById
{
    public class GetByIdQuery
    {
        public int Id { get; set; }
        private readonly BookStoreDbContext _dbContext;
        public GetByIdQuery(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetByIdViewModel Handle()
        {
            var book = _dbContext.Books.Where(x => x.Id == Id).SingleOrDefault<Book>();

            if (book is null)
                throw new InvalidOperationException("Book not found");

            GetByIdViewModel vm = new GetByIdViewModel()
            {
                Title = book.Title,
                Genre = ((GenreEnum)book.GenreId).ToString(),
                PageCount = book.PageCount,
                PublishDate = book.PublishDate.Date.ToString("dd/MM/yyyy")
            };

            return vm;
        }
    }

    public class GetByIdViewModel
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int PageCount { get; set; }
        public string PublishDate { get; set; }
    }
}