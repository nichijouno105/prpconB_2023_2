using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseButton : MonoBehaviour {

    public bool batsu = false;

    // クイズの正答を確認（ボタンから呼び出し）
    public void FalseClick()
    {
       QuizWall.instance.CheckAnswer(batsu);
       Debug.Log("batsu");
    }
}
