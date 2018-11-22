using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurringTest : MonoBehaviour
{


    private MonoBehaviour[] _behaviors;
    private Collider[] _colliders;
    private Rigidbody _rbd;
    private MeshRenderer _renderer;

    // Use this for initialization
    void Start()
    {
        // 各コンポーネントを取得
        _behaviors = GetComponents<MonoBehaviour>();
        _colliders = GetComponents<Collider>();
        _rbd = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnWillRenderObject()
    {

#if UNITY_EDITOR

        if (Camera.current.name != "SceneCamera" && Camera.current.name != "Preview Camera")

#endif
        {
            AllComponentsEnable();
        }
        else
        {
            AllComponentsDisable();
        }
    }

    void AllComponentsEnable()
    {
        // このスクリプト以外のコンポーネントを有効化
        foreach (MonoBehaviour cmp in _behaviors)
        {
            if (cmp.GetType().Name != this.GetType().Name)
                cmp.enabled = true;
        }

        // コライダー、リジッドボディー、メッシュレンダラーを有効化
        if (_colliders.Length > 0)
            foreach (Collider col in _colliders)
        {
            col.enabled = true;
        }
        if (_rbd != null)
            _rbd.WakeUp();
        //if (_renderer != null)
        //    _renderer.enabled = true;
    }


    void AllComponentsDisable()
    {
        // このスクリプト以外のコンポーネントを無効化
        foreach (MonoBehaviour cmp in _behaviors)
        {
            if (cmp.GetType().Name != this.GetType().Name)
                cmp.enabled = false;
        }

        // コライダー、リジッドボディー、メッシュレンダラーを無効化
        if(_colliders.Length > 0)
            foreach (Collider col in _colliders)
            {
               col.enabled = false;
            }
        if(_rbd != null)
            _rbd.Sleep();
        //if (_renderer != null)
        //    _renderer.enabled = false;
    }
}
