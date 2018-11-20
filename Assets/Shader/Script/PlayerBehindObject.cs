using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehindObject : MonoBehaviour
{
    private Transform
        tr_position;

    private Material
       mat_effect;

    private void Start()
    {
        tr_position = GameObject.FindGameObjectWithTag("Player").transform;
        mat_effect = gameObject.GetComponent<Renderer>().material;
    }

    void Update ()
    {
        mat_effect.SetVector("_PlayerPosition", tr_position.position);
    }
}
