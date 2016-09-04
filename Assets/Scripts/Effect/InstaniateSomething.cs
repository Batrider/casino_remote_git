using UnityEngine;
using System.Collections;

public class InstaniateSomething : MonoBehaviour {
    public Vector3 localPosition;
    public Vector3 localScale;
    public string PrefabName;
    public GameObject parentObj;
    void Start()
    {
        StartCoroutine(Cyclemethod());
    }
    IEnumerator Cyclemethod()
    {

        while(true)
        {
            InstaniateMethod();
            yield return new WaitForSeconds(Random.Range(15,60));
            
        }
    }
    void InstaniateMethod()
    {
        GameObject so = Instantiate(Resources.Load(PrefabName)) as GameObject;
        so.transform.parent = parentObj.transform;
        so.transform.localPosition = this.localPosition;
        so.transform.localScale = this.localScale;
        so.transform.localEulerAngles = Vector3.zero;

    }
}
