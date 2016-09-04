using UnityEngine;
using System.Collections;

public class FollowColor : MonoBehaviour {

    private UISprite parentSprite;
    private UISprite ownSprite;
    void Start()
    {
        parentSprite = transform.parent.GetComponent<UISprite>();
        ownSprite = GetComponent<UISprite>();
    }

    void Update()
    {
        ownSprite.color = parentSprite.color;
    }

}
