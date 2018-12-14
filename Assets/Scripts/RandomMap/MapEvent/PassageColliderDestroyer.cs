using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassageColliderDestroyer : MonoBehaviour
{
    private BoxCollider _collidor; 

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
            //Destroy(collision.gameObject);
            collision.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            //Destroy(gameObject);

            gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("通過");
            GameStateManager.Instance.Map.GetComponent<NearBlockActivator>().Activate();
        }
    }
}
