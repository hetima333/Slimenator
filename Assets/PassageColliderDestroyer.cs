using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassageColliderDestroyer : MonoBehaviour
{
    private BoxCollider[] _colliderList;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PassageCollidor")
        {
            // お前を殺して俺も死ぬ
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "PassageCollidor")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
