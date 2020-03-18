using System;
using System.Collections.Generic;
using System.Collections;
using Blog_Rest_Api.Persistent_Model;

namespace BlogRestAPiTest.Data
{
    public class StoriesTestData : IEnumerable<object[]>
    {

        public IEnumerator<object[]> GetEnumerator()
        {
            Story story = new Story
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
