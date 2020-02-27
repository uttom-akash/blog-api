namespace Blog_Rest_Api{
    public class JwtInfo{
        public string ValidIssuer {get;set;}
        public string ValidAudience {get;set;}
        public string Key {get;set;}
        public int expires {get;set;}
    }
}