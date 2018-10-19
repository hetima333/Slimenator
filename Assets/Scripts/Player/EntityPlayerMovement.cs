using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayerMovement : MonoBehaviour
{

    [SerializeField]
    private Camera
        _Camera;

    [SerializeField]
    private PlayerStats
        _Player_Stats;

    [SerializeField]
    private EntityPlayer
        _EntityPlayer;

    private Ray
        _MouseRay;

    private RaycastHit
        _MouseRayCastHit;

    private void Start()
    {
        _MouseRayCastHit = new RaycastHit();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_EntityPlayer.GetPlayerState() == EnumHolder.States.MOVING)
            Move();
    }

    private void LateUpdate()
    {
        Rotate();
    }

    void Move()
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movement += _Camera.gameObject.transform.forward.normalized * _Player_Stats.SpeedProperties * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement -= _Camera.gameObject.transform.forward.normalized * _Player_Stats.SpeedProperties * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement -= _Camera.gameObject.transform.right.normalized * _Player_Stats.SpeedProperties * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement += _Camera.gameObject.transform.right.normalized * _Player_Stats.SpeedProperties * Time.deltaTime;
        }

        movement.y = 0;

        gameObject.transform.position += movement;
    }

    void Rotate()
    {
        _MouseRay = this._Camera.ScreenPointToRay(Input.mousePosition);

        int layer = (1 << LayerMask.NameToLayer("Ground"));

        if (Physics.Raycast(_MouseRay, out _MouseRayCastHit, Mathf.Infinity, layer))
        {
            Vector3 temp = new Vector3(_MouseRayCastHit.point.x, gameObject.transform.position.y, _MouseRayCastHit.point.z);

            gameObject.transform.LookAt(temp);
        }
    }
}
