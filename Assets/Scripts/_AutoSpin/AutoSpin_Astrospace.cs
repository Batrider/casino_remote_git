using UnityEngine;
using System.Collections;

public class AutoSpin_Astrospace : AutoSpinManager {
        public GameObject SpinButton;
        public GameObject timeObjBackground;
        public AudioClip boundOutClip;
        
        public void StartAutoSpin(GameObject objTime)
        {
            if(!GameObject.Find("/UI Root").GetComponent<manager>().IsNowMoving()&&!manager.isDraw)
            {
            /*
                for (int i = autoSpins.Length-1; i>=0; i--)
                {
                    autoSpins [i].GetComponent<BoxCollider>().enabled = false;
                    autoSpins[i].transform.localScale = Vector3.zero;
                }
                */
                GetComponent<UIPlayTween>().Play(false);
                manager.autoSpinTimess =int.Parse(objTime.name);
                GameObject.Find("/UI Root").GetComponent<manager>().reset();
            }
        } 
        
    }
