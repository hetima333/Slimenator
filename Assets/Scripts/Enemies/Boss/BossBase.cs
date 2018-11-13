﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour {

	protected const float ACT_INTERVAL = 1.0f;

	protected float _actInterval = ACT_INTERVAL;

	public float _maxHp;
	public float _hp;
	//インタフェース用最大Hp取得
	public float MaxHitPoint { get { return _maxHp; } }
	//インタフェース用現在Hp取得
	public float HitPoint { get { return _hp; } }

	public GameObject _target;

	public bool _isAction = false;

	protected int _phase = 0;

	//アニメーション
	public SimpleAnimation _anim;

	//Object for long range attack
	public GameObject _slimeBullet;

	public BossSkill _previousSkill;

	[SerializeField]
	//発動できるスキルリスト
	public List<BossSkill> _skillList = new List<BossSkill> ();

	public List<BossSkill> _canUseSkillList = new List<BossSkill> ();

}