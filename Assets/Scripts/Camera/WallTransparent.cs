using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壁(ボス部屋)の透明化
/// </summary>
public class WallTransparent : MonoBehaviour
{
    //レイを飛ばす対象オブジェクト
    private Ray _ray;
    //レイによって透明化させるオブジェクト
    private RaycastHit _rayHit;

    private GameObject _camera;
    private GameObject _player;
    private GameObject _wall;

    // Use this for initialization
    void Start()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        _player = GameObject.FindGameObjectWithTag("Player");
        _wall = GameObject.FindGameObjectWithTag("Wall");

    }

    // Update is called once per frame
    void Update()
    {
        //カメラからプレイヤーにレイを飛ばす
        _ray = new Ray(_camera.transform.position, -(_camera.transform.position - _player.transform.position));
        if (Physics.Raycast(_ray, out _rayHit))
        {
            //間に壁があったら
            if (_rayHit.collider.gameObject == _wall)
            {
                //壁を透明化する
                _wall.GetComponent<MeshRenderer>().material.color = new Color(0,0,0,0.5f);
            }
            else
            {
                //無ければ透明化しない
                _wall.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1.0f);
            }
        }
    }
}
