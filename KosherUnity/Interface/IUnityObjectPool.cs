using KosherUtils.ObjectPool.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KosherUnity.Interface
{
    public interface IUnityObjectPool<T> : IObjectPool<T>
    {
        GameObject GetObjectPool();
    }
}
