using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayerMovement : MonoBehaviour
{

    [SerializeField]
    private Camera
        _Camera;

    [SerializeField]
    private EntityPlayer
        _EntityPlayer;

    private Ray
        _MouseRay;

    private RaycastHit
        _MouseRayCastHit;

    private bool
        _IsUsingMouse;

    private Vector3
        _LastMousePosition;

    private void Start()
    {
        _MouseRayCastHit = new RaycastHit();
        _IsUsingMouse true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_EntityPlayer.GetPlayerState() != EnumHolder.States.DIE &&
            !(_EntityPlayer.GetPlayerState() == EnumHolder.States.CASTING && _EntityPlayer.RestrictMovement))
            Move();
    }

    private void LateUpdate()
    {
        if (_EntityPlayer.GetPlayerState() != EnumHolder.States.DIE && _EntityPlayer.Speed > 0)
            Rotate();
    }

    void Move()
    {
        Vector3 movement = Vector3.zero;

        if (InputManager.LS_Joystick() != Vector3.zero)
        {
            movement += Vector3.Scale(_Camera.gameObject.transform.forward.normalized, InputManager.LS_Joystick()) * _EntityPlayer.Speed * Time.deltaTime;
            movement += Vector3.Scale(_Camera.gameObject.transform.right.normalized, InputManager.LS_Joystick()) * _EntityPlayer.Speed * Time.deltaTime;
        }

        //if (InputManager.RS_Horizontal() > 0)
        //{
        //    movement += _Camera.gameObject.transform.forward.normalized * _EntityPlayer.GetPlayerStats().SpeedProperties * Time.deltaTime;
        //}
        //else if (InputManager.RS_Horizontal() < 0)
        //{
        //    movement -= _Camera.gameObject.transform.forward.normalized * _EntityPlayer.GetPlayerStats().SpeedProperties * Time.deltaTime;
        //}

        //if (InputManager.RS_Vertical() > 0)
        //{
        //    movement -= _Camera.gameObject.transform.right.normalized * _EntityPlayer.GetPlayerStats().SpeedProperties * Time.deltaTime;
        //}
        //else if (InputManager.RS_Vertical() < 0)
        //{
        //    movement += _Camera.gameObject.transform.right.normalized * _EntityPlayer.GetPlayerStats().SpeedProperties * Time.deltaTime;
        //}

        //movement.y = 0;

        gameObject.transform.position += movement;
    }

    void Rotate()
    {
        if (!_IsUsingMouse)
        {
            if (Input.mousePosition != _LastMousePosition)
            {
                _IsUsingMouse = true;
                Cursor.visible = true;
                return;
            }

            if (InputManager.RS_Joystick() != Vector3.zero)
            {
                _MouseRay = this._Camera.ScreenPointToRay(InputManager.RS_Joystick());
                Vector3 direction = Vector3.right * InputManager.RS_Joystick().x + Vector3.forward * InputManager.RS_Joystick().z;
                if (direction.sqrMagnitude > 0)
                {
                    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                }
            }
        }
        else
        {
            if (InputManager.RS_Joystick() != Vector3.zero)
            {
                _IsUsingMouse = false;
                Cursor.visible = false;
                _LastMousePosition = Input.mousePosition;
                return;
            }

            _MouseRay = this._Camera.ScreenPointToRay(Input.mousePosition);
            int layer = (1 << LayerMask.NameToLayer("Ground"));
            if (Physics.Raycast(_MouseRay, out _MouseRayCastHit, Mathf.Infinity, layer))
            {
                Vector3 temp = new Vector3(_MouseRayCastHit.point.x, gameObject.transform.position.y, _MouseRayCastHit.point.z);

                gameObject.transform.LookAt(temp);
            }
        }
    }
}
