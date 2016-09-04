using UnityEngine;
using System.Collections;

public class SimulateOpen : MonoBehaviour {
	private GameObject simuObj;
    void Awake()
    {
        if (GameObject.FindGameObjectWithTag("simulateBtn"))
        {
            simuObj = GameObject.FindGameObjectWithTag("simulateBtn");
            if (GameObject.FindWithTag("GameController"))
            {
                bool state = GameObject.FindWithTag("GameController").GetComponent<SceneManager>().OpenSimlateBtn;
                simuObj.SetActive(state);
            }
        }
    }
}
