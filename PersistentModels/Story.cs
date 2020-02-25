using System;
using System.ComponentModel.DataAnnotations;

namespace Blog_Rest_Api.Persistent_Model{
    public class Story{
        public Guid StoryId {get;set;}
        [MinLength(10)]
        public string Title {get;set;}
        [MinLength(100)]
        public string Body {get;set;}
        public DateTime PublishedDate {get;set;}        
    }
}