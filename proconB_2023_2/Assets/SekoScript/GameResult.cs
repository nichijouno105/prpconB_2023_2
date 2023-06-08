using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ライブラリ読み込み
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameResult : MonoBehaviour {
    // ハイスコア保存用
	private float highScore;
	
	// ゴール表示
    // テキスト描画用
	public Text resultTime;
	public Text bestTime;
	public GameObject resultUI;

	// Use this for initialization
	void Start () {
        // PlayerPrefs.HasKeyでデータのセーブ
        // HighScoreに値があれば、highScoreを保存
		if (PlayerPrefs.HasKey ("HighScore")) {
			highScore = PlayerPrefs.GetInt ("HighScore");
		} else { // なければ999
			highScore = 999;
		}
	}
	
	// Update is called once per frame
	void Update () {
        // goalフラグがtrueのとき
		if (Goal.goal) {
            // 結果を表示させる
			resultUI.SetActive (true);
            // 今回の時間とハイスコアを設定
			float result = Mathf.Floor (Timer.time * 10)/10;
			resultTime.text = "ResultTime:" + result;
			bestTime.text = "BestTime:" + highScore;
            // 今回が良ければハイスコアを書き換える
			if (highScore > result) { 
				PlayerPrefs.SetFloat ("HighScore", result);
			}
			//3秒後にメソッドを実行する
			Invoke("OnRetry", 5);
		}
	}
    // Retryボタンが押されたとき
	public void OnRetry()
	{
        // 再読み込み
		GameObject targe1 = GameObject.Find("Dungeon");
		GameObject targe2 = GameObject.Find("Canvas");
		Destroy(targe1);
		Destroy(targe2);
		SceneManager.LoadScene ("StartScene",LoadSceneMode.Single);

		SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
	}
}
