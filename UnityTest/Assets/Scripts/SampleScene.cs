using KosherUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScene : MonoBehaviour
{
    public TempUI TestObject;
    public TempUI1 TestObject1;
    public Transform Test;
    // Start is called before the first frame update

    private List<TempUI> items = new List<TempUI>();
    void Start()
    {
        for(int i=0;i<10; i++)
        {
            var obj = KosherUnityObjectPool.CallLocation<TempUI>(TestObject);
            obj.text.text = $"test{i}";
            items.Add(obj);
        }
        for (int i = 0; i < 10; i++)
        {
            items[i].Recall<TempUI>();
        }
        items.Clear();

        for (int i = 0; i < 5; i++)
        {
            var obj = KosherUnityObjectPool.CallLocation<TempUI>(TestObject, Test);

            obj.text.text = $"test{i + 10 }";
            items.Add(obj);
        }
        //for(int i=0; i<items.Count; ++i)
        //{
        //    items[i].Recycle();
        //}

        var temp1 = GameObject.Instantiate(TestObject1, Test);
        temp1.Recall<TempUI1>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
