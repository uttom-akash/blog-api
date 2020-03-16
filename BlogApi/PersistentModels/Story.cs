using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_Rest_Api.Persistent_Model{
    public class Story{
        public Guid StoryId {get;set;}

        [Column(TypeName = "varchar(250)")]
        public string Title {get;set;}
        
        public string Body {get;set;}
        
        [DataType(DataType.Date)]
        public DateTime PublishedDate {get;set;}

        public DateTime LastModified {get;set;}
        
        public User Author {get;set;}
        
        [ForeignKey("Author")]
        public string AuthorId {get;set;}   

        public override string ToString(){
            return LastModified.ToString();
        }     
    }
}