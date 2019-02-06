using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SlimeStock))]
public class SlimeStockPopper : MonoBehaviour {

    // 移動先座標
    [SerializeField]
    private float _showPos_x;
    [SerializeField]
    private float _hidePos_x;

    // シークエンス
    private Sequence _sequence;

    enum SequenceState
    {
        MOVE_RIGHT = 0,
        MOVE_LEFT,
        STAY_SHOW,
        STAY_HIDE,
        LOCKED_SHOW

    };

    SequenceState _state = SequenceState.STAY_HIDE;

    // 移動にかかる時間
    [SerializeField, Range(0.0f, 5.0f)]
    private float _duration = 0.75f;

    int _oldAmmount = 0;

    void Start()
    {
        var rectTrans = GetComponent<RectTransform>();

        var stock = GetComponent<SlimeStock>();

        // スライムを吸引したときの処理
        _sequence = DOTween.Sequence();
        _sequence.Append(rectTrans.DOAnchorPosX(_showPos_x, _duration));
        _sequence.AppendCallback(()=> { _state = SequenceState.STAY_SHOW; });
        _sequence.AppendInterval(_duration*3);
        _sequence.AppendCallback(() => { _state = SequenceState.MOVE_LEFT; });
        _sequence.Append(rectTrans.DOAnchorPosX(_hidePos_x, _duration));
        _sequence.AppendCallback(() => { _state = SequenceState.STAY_HIDE; });
        _sequence.SetAutoKill(false);

        _sequence.Pause();

        rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, rectTrans.anchoredPosition.y);

        // スライムが増減した時
        stock.ObserveEveryValueChanged(x => x.GetSlimeAmount())
            .Skip(1)
            .DistinctUntilChanged()
            .Subscribe(_stock => {


                // テキストコンポーネントを取得し、スライムの数を入れる
                foreach (var slime in stock.SlimeStockList)
                {
                    var image = transform.Find(slime.Key.name.ToString());
                    var text = image.Find("Num").GetComponent<Text>();
                    text.text = "× " + slime.Value.ToString();

                    _oldAmmount = stock.GetSlimeAmount();
                }

                // 隠れているときだけポップ表示ON
                if (_state == SequenceState.STAY_HIDE || _state == SequenceState.MOVE_LEFT)
                {
                    _sequence.Restart();
                    _state = SequenceState.MOVE_RIGHT;
                }

            });
    }

    private void Update()
    {
        // ポップの強制表示
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            SwithiPanel();
        }
    }

    public void SwithiPanel()
    {
        var rectTrans = GetComponent<RectTransform>();
        var subSequence = DOTween.Sequence();

        // 呼び出しで強制呼び出しモード()
        if ( _state == SequenceState.STAY_HIDE)
        {
            subSequence.Append(rectTrans.DOAnchorPosX(_showPos_x, _duration));
            subSequence.AppendCallback(() => { _state = SequenceState.LOCKED_SHOW; });
        }

        if (_state == SequenceState.LOCKED_SHOW)
        {
            subSequence.Append(rectTrans.DOAnchorPosX(_hidePos_x, _duration));
            subSequence.AppendCallback(() => { _state = SequenceState.STAY_HIDE; });
        }
    }
}
