using System.ComponentModel.DataAnnotations;
using Blog_Rest_Api.Custom_Attribute;

namespace Blog_Rest_Api.DTOModels{
    public class UserCredentialsDTO
    {
        [Required]
        public string UserId {get;set;}      
        [Required]  
        public string Password {get;set;} 
    }
}