using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : SingletonMonoBehaviour<SlimeManager> {

    [SerializeField]
    private GameObject
        _prefab;

    private void Start()
    {
    }

    private void Update()
    {
        //Temporary
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ObjectManager.Instance.InstantiateWithObjectPooling(_prefab, transform.position, transform.rotation).GetComponent<EnvironmentBase>();
        }
    }
}
