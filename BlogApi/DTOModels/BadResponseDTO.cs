using System.Collections.Generic;

namespace Blog_Rest_Api.DTOModels{
    public class BadResponseDTO{

        public string Type {get;set;}
        public string Title {get;set;}
        public int Status {get;set;}
        public string TraceId {get;set;}
        public object Errors {get;set;}
    }
}