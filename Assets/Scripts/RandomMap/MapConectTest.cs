using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConectTest : MonoBehaviour
{
    [SerializeField]
    private RandomMapGenerator _mapGen;
    private int _baseBlockInfo;  // 繋がれる側のブロックの情報
    private OneRoomInfo _nextBlock;	// つなぐ側のブロックの情報

    public enum BitDirection
    {
        BACK = 1,	// 0001
        RIGHT = 2,	// 0010
        FRONT = 4,	// 0100 手前
        LEFT = 8,	// 1000
    };

    Vector2Int _mapSize;

    // Use this for initialization

    private void Awake()
    {
        // 大きさを指定してマップを生成する
        if (Difficulty.Instance._difficulty == "NORMAL")
            _mapGen.MapResize(3, 3);
        else if(Difficulty.Instance._difficulty == "HARD")
            _mapGen.MapResize(6, 6);

        //最初の部屋を生成する
        _mapGen.ChoiceFirstRoom();

        _mapSize = _mapGen.GetSize();

        // マップの生成を実行
        GenerateMap(_mapGen._maps, _mapSize.y, _mapSize.x);
    }

    void Start()
    {
        // 
        _mapGen.InitPortal();
    }

    // ブロックオブジェクトのbitデータを時計回りに1回だけ90°回転させる
    public int RotateBitClockwise(int bitData)
    {
        int nextCorridor = (bitData << 1) % (15);

        // TODO：修正 1111はまわしたら0になります　
        if (nextCorridor == 0)
        {
            nextCorridor = 15;
        }
        return nextCorridor;
    }

    // ブロックオブジェクトのbitデータを時計回りに1回だけ180°回転(反転)させる
    public int ReveseBit(int bitData)
    {
        int nextCorridor = (bitData << 1) % (15);
        nextCorridor = (nextCorridor << 1) % (15);
        // TODO：修正 1111はまわしたら0になります　
        if (nextCorridor == 0)
        {
            nextCorridor = 15;
        }
        return nextCorridor;
    }


    //マップ生成実行
    public void GenerateMap(OneRoomInfo[,] infoList, int height, int width)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {

                // 最奥行でなければ奥のマスを確認してつなぐ
                if (i != 0 && infoList[j, i - 1] != null)
                {
                    ConectBlock(infoList[j, i - 1].passagePos, BitDirection.FRONT, new Vector2(j, i));
                    continue;
                }

                // 最左列でなければ右のマスを確認してつなぐ
                if (j != 0 && infoList[j - 1, i] != null)
                    ConectBlock(infoList[j - 1, i].passagePos, BitDirection.RIGHT, new Vector2(j, i));
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
        // 判定方向に道がない
        if ((info & (int)dir) == 0)
            return false;

        return true;
    }

    // 現在判定している通路に合うように回転させ,次に置くブロックを選択
    private void PutNextBlock(BitDirection dir, Vector2 position, int info)
    {
        int length = _mapGen.GetRoomInfos().Length;

        OneRoomInfo nextInfo = _mapGen.GetRoomInfos()[Random.Range(1, length)]; // プレハブリストからランダムに取得

        //通路データを取得
        _nextBlock = nextInfo;
        int nextCorridor = nextInfo.passagePos;

        // 3回転以内でつながる
        int i = 0;
        // TDOO: このループ気持ち悪いので今度直す
        while (i < 3)
        {
            // つながってなかったら
            if (((int)dir & nextCorridor) == 0
            // もしくは 右にも手前にも通路がなかったら回転する
            || ((nextCorridor & (int)BitDirection.RIGHT) == 0 && (nextCorridor & (int)BitDirection.FRONT) == 0))
            {
                // Bitデータを回転
                nextCorridor = RotateBitClockwise(nextCorridor);
                i++;
            }
            else
            {
                break;
            }
        }

        // オブジェクトの座標を計算
        Vector3 pos = new Vector3(position.x * _mapGen.ModelSize, 0, position.y * (-_mapGen.ModelSize));

        // オブジェクトの回転量を計算
        Quaternion rot = Quaternion.identity;
        rot = Quaternion.Euler(0.0f, 90.0f * i, 0.0f);

        // オブジェクトの生成
        var block = Instantiate(_nextBlock, pos, rot, gameObject.transform);

        // つくったオブジェクトを配列に格納
        block.passagePos = nextCorridor;
        _mapGen._maps[(int)position.x, (int)position.y] = block;
        _mapGen._rooms.Add(block);
    }

}