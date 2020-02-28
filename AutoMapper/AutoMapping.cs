using AutoMapper;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;

namespace Blog_Rest_Api.Auto_Mapper{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Story,StoryDTO>();
            CreateMap<User,AuthorDTO>();
        }
    }
}