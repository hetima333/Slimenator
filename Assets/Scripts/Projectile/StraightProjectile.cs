﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProjectile : Projectile {
	[SerializeField]
	private StringList
	_TagList;

	public StringList TagList {
		get { return _TagList; }
	}

	public override void Init(Vector3 dir, float speed, ProjectileProperties projectile_properties, List<StatusEffect> Status, GameObjectList Targetable, float timer = 5, float damage = 1, float multiplyer = 1, float percentage = 0) {
		_Dir = dir;
		_speed = speed;
		_timer = timer * multiplyer;
		_damage = damage;
		_multiplyer = multiplyer;
		_percentage = percentage;
		_ProjectileProperties = projectile_properties;
		_PlayedCastingAudio = _PlayedEndingAudio = false;

		if (projectile_properties.GetMovingParticle() != null) {
			_MovingParticleCopy = Instantiate(projectile_properties.GetMovingParticle(), gameObject.transform);
		}

		if (projectile_properties.GetImpactParticle() != null) {
			ParticleInterface temp = projectile_properties.GetImpactParticle().GetComponent<ParticleInterface>();
			temp.Init();
			_impact_particle_timer = temp.GetLongestParticleEffect();
		}

		if (_Targetable.Count > 0)
			_Targetable.Clear();
		_Targetable.AddRange(Targetable.GetList());

		if (_StatusEffects.Count > 0)
			_StatusEffects.Clear();

		if (Status.Count > 0) {
			_StatusEffects.AddRange(Status);
		}
	}

	// Update is called once per frame
	protected override void Update() {
		base.Update();

		if (_timer > 0)
			_timer -= Time.deltaTime;

		gameObject.transform.position += _Dir.normalized * _speed * Time.deltaTime;

		if (_timer <= 0)
			Dead();
	}

	private void OnTriggerEnter(Collider collision) {
		if (!_TagList.IsTagNotHitable(collision.gameObject.tag))
			Dead();
	}
}