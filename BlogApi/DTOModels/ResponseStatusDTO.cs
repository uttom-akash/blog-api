using Blog_Rest_Api.Persistent_Model;

namespace Blog_Rest_Api.DTOModels{
    public class ResponseStatusDTO
    {
        public ResponseStatusDTO(int status,string message)
        {   
            Status=status;
            Message=message;
        }

        public int Status {get;set;}
        public string Message {get;set;} 
    }
}