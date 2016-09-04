using UnityEngine;
using System.Collections;

public class Ellipse : MonoBehaviour {
    float a;
    float b;

    public Ellipse(float a,float b)
    {
        this.a = a;
        this.b = b;
    }

    public Vector2 XYPoint(float angle)
    {
        Vector2 ellipsePoint = new Vector2();
        ellipsePoint.x = a * Mathf.Sin(angle);
        ellipsePoint.y = b * Mathf.Cos(angle);
        return ellipsePoint;
    }
}
