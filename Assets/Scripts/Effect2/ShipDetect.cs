using UnityEngine;
using System.Collections;

public class ShipDetect : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other){
        GetComponent<UISprite>().spriteName = "L2";
    }
    void OnTriggerExit2D(Collider2D other){
        GetComponent<UISprite>().spriteName = "L1";
    }
}
