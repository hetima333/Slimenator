using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAutoDestroyer : MonoBehaviour {

    ParticleSystem _ps;

	// Use this for initialization
	void Start () {
        _ps = this.GetComponent<ParticleSystem>();

    }
	
	// Update is called once per frame
	void Update () {
		
        if(_ps.isEmitting == false)
        {
            StartCoroutine("Finish");
        }
	}

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(1.0f);

        Destroy(this.gameObject);
    }
}
