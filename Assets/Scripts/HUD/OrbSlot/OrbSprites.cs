using UnityEngine;

[CreateAssetMenu(menuName = "HUD/Create OrbSprites")]
public class OrbSprites : ScriptableObject{
	[SerializeField]
	private Sprite _fire;
	public Sprite Fire {
		get { return _fire; }
	}

	[SerializeField]
	private Sprite _ice;
	public Sprite Ice {
		get { return _ice; }
	}

	[SerializeField]
	private Sprite _lightning;
	public Sprite Lightning {
		get { return _lightning; }
	}
}

public enum Orbs {
	NONE = -1,
	FIRE = 0,
	ICE,
	LIGHTNING
}