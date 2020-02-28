using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog_Rest_Api.Persistent_Model{
    public class User{
        [Required]
        public string UserId {get;set;}        
        [Required]
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string PasswordHash {get;set;}
    }
}