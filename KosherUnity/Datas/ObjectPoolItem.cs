using KosherUnity.Interface;
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
        private IUnityObjectPool<Component> objectPool;
        public void Init(IUnityObjectPool<Component> objectPool)
        {
            this.objectPool = objectPool;
        }
        public virtual void Recycle()
        {
            this.transform.SetParent(this.objectPool.GetObjectPool().transform);
            this.objectPool.Push(this);
        }
    }
}
