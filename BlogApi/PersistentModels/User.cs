using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_Rest_Api.Persistent_Model{
    public class User{
        [Column(TypeName = "varchar(100)")]
        public string UserId {get;set;}        
        
        [Column(TypeName = "varchar(100)")]
        public string FirstName {get;set;}
        
        [Column(TypeName = "varchar(100)")]
        public string LastName {get;set;}
        
        [Column(TypeName = "varchar(70)")]
        public string PasswordHash {get;set;}
    }
}