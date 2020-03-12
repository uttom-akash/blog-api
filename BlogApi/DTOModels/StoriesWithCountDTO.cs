using System;
using System.Collections.Generic;

namespace Blog_Rest_Api.DTOModels{
    public class StoriesWithCountDTO
    {
        public List<ResponseStoryDTO> Stories {get;set;}
        public int Total {get;set;}
    }
}