using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColor : PropertyAttribute
{
    public float r;
    public float g;
    public float b;
    public float a;
    public BackgroundColor()
    {
        r = g = b = a = 1f;
    }
    public BackgroundColor(float aR, float aG, float aB, float aA)
    {
        r = aR;
        g = aG;
        b = aB;
        a = aA;
    }
    public Color color { get { return new Color(r, g, b, a); } }
}
