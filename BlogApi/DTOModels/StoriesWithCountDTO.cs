using System;
using System.Collections.Generic;

namespace Blog_Rest_Api.DTOModels
{
    public class StoriesWithCountDTO
    {
        public IEnumerable<ResponseStoryDTO> Stories { get; set; }
        public int Total { get; set; }
    }
}