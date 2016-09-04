using UnityEngine;
using System.Collections;

public class GameEffectLoad : MonoBehaviour {
    void Start()
    {
        GameObject gameEffect = Instantiate(Resources.Load("GameEffects")) as GameObject;
        gameEffect.transform.parent = transform;
        gameEffect.transform.localPosition = new Vector3(0,0,1000);
        gameEffect.transform.localScale = Vector3.one;
        gameEffect.transform.localEulerAngles = Vector3.zero;
    }

}
