using UnityEngine;
using System.Collections;

public class SystemGcCollect : MonoBehaviour {

    void Update()
    {
        if(Time.frameCount%50 == 0)
        {
            System.GC.Collect();
        }
    }
}
