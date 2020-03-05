namespace Blog_Rest_Api.DTOModels{
    public class LoggedInUserDTO :UserInfoDTO{
        public string JwtToken {get;set;}
    }
}