using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    //[SerializeField]
    private GameObject _go_target;   //Current camera's target

    [SerializeField]
    private Vector3 _offSet = new Vector3(0.0f, 70.0f, -20.0f);     //Offset of the camera

    [SerializeField]
    private float _smoothing = 5.0f;   //ラープの速さ

    //private Vector3
    //    _v3_target_position,        //Storing of Target's position as to not call the value from other script too many times
    //    _v3_camera_last_position;   //Storing of camera's last moved position    

    private void Start()
    {
        _go_target = GameObject.FindGameObjectWithTag("Player");
        transform.position = _go_target.transform.position + _offSet;

        //_offSet = transform.position - _go_target.transform.position;
        //_v3_target_position = _go_target.transform.position;  //Setting the target position to a Vector3 variable
        //transform.position = new Vector3(   
        //      _v3_target_position.x - (((_go_target.transform.forward).normalized).x * _offSet),
        //      _v3_target_position.y + _offSet,
        //      _v3_target_position.z - (((_go_target.transform.forward).normalized).z * _offSet));
    }

    void LateUpdate()
    {
        //transform.LookAt(_v3_target_position);   

        //_v3_camera_last_position = transform.position;   //Saving the Camera's last position into a Vector3 variable

        //if (_v3_target_position != _go_target.transform.position)
        //{
        //    transform.position -= _v3_target_position - _go_target.transform.position;

        //    _v3_target_position = _go_target.transform.position; //Moving the target as well as to ensure proper rotation
        //}

        //カメラの移動
        Move();

    }

    /// <summary>
    ///　カメラの移動
    /// </summary>
    private void Move()
    {
        //ターゲットに補間追尾する
        Vector3 currentPosition = _go_target.transform.position + _offSet;
        transform.position = Vector3.Lerp(transform.position, currentPosition, Time.deltaTime * _smoothing);
    }

}
