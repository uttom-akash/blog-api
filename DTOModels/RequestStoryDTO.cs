using System;
using System.ComponentModel.DataAnnotations;
using Blog_Rest_Api.Persistent_Model;

namespace Blog_Rest_Api.DTOModels{
    public class RequestStoryDTO
    {
        public Guid StoryId {get;set;}
        [MinLength(10)]
        [MaxLength(250)]
        public string Title {get;set;}
        [MinLength(100)]
        public string Body {get;set;}
        [DataType(DataType.Date)]
        public DateTime PublishedDate {get;set;}
    }
}