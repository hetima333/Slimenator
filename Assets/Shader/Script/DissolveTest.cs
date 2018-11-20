using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveTest : MonoBehaviour
{
    [SerializeField]
    [Range(0, 2)]
    private float
       f_dissolveChangingSpeed;

    private Material
       mat_effect;

    private float
      f_dissolveValue;

    private bool
        b_switch;

    private void Start()
    {
        f_dissolveValue = 0;
        b_switch = false;

        mat_effect = gameObject.GetComponent<Renderer>().material;
    }

    void Update ()
    {
        if (f_dissolveValue >= 1)
            b_switch = true;
        else if (f_dissolveValue <= 0)
            b_switch = false;

        f_dissolveValue += f_dissolveChangingSpeed * ((b_switch) ? -Time.deltaTime : Time.deltaTime);
        mat_effect.SetFloat("_DissolveAmount", f_dissolveValue);
        mat_effect.SetFloat("_DissolveSize", f_dissolveValue / 10);
    }
}
