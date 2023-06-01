using AutoMapper;

namespace BooksMVC.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            this.CreateMap<Book, EditBookVM>().ReverseMap();
        }
    }
}
