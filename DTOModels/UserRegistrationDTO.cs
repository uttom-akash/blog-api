using System.ComponentModel.DataAnnotations;
using Blog_Rest_Api.Custom_Attribute;
using Blog_Rest_Api.Persistent_Model;

namespace Blog_Rest_Api.DTOModels{
    public class UserRegistrationDTO
    {
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Characters should be Alphanumeric")]
        [MinLength(5)]
        [MaxLength(100)]
        public string UserId {get;set;}        
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Characters should be Alphabate")]
        [MinLength(5)]
        [MaxLength(100)]
        public string FirstName {get;set;}
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Characters should be Alphabate")]
        [MaxLength(100)]
        public string LastName {get;set;}
        [ValidatePassword(minLength=5,minAlphabet=2,minNumeric=1)]
        public string Password {get;set;} 
    }
}