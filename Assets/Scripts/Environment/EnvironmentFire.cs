using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentFire : EnvironmentBase {
    private ParticleSystem _pSystem;
    private ParticleSystem.MainModule _pSystemMain;
    private ParticleInterface _pInterface;

    [SerializeField]
    private GameEvent _burnEvent;

    protected override void Awake() {
        _pSystem = GetComponentInChildren<ParticleSystem>();
        _pInterface = GetComponent<ParticleInterface>();
        _pSystemMain = _pSystem.main;
    }

    // Use this for initialization
    protected override void Start () {
        _pInterface.Init();
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}

    public override void InitObjectWithLife(float lifetime, Vector3 pos, Vector3 size, bool isStatic = true)
    {
        _pSystem.Stop();
        base.InitObjectWithLife(lifetime, pos, size);
        _pInterface.SetParticleEffectSize(0.5f);
        _pSystemMain.duration = lifetime * 0.5f;
        _pSystem.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SlimeBase>() != null)
            return;
        if (other.GetComponent<IDamageable>() != null)
            _burnEvent.InvokeSpecificListner(other.gameObject.GetInstanceID());
    }
}
