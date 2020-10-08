using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace crossapp.action
{
    public interface IUpdateAction<T>   where T : class
    {
        Action Create(T obj);
    }
}
