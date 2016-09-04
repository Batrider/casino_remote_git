using UnityEngine;
using System.Collections;

public class VersionSet : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<UILabel>().text = (VersionController.curVersion / 100) + "." + ((VersionController.curVersion / 10) % 10) + "." + (VersionController.curVersion % 10);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
