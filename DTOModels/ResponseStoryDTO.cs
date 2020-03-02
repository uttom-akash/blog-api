using System;
using System.ComponentModel.DataAnnotations;
using Blog_Rest_Api.Persistent_Model;

namespace Blog_Rest_Api.DTOModels{
    public class ResponseStoryDTO
    {
        public Guid StoryId {get;set;}
        public string Title {get;set;}
        public string Body {get;set;}
        [DataType(DataType.Date)]
        public DateTime PublishedDate {get;set;}
        public string AuthorId {get;set;}
        public AuthorDTO Author {get;set;}
    }
}