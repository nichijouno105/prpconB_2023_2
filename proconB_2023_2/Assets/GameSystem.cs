using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
 
public class GameSystem : MonoBehaviour {
 
	//　スタートボタンを押したら実行する
	public void StartGame() {
        Debug.Log("Start");
		SceneManager.LoadScene ("CreateGame");
	}
	void Update(){
		if(Input.GetKey(KeyCode.Joystick1Button9))
		StartGame();
	}
}
 
