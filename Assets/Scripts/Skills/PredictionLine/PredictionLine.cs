using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionLine : MonoBehaviour
{

    private LineRenderer _lr;

    // Use this for initialization
    void Start () {
        _lr = GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        CalcRange();
    }

    private void CalcRange()
    {
        // レイの発射方向を決定
        Vector3 rayDirection = gameObject.transform.forward;

        // 向いている方向にレイを発射
        RaycastHit hitInfo;
        Physics.Raycast(gameObject.transform.position,
            rayDirection * 100, out hitInfo);

        // レイの衝突位置を検出
        Vector3 hitPos = hitInfo.point;

        //　レイの長さをLineRendererの長さに適応
        float rayRange = (hitPos - gameObject.transform.position).magnitude;
        Vector3 distination = new Vector3(0,0,rayRange);
        _lr.SetPosition(1, distination);
    }


}
    