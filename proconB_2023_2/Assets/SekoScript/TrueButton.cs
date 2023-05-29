using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueButton : MonoBehaviour {

    public bool maru = true;

    // クイズの正答を確認（ボタンから呼び出し）
    public void TrueClick()
    {
       QuizWall.instance.CheckAnswer(maru);
       Debug.Log("maru");
    }
}
