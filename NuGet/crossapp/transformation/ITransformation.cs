using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace crossapp.transformation
{
    public interface ITransformation<T>
    {
        Task Do(T obj);
    }
}
