using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConectTest : MonoBehaviour
{
    private const int BLOCK_SIZE = 80;

    private int _baseBlockInfo;  // 繋がれる側のブロックの情報
    private GameObject _nextBlock;	// つなぐ側のブロックの情報

    public enum BitDirection
    {
        BACK = 1,	// 0001
        RIGHT = 2,	// 0010
        FRONT = 4,	// 0100
        LEFT = 8,	// 1000
    };

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ブロックオブジェクトのbitデータを時計回りに1回だけ90°回転させる
    public int RotateBitClockwise(int bitData)
    {
        int nextCorridor = (bitData << 1) % (15);
        return nextCorridor;
    }

    // ブロックオブジェクトのbitデータを時計回りに1回だけ180°回転(反転)させる
    public int ReveseBit(int bitData)
    {
        int nextCorridor = (bitData << 1) % (15);
        nextCorridor = (nextCorridor << 1) % (15);
        return nextCorridor;
    }


    //マップ生成実行
    public void GenerateMap(int[][] list, int height, int width)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                // 最奥行でなければ奥のマスを確認してつなぐ
                if (i != 0)
                {
                    ConectBlock(list[j][i - 1], BitDirection.BACK, new Vector2(j, i));
                }

                // 最左列でなければ左のマスを確認してつなぐ
                if (i != 0)
                {
                    ConectBlock(list[j - 1][i], BitDirection.LEFT, new Vector2(j, i));
                }
            }
        }
    }


    private void ConectBlock(int info, BitDirection dir, Vector2 pos)
    {
        //マスを確認して、つなげたらつなぐ
        if (CheckConect(dir, info) == true)
            PutNextBlock((BitDirection)ReveseBit((int)dir), pos, info);
    }

    // ブロックの通路を調べ、つなげるかどうかを判定
    private bool CheckConect(BitDirection dir, int info)
    {
        // そもそもこのますにオブジェクトがない
        if (info == 0)
            return false;

        // 判定方向に道がない
        if ((info & (int)dir) == 0)
            return false;

        return true;
    }

    // 現在判定している通路に合うように回転させ,次に置くブロックを選択
    private void PutNextBlock(BitDirection dir, Vector2 position, int info)
    {
        _nextBlock = new GameObject();  			// TODO: =>プレハブリストからランダムに取得
        int nextCorridor = _nextBlock.GetCorridor();// TODO: 通路データを取得すればおｋ

        // 3回転以内でつながる
        int i = 0;
        for (i = 0; i < 3; i++)
        {
            // Bitデータを回転
            nextCorridor = RotateBitClockwise(nextCorridor);

            // つながったら && 右or手前に通路があったら回転やめる
            if (((int)dir & info & ReveseBit(nextCorridor)) != 0 &&
            (nextCorridor & (int)BitDirection.RIGHT) != 0 || (nextCorridor & (int)BitDirection.FRONT) != 0)
                break;
        }
        // オブジェクトの座標を計算
        Vector3 pos = new Vector3(position.x * BLOCK_SIZE, 0, position.y * BLOCK_SIZE);

        // オブジェクトの回転量を計算
        Quaternion rot = Quaternion.identity;
        rot = Quaternion.Euler(0.0f, 90.0f * (i + 1), 0.0f);

        // オブジェクトの生成
        Instantiate(_nextBlock, pos, rot);

        // つくったオブジェクトを配列に格納
    }
}