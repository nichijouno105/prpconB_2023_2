using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ライブラリ読み込み
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizWall : MonoBehaviour {

    // クイズ結果表示
	public Text quizText;
	public GameObject QuizUI;

    // Quiz
    public static bool QuizFlag = false;
    // string  QuizData = "KITのシンボルは、風の塔である？";
    // bool correctAnswer = false;
    public static string  QuizData ;
    public static bool correctAnswer;
    //bool isAnswered = false; // quizを解答したかどうか
    int quizNum;

    //---以下クイズリスト作成---------//
    Dictionary<int,List<string>> QuizList;
    
    // ◯と×ボタンからの呼び出し用インスタンス
    public static QuizWall instance;
    // 複数の壁を生成した時に接触した物体を保持
    public static GameObject currentWall;

    // instanceの設定
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // クイズの内容を登録
        QuizList = new Dictionary<int,List<string>>();
        QuizList.Add(0,new List<string>(){"KITのシンボルは、風の塔である？","0"});
        QuizList.Add(1,new List<string>(){"KITの学食はオルタスという？","1"});
        QuizList.Add(2,new List<string>(){"KITは国立大学？","1"});
    }

    void Update()
    {
        bool maru = true;
        bool batsu = false;
        //エンターキーが入力された場合「true」
        if (Input.GetKey(KeyCode.O)){
            //オブジェクトを削除
            CheckAnswer(maru);
        }
        if (Input.GetKey(KeyCode.X)){
            //オブジェクトを削除
            CheckAnswer(batsu);
        }
    }

    // 壊れる演出
    public void BreakEffect()
    {
        GameObject flagment = (GameObject)Resources.Load("breakBox");
        Instantiate(flagment, currentWall.transform.position + new Vector3(0, 1, 0), currentWall.transform.rotation);
    }

    // クイズを表示する
    void QuizShow() {
		QuizUI.SetActive (true);
        // ランダムな問題番号を取得
        quizNum  = UnityEngine.Random.Range(0, 3);
        QuizData = QuizList[quizNum][0];
        Debug.Log(quizNum);
        if(QuizList[quizNum][1] == "1"){
            correctAnswer = true;
        }else{
            correctAnswer = false;
        }
        quizText.text = QuizData;

        Debug.Log(QuizData);
        Debug.Log(correctAnswer);
    }
    // クイズの正答を確認（ボタンから呼び出し）
    public void CheckAnswer(bool answer)
    {
        if (answer == correctAnswer)
        {
            // 正解の場合、オブジェクトを削除
            Destroy(currentWall, 0.1f);
            BreakEffect();
            //isAnswered = true;
        }
        else
        {
            // 不正解の場合、何らかの処理を行う（例えば再挑戦など）
            //quizText.text = QuizData;
        }

        // クイズパネルを非表示にする
        QuizUI.SetActive(false);
        QuizFlag = false;
        Debug.Log(QuizFlag);
    }

	//playerがオブジェクトに当たったら発火
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
            // Quiz状態に他のクイズ壁に当たった場合は無効化
            if(QuizFlag){
                return;
            }else{
                QuizFlag = true;
                currentWall = this.gameObject;
                //QuizFlag = true;
                QuizShow();
            }
		}
	}
}
