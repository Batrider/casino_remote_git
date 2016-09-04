using UnityEngine;
using System.Collections;

public class SetLogoutClick : MonoBehaviour {
    GameObject gameController;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }
    void OnClick()
    {
        if(gameObject.name =="Setting")
            gameController.GetComponent<NewLogin>().SetScaleToOne(gameController.GetComponent<NewLogin>().SettingPanel);
        else
            gameController.GetComponent<NewLogin>().LoginOut();
    }
}
