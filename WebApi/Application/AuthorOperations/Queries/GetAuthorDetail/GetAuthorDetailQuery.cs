using WebApi.DBOperations;
using AutoMapper;
using WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Application.AuthorOperations.Queries.GetAuthorDetail
{
    public class GetAuthorDetailQuery
    {
        public int AuthorId;
        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetAuthorDetailQuery(BookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public AuthorDetailViewModel Handle()
        {
            var author = _dbContext.Authors.Include(x => x.Book).SingleOrDefault(x => x.Id == AuthorId);
            if (author is null)
                throw new InvalidOperationException("Author is not found");

            AuthorDetailViewModel vm = _mapper.Map<AuthorDetailViewModel>(author);
            return vm;
        }
    }

    public class AuthorDetailViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<string> Books { get; set; }
    }
}