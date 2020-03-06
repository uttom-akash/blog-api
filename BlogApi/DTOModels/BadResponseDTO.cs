using System;
using System.Collections.Generic;

namespace Blog_Rest_Api.DTOModels{
    public class BadResponseDTO{

        public string Type {get;set;}
        public string Title {get;set;}
        public int Status {get;set;}
        public string TraceId {get;set;}
        public Errors Errors {get;set;}
    }

    public class Errors
    {
        public  List<string> Message {get;set;}        
    }
}