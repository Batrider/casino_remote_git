using UnityEngine;
using System.Collections;

public class MoveLights : MonoBehaviour {
    public Sprite[] sprites;
    public float timeDelta = 0.02f;
    float time = 0;
    int i = 0;
	void Update () {
        time += Time.deltaTime;
        if (time > timeDelta)
        {
            time = 0;
            GetComponent<UI2DSprite>().sprite2D = sprites[i];
            i++;
            if(i==sprites.Length)
            {
                i = 0;
            }
        }
	}
}
