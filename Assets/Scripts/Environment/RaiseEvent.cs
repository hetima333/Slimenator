using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseEvent : MonoBehaviour {

    GameObject _parentObject;

    private void Start()
    {
        _parentObject = transform.parent.gameObject;
    }

    public void RaiseEffect()
    {
        _parentObject.GetComponent<SlimeBase>().SpawnTrail();
    }
}
