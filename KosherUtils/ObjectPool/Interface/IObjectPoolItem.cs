using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KosherUtils.ObjectPool.Interface
{
    public interface IObjectPoolItem
    {
        void Recycle();
    }
}
