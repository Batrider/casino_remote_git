using UnityEngine;
using System.Collections;

public class InstaniateIcons : MonoBehaviour {

    public Transform[] fiveParents = new Transform[5];
    void Awake()
    {
        Object[] prefabss = GetComponent<StopController>().prefabss;
        for(int i = 0;i < fiveParents.Length;i++)
        {
            for(int j = 0;j < 4;j++)
            {
                GameObject icon = Instantiate(prefabss[(i*4+j)%15]) as GameObject;// ObjectPoolManager.Spawn(prefabss[(i*4+j)%15] as GameObject);
                icon.transform.parent = fiveParents[i];
                icon.transform.localPosition = new Vector3(-5,-150*j,0);
                icon.transform.localScale = Vector3.one;
            }
        }
    }





}
