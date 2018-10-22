using UnityEngine;

[CreateAssetMenu(menuName = "HUD/Create OrbData")]
public class OrbDefine : SingletonScriptableObject<OrbDefine>{
	[SerializeField]
	public Sprite fireSprite;
	[SerializeField]
	public Sprite waterSprite;
	[SerializeField]
	public Sprite thunderSprite;
}

public enum Orbs {
	NONE,
	FIRE,
	WATER,
	THUNDER
}
