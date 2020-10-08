using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace crossapp.action
{
    public interface IDeleteAction<T>  where T : class
    {
        Action Create(T obj);
    }
}
