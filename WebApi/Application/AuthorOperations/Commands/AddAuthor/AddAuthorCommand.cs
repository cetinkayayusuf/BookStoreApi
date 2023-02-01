using WebApi.DBOperations;
using AutoMapper;
using WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Application.AuthorOperations.Commands.AddAuthor
{
    public class AddAuthorCommand
    {
        public AddAuthorModel Model;
        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public AddAuthorCommand(BookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Handle()
        {
            var author = _dbContext.Authors.SingleOrDefault(x => x.FirstName.ToLower() == Model.FirstName.ToLower() && x.LastName.ToLower() == Model.LastName.ToLower() && x.BirthDate.Date == Model.BirthDate.Date);
            if (author is not null)
                throw new InvalidOperationException("Author already exist");

            author = _mapper.Map<Author>(Model);

            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();
        }
    }

    public class AddAuthorModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}