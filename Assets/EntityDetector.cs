using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDetector : MonoBehaviour
{
    private float 
        f_gravitiyConstant = -9.8f;

    [SerializeField]
    private GameObject
            _Owner;

    [SerializeField]
    private PlayerStats
        _PlayerStats;

    [SerializeField]
    private EntityPlayer
        _Player;

    protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Suckable"))
        {
            Rigidbody RB = other.GetComponent<Rigidbody>();

            if (RB != null && RB.mass < _PlayerStats.SuckingPowerProperties)
                RB.AddForce(-GAcceleration(_Owner.transform.position, RB.mass, RB));

            if(Vector3.Distance(other.gameObject.transform.position, _Owner.gameObject.transform.position) < 1)
            {
                _Player.StoreElementInOrb(other.GetComponent<IElement>().GetElementType());
            }
        }
    }

    public Vector3 GAcceleration(Vector3 position, float mass, Rigidbody r)
    {
        Vector3 direction = position - r.position;

        //Realist GF with increasing force when getting closer to each other.
        //float gravityForce = f_gravitiyConstant * ((mass * r.mass * 1000) / direction.sqrMagnitude);
        //Simple GF with linear force.
        float gravityForce = f_gravitiyConstant * (mass * r.mass * 1000);
        gravityForce /= r.mass;
        //Debug.Log("gravityForce: " + gravityForce);

        return direction.normalized * gravityForce * Time.fixedDeltaTime;
    }
}
