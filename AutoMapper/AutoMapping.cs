using AutoMapper;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;

namespace Blog_Rest_Api.Auto_Mapper{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            // STORY
            CreateMap<Story,RequestStoryDTO>();
            CreateMap<RequestStoryDTO,Story>();
            
            CreateMap<Story,ResponseStoryDTO>();
            
            // USER
            CreateMap<User,AuthorDTO>();
            CreateMap<User,UserInfoDTO>();
            CreateMap<User,LoggedInUserDTO>();
            CreateMap<UserRegistrationDTO,User>();
        }
    }
}