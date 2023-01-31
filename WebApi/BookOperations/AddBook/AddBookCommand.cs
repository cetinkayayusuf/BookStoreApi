using AutoMapper;
using WebApi.DBOperations;

namespace WebApi.BookOperations.AddBook
{
    public class AddBookCommand
    {
        public AddBookModel Model { get; set; }
        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public AddBookCommand(BookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Handle()
        {
            var book = _dbContext.Books.SingleOrDefault(x => x.Title == Model.Title);

            if (book is not null)
                throw new InvalidOperationException("Book exist");

            book = _mapper.Map<Book>(Model);

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