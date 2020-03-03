using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Blog_Rest_Api.DTOModels{
    public class RequestStoryDTO
    {
        public Guid StoryId {get;set;}=Guid.NewGuid();
        [StringLength(250,MinimumLength=10)]
        public string Title {get;set;}
        [MinLength(100)]
        public string Body {get;set;}
        [DataType(DataType.Date)]
        public DateTime PublishedDate {get;set;}
    }
}