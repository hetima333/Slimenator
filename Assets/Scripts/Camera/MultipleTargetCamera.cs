using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour {

    //カメラ本体
    [SerializeField] private Camera _camera;
    //カメラの親
    [SerializeField] private GameObject _camHolder;
    //カメラの範囲に入れるオブジェクト
    private List<Transform> _targets;

    [SerializeField] private Vector3 _offsetPos = new Vector3(0.0f, 70.0f, -70.0f);
    [SerializeField] private Vector3 _offsetRot = new Vector3(-30.0f, 0.0f, 0.0f);
    [SerializeField] private float _smoothTime = 0.5f;

    //範囲
    [SerializeField] private float _minZoom = 40;
    [SerializeField] private float _maxZoom = 10;
    [SerializeField] private float _zoomLimiter = 50;

    private Vector3 _velocity;

    private void Reset()
    {
        _camera = GetComponent<Camera>();
    }

    void Start()
    {
        _targets = new List<Transform>();
        //プレイヤーをターゲットに追加
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _targets.Add(player.transform);
        //ボス達をターゲットに追加  
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        //GameObject boss = GameObject.Find("KingSlime(Clone)");
        _targets.Add(boss.transform);

        //カメラの向きを設定
        _camHolder.transform.Rotate(_offsetRot);

    }

    private void LateUpdate()
    {
        //ターゲットが無ければ終了
        if (_targets.Count == 0) return;

        //カメラの移動
        Move();
        //ターゲットに合わせてズームする
        Zoom();
    }

    /// <summary>
    /// ターゲットに合わせて補間で拡大
    /// </summary>
    private void Zoom()
    {
        var newZoom = Mathf.Lerp(_maxZoom, _minZoom, GetGreatestDistance() / _zoomLimiter);
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, newZoom, Time.deltaTime);
    }

    /// <summary>
    /// カメラの移動
    /// </summary>
    private void Move()
    {
        var centerPoint = GetCenterPoint();
        var newPosition = centerPoint + _offsetPos;
        _camHolder.transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, _smoothTime);
    }

    /// <summary>
    /// 最大距離の取得
    /// </summary>
    /// <returns></returns>
    private float GetGreatestDistance()
    {
        var bounds = new Bounds(_targets[0].position, Vector3.zero);
        for (int i = 0; i < _targets.Count; i++)
        {
            bounds.Encapsulate(_targets[i].position);
        }
        return bounds.size.x;
    }

    /// <summary>
    /// 中心座標の取得
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCenterPoint()
    {
        //ターゲットが1体のみならそのターゲットが中心
        if (_targets.Count == 1) return _targets[0].position;

        var bounds = new Bounds(_targets[0].position, Vector3.zero);
        for (int i = 0; i < _targets.Count; i++)
        {
            bounds.Encapsulate(_targets[i].position);
        }
        return bounds.center;
    }

}
