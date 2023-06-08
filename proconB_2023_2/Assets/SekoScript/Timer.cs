using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // 経過時間を定義
    public static float time;
	//public GameObject startTime;

	// Use this for initialization
	void Start () {
        // 経過時間をリセット
		time = -3;
		// Invoke("Showtext",3);
	}

	// void Showtext(){
	// 	startTime.SetActive (true);
	// 	Debug.Log("comfirm");
	// }
	
	// Update is called once per frame
	void Update () {
        // Goalクラスのgoalがfalseなら時間を加算
		if (Goal.goal == false) {
			time += Time.deltaTime;
		}
        // 秒数に変換
		float t = Mathf.Floor (time * 10)/10;
        // textを取得して書き換え
		Text uiText = GetComponent<Text> ();
		if(t < 0){
			uiText.text = "Time : 0.0";
		}else{
			uiText.text = "Time : " + t;
		}
	}
}
