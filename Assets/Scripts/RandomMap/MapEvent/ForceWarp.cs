using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceWarp : MonoBehaviour
{

    public GameObject _target;
    public GameObject _Player;

    private GameObject _Boss;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.O))
        {
            Warp();
        }
    }


    public void Warp()
    {
        _Player.transform.position = _target.transform.position;
        _Boss = GameObject.Find("KingSlime(Clone)");
        _Boss.GetComponent<TestBoss>().WakeUp();

        GameStateManager.Instance.DestroyMap();
    }
}

