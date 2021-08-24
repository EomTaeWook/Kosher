using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Handy
{
    public static class ObjectPoolExtensions
    {
        public static void Recycle(this GameObject gameObject)
        {
            gameObject.transform.SetParent(HandyObjectPool.Instance.transform);
            gameObject.SetActive(false);
            HandyObjectPool.Instance.Push(gameObject);
        }
        public static void Recycle<T>(this Component component) where T : Component
        {
            component.transform.SetParent(HandyObjectPool.Instance.transform);
            component.gameObject.SetActive(false);
            HandyObjectPool.Instance.Push(component.gameObject);
        }
    }
}
