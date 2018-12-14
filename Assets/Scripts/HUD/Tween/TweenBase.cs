using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TweenBase : MonoBehaviour {
	// 終了値
	[SerializeField, Range(0.0f, 10.0f)]
	protected float endValue = 1.5f;

	// 所要時間
	[SerializeField, Range(0.0f, 10.0f)]
	protected float duration = 1.0f;

	// ループ回数
	[SerializeField]
	protected int loops = -1;

	// ループタイプ
	[SerializeField]
	protected LoopType loopType = LoopType.Yoyo;
}