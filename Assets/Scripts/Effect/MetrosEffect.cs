using UnityEngine;
using System.Collections;

public class MetrosEffect : MonoBehaviour {
    public Object metroPrefabs;
    public GameObject parentObj;
    [SerializeField]
    private float timeDelta = 0;
    void Update()
    {
        timeDelta += Time.deltaTime;
        if (timeDelta > 1)
        {
            timeDelta = 0;
            MetroInit();
        }
    }
    void MetroInit()
    {
        float posX = Random.Range(-272, 1886);
        GameObject metro = Instantiate(metroPrefabs) as GameObject;
        metro.transform.parent = parentObj.transform;
        metro.transform.localScale = Vector3.one;
        metro.transform.localEulerAngles = new Vector3(0,0,40);
        metro.transform.localPosition = new Vector3(posX, 867, 0);
        TweenPosition tp = metro.AddComponent<TweenPosition>();
        tp.from = metro.transform.localPosition;
        tp.to = metro.transform.localPosition + new Vector3(-2000, -1500, 0);
        tp.delay = 0;
        tp.duration = 3f;
    }
}
