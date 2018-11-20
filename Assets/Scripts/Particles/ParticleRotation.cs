using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class Vector3Range
{
    [SerializeField]
    [Range(0, 1)]
    public int x, y, z;
}


public class ParticleRotation : MonoBehaviour {

    [SerializeField]
    Vector3Range _rotation;

    [SerializeField]
    [Range(0,360)]
    float _speed;
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(_rotation.x * Vector3.right * _speed * Time.deltaTime);
        gameObject.transform.Rotate(_rotation.y * Vector3.up * _speed * Time.deltaTime);
        gameObject.transform.Rotate(_rotation.z * Vector3.forward * _speed * Time.deltaTime);
    }
}
