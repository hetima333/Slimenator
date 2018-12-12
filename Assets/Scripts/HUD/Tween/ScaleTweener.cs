using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScaleTweener : TweenBase {

	// Use this for initialization
	void Start() {
		transform.DOScale(endValue, duration).SetLoops(loops, loopType);
	}
}