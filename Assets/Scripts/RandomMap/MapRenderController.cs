using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderController : MonoBehaviour
{
    //カメラのタグ
    private const string CAMERA_TAG_NAME = "MainCamera";
    //カメラに映っているかどうか
    private bool _isRendered = false;

    // Use this for initialization
    void Start()
    {
        //カメラに映っていない
        _isRendered = false;
    }

    // Update is called once per frame
    void Update()
    {
        // カメラに映っているときのみ呼ばれる
        //OnWillRenderObject();

        if (_isRendered)
        {
            Debug.Log("TRUE");
            //gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("FALSE");
            //gameObject.SetActive(false);
        }

        //常に非表示
        _isRendered = false;

    }

    /// <summary>
    /// カメラに映っているときのみ呼ばれる
    /// </summary>
    private void OnWillRenderObject()
    {
        if (Camera.current.tag == CAMERA_TAG_NAME)
        {
            Debug.Log("camera");
            _isRendered = true;
        }

    }

    //private Renderer _renderer;

    //void Start()
    //{
    //    _renderer = GetComponent<Renderer>();
    //}

    //void Update()
    //{
    //    if(IsVisible())
    //    {
    //        Debug.Log("TRUE");
    //    }
    //    else
    //    {
    //        Debug.Log("FALSE");
    //    }
    //}

    //private bool IsVisible()
    //{
    //    return _renderer.isVisible;
    //}

}
