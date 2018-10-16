using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject
        _SuckingParticle,
        _SuckingRadius;

    [SerializeField]
    private PlayerStats
        _Player_Stats;

    private Queue<ElementType>
        _OrbSlot = new Queue<ElementType>();

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!_SuckingParticle.activeSelf)
                _SuckingParticle.SetActive(true);

            if (!_SuckingRadius.activeSelf)
                _SuckingRadius.SetActive(true);

        }
        else
        {
            if (_SuckingParticle.activeSelf)
                _SuckingParticle.SetActive(false);

            if (_SuckingRadius.activeSelf)
                _SuckingRadius.SetActive(false);
        }

    }

    public void StoreElementInOrb(ElementType type)
    {
        _OrbSlot.Enqueue(type);

        if (_OrbSlot.Count > 3)
            _OrbSlot.Dequeue();
    }
}
