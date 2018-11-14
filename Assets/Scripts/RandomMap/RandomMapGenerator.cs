using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMapGenerator : MonoBehaviour
{

    //部屋の登録
    [SerializeField]
    private GameObject[] _rooms;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// 部屋を選ぶ
    /// </summary>
    private void ChoiceRoom()
    {
        //登録された部屋の種類を取得
        int count = _rooms.Length;

        //最初に生成する部屋をランダムに決定する
        //int firstRoom = RogueUtils

    }

}
