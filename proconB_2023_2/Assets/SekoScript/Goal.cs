using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    // goalフラグ
	public static bool goal;

	// Use this for initialization
	void Start () {
		goal = false;
	}
	// playerがゴールに入ったらgoalフラグをtrue
	void OnTriggerEnter( Collider col ) {
		if (col.gameObject.tag == "Player") { 
			goal = true;
		}
	}
}
