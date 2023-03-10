using AutoMapper;
using WebApi.DBOperations;
using WebApi.Entities;

namespace WebApi.Application.GenreOperations.Commands.AddGenre
{
    public class AddGenreCommand
    {
        public AddGenreModel Model { get; set; }
        private readonly IBookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public AddGenreCommand(IBookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Handle()
        {
            var genre = _dbContext.Genres.SingleOrDefault(x => x.Name.ToLower() == Model.Name.ToLower());

            if (genre is not null)
                throw new InvalidOperationException("Genre exist");

            genre = _mapper.Map<Genre>(Model);

            _dbContext.Genres.Add(genre);
            _dbContext.SaveChanges();
        }
    }

    public class AddGenreModel
    {
        public string Name { get; set; }
    }
}