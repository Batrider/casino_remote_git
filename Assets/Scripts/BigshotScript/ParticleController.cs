using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour {

    public int renderQueue = 30000;
    void Start()
    {
        if (renderer != null && renderer.sharedMaterial != null)  
        {
            renderer.sharedMaterial.renderQueue = renderQueue;  
        }
    }
}
