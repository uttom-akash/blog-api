
using Blog_Rest_Api.Custom_Attribute;

namespace Blog_Rest_Api.DTOModels{
    public class UpdateUserPasswordDTO
    {
        
        public string UserId {get;set;}
        [ValidatePassword(minLength=5,minAlphabet=2,minNumeric=1)]
        public string OldPassword {get;set;} 
        [ValidatePassword(minLength=5,minAlphabet=2,minNumeric=1)]
        public string NewPassword {get;set;} 
         
    }
 }