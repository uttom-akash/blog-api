using System.ComponentModel.DataAnnotations;
using Blog_Rest_Api.Custom_Attribute;

namespace Blog_Rest_Api.DTOModels{
    public class UserCredentialsDTO
    {
        [Required]
        public string UserId {get;set;}      
        [Required]  
        [ValidatePassword(minLength=5,minAlphabet=2,minNumeric=1)]
        public string Password {get;set;} 
    }
}