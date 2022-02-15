using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KosherUtils.ObjectPool.Interface
{
    public interface IObjectPool
    {
        void Push<T>(T item) where T : IObjectPoolItem;
    }
}
