using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog_Rest_Api.Repositories{
    public interface IBaseRepository
    {
          Task<List<T1>> GetAllAsync<T1>(int skip,int top);
    }
}