using UnityEngine;
using System.Collections;

public class BombAttack : MonoBehaviour {
    private GameObject monsterCamera;
    public Object InsPomObj;
    public Vector3 scaleOfEfect;
	// Use this for initialization
	void Start () {
        monsterCamera = GameObject.FindGameObjectWithTag("MonsterCamera");
	}
	
	// Update is called once per frame
	void Update () {
	  
        float distance = Vector3.Distance(transform.position, monsterCamera.transform.position);
        if(distance<0.5)
        {
            bombAttackEffect();
            Destroy(gameObject);
        }
        gameObject.transform.Rotate (360*Time.deltaTime,360*Time.deltaTime,0);
        transform.position = Vector3.MoveTowards(transform.position, monsterCamera.transform.position,2f*Time.deltaTime);
	}
    public void bombAttackEffect()
    {
        GameObject bombEffect=Instantiate(InsPomObj) as GameObject;
        bombEffect.transform.localPosition=transform.localPosition;
        bombEffect.transform.localEulerAngles=Vector3.zero;
        bombEffect.transform.localScale = scaleOfEfect;;
     //   PlayBombAttackSound();
    }
}
