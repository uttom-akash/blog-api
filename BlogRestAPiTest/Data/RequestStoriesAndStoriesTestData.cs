using Blog_Rest_Api.DTOModels;
using System;
using System.Collections.Generic;
using System.Collections;
using Blog_Rest_Api.Persistent_Model;

namespace BlogRestAPiTest.Data
{
    public class RequestStoriesAndStoriesTestData : IEnumerable<object[]>
    {

        public IEnumerator<object[]> GetEnumerator()
        {
            Guid storyId = Guid.NewGuid();
            RequestStoryDTO requestStoryDTO = new RequestStoryDTO
            {
                StoryId = storyId,
                Title = "Corona Virus",
                Body = "Corona virus is a huge threat for Mankind",
                PublishedDate = DateTime.UtcNow
            };

            Story story = new Story
            {
                StoryId = storyId,
                Title = "Corona Virus",
                Body = "Corona virus is a huge threat for Mankind",
                PublishedDate = DateTime.UtcNow,
                AuthorId = "akash"
            };

            yield return new object[] { requestStoryDTO, story };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
