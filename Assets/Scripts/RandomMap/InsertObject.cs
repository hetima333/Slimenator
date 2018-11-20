using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーや敵、スライムなどのオブジェクトをマップ上に挿入する
/// </summary>
public class InsertObject : MonoBehaviour {

	//プレイヤー
	private GameObject _player;

    //スライム
    //[SerializeField] private AddObject[] _slimes;

    //敵
    //[SerializeField] private AddObject[] _enemys;

    //マップ情報
    private RandomMapGenerator _mapGen; 

    // Use this for initialization
    void Start () {
        //プレイヤーの取得
        _player = GameObject.FindGameObjectWithTag("Player");
        //マップ情報取得
        _mapGen = GetComponent<RandomMapGenerator>();

        //プレイヤーの座標を設定
        SetPlayerPosition();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// プレイヤーの座標を設定する
    /// </summary>
	public void SetPlayerPosition()
	{
        //テスト：プレイヤー
        if (!_player)
            return;

        //最初に作られた部屋にプレイヤーのみ生成する
        var firstRoomPos = GetFirstRoom();

        //プレイヤーの位置を設定
        _player.transform.position = new Vector3(firstRoomPos.x * _mapGen.GetSize().x, 2, firstRoomPos.y * _mapGen.GetSize().y);
        //Debug.Log(_player.transform.position);
    }

    /// <summary>
    /// 最初に作られた部屋の取得
    /// </summary>
    /// <returns>幅と奥行きのvector2(int)</returns>
    public Vector2Int GetFirstRoom()
    {
        for (int z = 0; z < _mapGen.GetSize().y; z++)
        {
            for (int x = 0; x < _mapGen.GetSize().x; x++)
            {
                if (_mapGen._maps[x, z] != null)
                {
                    //Debug.Log(new Vector2(x, z) + "!!!!");
                    return new Vector2Int(x, z);
                }
            }
        }

        //部屋が無い場合
        Debug.Log("Not room");
        return new Vector2Int(0,0);
    }


}
