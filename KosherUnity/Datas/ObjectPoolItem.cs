using KosherUtils.ObjectPool.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KosherUnity.Datas
{
    public class ObjectPoolItem : MonoBehaviour, IObjectPoolItem
    {
        private IObjectPool<Component> objectPool;
        public void Init(IObjectPool<Component> objectPool)
        {
            this.objectPool = objectPool;
        }
        public virtual void Recycle()
        {
            this.objectPool.Push(this);
        }
    }
}
