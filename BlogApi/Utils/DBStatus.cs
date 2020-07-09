namespace Blog_Rest_Api.Utils
{
    public enum DBStatus
    {
        Added=0,
        Failed=1,
        NotFound=2,
        NotModified=3,
        Modified=4,
        NotDeleted=5,
        Deleted=6,
        Forbidden=7,
        Taken=8,
        PreconditionFailed = 10
    }
}