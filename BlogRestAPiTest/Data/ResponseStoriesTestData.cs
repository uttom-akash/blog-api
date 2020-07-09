using Blog_Rest_Api.DTOModels;
using System;
using System.Collections.Generic;
using System.Collections;

namespace BlogRestAPiTest.Data
{
    public class ResponseStoriesTestData : IEnumerable<object[]>
    {

        public IEnumerator<object[]> GetEnumerator()
        {
            ResponseStoryDTO story = new ResponseStoryDTO
            {
                StoryId = Guid.NewGuid(),
                Title = "Corona Virus",
                Body = "Corona virus is a huge threat for Mankind",
                PublishedDate = DateTime.UtcNow
            };

            yield return new object[] { story };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
