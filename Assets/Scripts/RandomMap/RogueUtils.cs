using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueUtils {

    public static int GetRandomInt(int min,int max)
    {
        return min + Mathf.FloorToInt(Random.value * (max - min + 1));
    }

    public static bool RandomJadge(float rate)
    {
        return Random.value < rate;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
