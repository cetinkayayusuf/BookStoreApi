using AutoMapper;
using WebApi.DBOperations;
using WebApi.Entities;

namespace WebApi.Application.BookOperations.Commands.AddBook
{
    public class AddBookCommand
    {
        public AddBookModel Model { get; set; }
        private readonly IBookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public AddBookCommand(IBookStoreDbContext dbContext, IMapper mapper)
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
        public int AuthorId { get; set; }
    }
}