using System;
using System.ComponentModel.DataAnnotations;

namespace Blog_Rest_Api.Persistent_Model{
    public class UserInfoDTO{
        public string UserId {get;set;}        
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string JwtToken {get;set;}
    }
}