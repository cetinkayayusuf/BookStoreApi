using AutoMapper;
using WebApi.BookOperations.AddBook;
using WebApi.BookOperations.GetBookDetail;
using WebApi.BookOperations.GetBooks;

namespace WebApi.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddBookModel, Book>();
            CreateMap<Book, BookDetailViewModel>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => ((GenreEnum)src.GenreId).ToString()))
                .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishDate.Date.ToString("dd/MM/yyyy")));

            CreateMap<Book, BooksViewModel>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => ((GenreEnum)src.GenreId).ToString()))
                .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishDate.Date.ToString("dd/MM/yyyy")));
        }
    }
}