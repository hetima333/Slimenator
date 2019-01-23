using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Stats")]
public class Stats : ScriptableObject {
	[SerializeField]
	private GameObject
	_Prefab;

	[SerializeField]
	private float
	_MaxHealth,
	_MaxHealthMultiplyer,
	_Health,

	_MaxDamage,
	_MaxDamageMultiplyer,

	_MaxSpeed,
	_MaxSpeedMultiplyer,

	_SuckingPower,
	_SuckingPowerMultiplyer,
	
	_SkillPower,
	_SkillPowerMultiplyer;

	[SerializeField]
	private bool
	_UsedByMultipleEntities = true;

	private bool _isInvincible = false;
	public bool IsInvincible {
		get {
			return _isInvincible;
		}
		set {
			_isInvincible = value;
		}
	}

	public float MaxHealthProperties {
		get {
			return _MaxHealth;
		}
	}

	public float HealthProperties {
		get {
			return _Health;
		}

		set {
			float old = _Health;
			_Health = value;

			if (_isInvincible && old > _Health) {
				Debug.Log("c : " + _Health + ", o : " + old);

				_Health = old;
			}
		}
	}

	public float DamageProperties {
		get {
			return _MaxDamage * _MaxDamageMultiplyer;
		}
	}

	public float SpeedProperties {
		get {
			return _MaxSpeed * _MaxSpeedMultiplyer;
		}
	}

	public float HealthMultiplyerProperties {
		get {
			return _MaxHealthMultiplyer;
		}

		set {
			_MaxHealthMultiplyer = Mathf.Clamp(value, 0, 5);
		}
	}

	public float DamageMultiplyerProperties {
		get {
			return _MaxDamageMultiplyer;
		}

		set {
			_MaxDamageMultiplyer = Mathf.Clamp(value, 0, 100);
		}
	}

	public float SpeedMultiplyerProperties {
		get {
			return _MaxSpeedMultiplyer;
		}

		set {
			_MaxSpeedMultiplyer = Mathf.Clamp(value, 0, 100);
		}
	}

	public float SuckingPowerProperties {
		get {
			return _SuckingPower * _SuckingPowerMultiplyer;
		}
	}

	public float SuckingPowerMultiplyerProperties {
		get {
			return _SuckingPowerMultiplyer;
		}

		set {
			_SuckingPowerMultiplyer = Mathf.Clamp(value, 0, 100);
		}
	}

	public float SkillPowerProperties {
		get {
			return _SkillPower;
		}
	}

	public float SkillPowerMultiplyerProperties {
		get {
			return _SkillPowerMultiplyer;
		}

		set {
			_SkillPowerMultiplyer = Mathf.Clamp(value, 0, 5);
		}
	}

	public bool IsUseByMultiple {
		get {
			return _UsedByMultipleEntities;
		}
	}

	public string GetPrefabName() {
		return _Prefab.name;
	}
}