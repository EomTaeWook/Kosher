using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Handy;

public class SampleScene : MonoBehaviour
{
    public TempUI TestObject;
    public Transform Test;
    // Start is called before the first frame update

    private List<TempUI> items = new List<TempUI>();
    void Start()
    {
        for(int i=0;i<10; i++)
        {
            var obj = HandyObjectPool.Spawn<TempUI>(TestObject);
            obj.text.text = $"test{i}";
            items.Add(obj);
        }
        for (int i = 0; i < 10; i++)
        {
            items[i].Recycle<TempUI>();
        }
        items.Clear();

        for (int i = 0; i < 5; i++)
        {
            var obj = HandyObjectPool.Spawn<TempUI>(TestObject, Test);

            obj.text.text = $"test{i + 10 }";
            items.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
