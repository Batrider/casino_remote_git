using UnityEngine;
using System.Collections;

public class AESgLerpToTarget : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    private Vector3 startPos;
    public float coefficient = 1;
    private float coeYOffset = 0;
    private float timeSum;
    private float distance = 0f;
    private bool arrived1;
    private bool arrived2;

    void Start()
    {
        arrived1 = true;
        arrived2 = true;
        startPos = transform.position;
        timeSum = 0;
        distance = Vector3.Distance(startPos, target1.transform.position);
    }

    void FixedUpdate()
    {
        if (!arrived2)
        {
            if (arrived1)
            {
                transform.position = Vector3.Lerp(transform.position, target2.transform.position, 2 * Time.fixedDeltaTime);
                if (Vector3.Distance(transform.position, target2.transform.position) < 0.01f)
                {
                    TweenScale.Begin(gameObject,0.3f,Vector3.zero);
                    StartCoroutine(setDeaultVale());
                    arrived2 = true;
                }
            } else
            {
                timeSum += coefficient * Time.fixedDeltaTime;
                coeYOffset = Mathf.PI * Mathf.Abs(1 - timeSum);
                transform.position = Vector3.Lerp(startPos, target1.transform.position + 0.1f * Vector3.up, timeSum) + new Vector3(0, Mathf.Sin(coeYOffset) * 0.15f * distance);
                if (Vector3.Distance(transform.position, target1.transform.position) < 0.1f)
                {
                    arrived1 = true;
                }
            }
        }
    }
    public void startMove()
    {
        arrived1 = false;
        arrived2 = false;
    }
    IEnumerator setDeaultVale()
    {
        yield return new WaitForSeconds(2f);
        transform.position = startPos;
        timeSum = 0;
        distance = Vector3.Distance(startPos, target1.transform.position);
        TweenScale.Begin(gameObject,1f,Vector3.one);
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<TweenPosition>().enabled = true;

    }
}
