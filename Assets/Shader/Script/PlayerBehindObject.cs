using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehindObject : MonoBehaviour
{
    private Transform
        tr_position, 
        tr_camera_position;

    private Material
       mat_effect;

    private void Start()
    {
        tr_position = GameObject.FindGameObjectWithTag("Player").transform;
        tr_camera_position = GameObject.FindGameObjectWithTag("MainCamera").transform;
        mat_effect = gameObject.GetComponent<Renderer>().material;
    }

    void Update ()
    {
        mat_effect.SetVector("_PlayerPosition", tr_position.position);

        if(Vector3.Dot((gameObject.transform.position - tr_position.position).normalized, new Vector3(tr_camera_position.forward.x, gameObject.transform.position.y, tr_camera_position.forward.z).normalized) <= 0)
        {
            mat_effect.SetFloat("_ShouldRun", 1);          
        }
        else
        {
            mat_effect.SetFloat("_ShouldRun", 0);
        }
    }
}
