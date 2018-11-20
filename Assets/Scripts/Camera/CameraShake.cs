using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float
        _Intensity, 
        _MaxTremble;

    [SerializeField]
    [Range(0, 1)]
    private float
        _Magnitude;

    private float
        _Tremble;

    [SerializeField]
    private uint
        _RecoverSpeed;

    private Vector3
        _NewPosition;

	void Start ()
    {
        _NewPosition = Vector3.zero;
        _Tremble = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_NewPosition == gameObject.transform.localPosition)
            _NewPosition = new Vector3(Random.Range(-_Tremble, _Tremble), Random.Range(-_Tremble, _Tremble), Random.Range(-_Tremble, _Tremble));
        else
            gameObject.transform.localPosition = new Vector3(
                Mathf.Lerp(gameObject.transform.localPosition.x, _NewPosition.x, _Magnitude),
                Mathf.Lerp(gameObject.transform.localPosition.y, _NewPosition.y, _Magnitude),
                Mathf.Lerp(gameObject.transform.localPosition.z, _NewPosition.z, _Magnitude));

        if (_Tremble > 0)
            _Tremble -= Time.deltaTime * _RecoverSpeed;
        else
            _Tremble = 0;

    }

    public void ShakeCamera()
    {
        if (_Tremble < _MaxTremble)
            _Tremble += _Intensity;
    }
}
