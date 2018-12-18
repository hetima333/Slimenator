using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassageColliderDestroyer : MonoBehaviour
{
    private BoxCollider _collidor;
    private bool _isPassed = true;

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
            // 相手をTrigger化させる
            //Destroy(collision.gameObject);
            collision.gameObject.GetComponent<BoxCollider>().isTrigger = true;

            var aX = gameObject.transform.position.x;
            var aZ = gameObject.transform.position.z;
            var bX = collision.gameObject.transform.position.x;
            var bZ = collision.gameObject.transform.position.z;

            GameObject DestObj;
            if (aX == bX)
            {
                DestObj = aZ <= bZ ? gameObject : collision.gameObject;
            }
            else
            {
                 DestObj = aX <= bX ? gameObject : collision.gameObject;
            }
            Destroy(DestObj);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameStateManager.Instance.Map.GetComponent<NearBlockActivator>().Activate();
        }
    }


}
