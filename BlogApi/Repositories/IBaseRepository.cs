using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog_Rest_Api.Repositories{
    public interface IBaseRepository
    {
        Task<List<T>> GetAllAsync<T>(int skip,int top);
    }
}