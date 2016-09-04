using UnityEngine;
using System.Collections;

public class ShipAnimation : MonoBehaviour {
    public float speed;
    public Sg_Victory sv;
    void FixedUpdate()
    {
        transform.localPosition -= new Vector3(speed*Time.fixedDeltaTime,0,0);

    }
    IEnumerator Explosion()
    {
        GetComponent<UISpriteAnimation>().enabled = true;
        GetComponent<UISpriteAnimation>().ResetToBeginning();
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.name =="bomb")
        {
           StartCoroutine(Explosion());
        }
    }
    void OnClick()
    {
        sv.SelectBtnMessage(this.gameObject);
    }
}
