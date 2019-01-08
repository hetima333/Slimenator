using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearBlockActivator : MonoBehaviour {

    [SerializeField]
    private GameObject _player;

    private RandomMapGenerator _rmg;

    private int size;

	// Use this for initialization
	void Start () {
        _rmg = GetComponent<RandomMapGenerator>();

        Activate();
    }

    public void Activate()
    {
        // プレイヤーの現在位置を取得
        var pos = _player.transform.position;

        // プレイヤーの現在位置からどのマスにいるかを計算
        int px, py;
        px = (int)(pos.x + _rmg.ModelSize / 2 )/ _rmg.ModelSize ;
        py = (int)(-pos.z + _rmg.ModelSize / 2 )/ _rmg.ModelSize ;

        // 周囲1マスが配列の範囲内であればそのマスをアクティブ化

        // プレイヤーのマス
        int width = _rmg.GetSize().x;
        int height = _rmg.GetSize().y;
        Vector2Int gridPpos = new Vector2Int(px, py);

        // 一度すべてのマスを非アクティブ化
        Deactivate();

        // 最初のマスを強制アクティブ化
        _rmg._maps[0, 0].gameObject.SetActive(true);

        // 自分のいるマスをアクティブ化
        var playerGrid = _rmg._maps[gridPpos.x, gridPpos.y];
            playerGrid.gameObject.SetActive(true);

        // 調べる方向
        Vector2 vec = new Vector2(0, 1);
        // 調べるマス
        Vector2Int gridSpos = Vector2Int.zero;

        // 周囲8マスを調べる
        for (int i = 0; i < 8; i++) 
        {
            gridSpos.x = (int)Mathf.Round(gridPpos.x + vec.x);
            gridSpos.y = (int)Mathf.Round(gridPpos.y + vec.y);

            gridSpos.x = Mathf.Clamp(gridSpos.x, 0, width-1);
            gridSpos.y = Mathf.Clamp(gridSpos.y, 0, height-1);

            var searchGrid = _rmg._maps[gridSpos.x, gridSpos.y];
            if (searchGrid != null)
                searchGrid.gameObject.SetActive(true);

            vec = Quaternion.Euler(0, 0, 45) * vec;
            vec.x = Mathf.Round(vec.x);
            vec.y = Mathf.Round(vec.y);
        }
    }

    void Deactivate()
    {
        // 消す
        foreach (var block in _rmg._maps)
        {
            if (block != null && block.gameObject.activeSelf)
                block.gameObject.SetActive(false);
        }
    }
}
