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

    // クイズリスト作成用
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
        QuizList.Add(0,new List<string>(){"KITのシンボルは、海の塔である？","0"});
        QuizList.Add(1,new List<string>(){"KITのシンボルは、虹の塔である？","1"});
        QuizList.Add(2,new List<string>(){"KITの学食は、オルタスという？","1"});
        QuizList.Add(3,new List<string>(){"KITは、国立大学？","1"});
        QuizList.Add(4,new List<string>(){"渋谷先生の年齢は、60歳である？","1"});
        QuizList.Add(5,new List<string>(){"渋谷先生の誕生日は、5月21日である？","0"});
        QuizList.Add(6,new List<string>(){"KITの大学設置は、1946年である？","0"});
        QuizList.Add(7,new List<string>(){"KITの母体の創立は、1899年である？","1"});
        QuizList.Add(8,new List<string>(){"KITは昔、短大があった？","1"});
        QuizList.Add(9,new List<string>(){"けいおん！の作者は、KIT出身である？","1"});
        QuizList.Add(10,new List<string>(){"現在のKITの学長は、森迫清正である？","0"});
        QuizList.Add(11,new List<string>(){"寶珍先生は、昔NTTで働いていた？","1"});
        QuizList.Add(12,new List<string>(){"現在の情報工学課程長は、稲葉先生？","0"});
        QuizList.Add(13,new List<string>(){"KITと略す大学は日本に3つ以上ある？","1"});
        QuizList.Add(14,new List<string>(){"松ケ崎駅は、1997年に開業した？","1"});
        QuizList.Add(15,new List<string>(){"KITの3号館は、国の無形文化財である？","0"});
        QuizList.Add(16,new List<string>(){"KITの3号館は、1930年に建造された？","1"});
        QuizList.Add(17,new List<string>(){"KITは、「科捜研の女」のロケ地？","1"});

    }

    void Update()
    {
        bool maru = true;
        bool batsu = false;
        // oまたはxが入力された場合処理
        // これをジョイコンのボタンに変更する
        if (Input.GetKey(KeyCode.Joystick1Button12)){
            //正解を確認する
            this.CheckAnswer(maru);
        }
        if (Input.GetKey(KeyCode.Joystick1Button9)){
            //正解を確認する
            this.CheckAnswer(batsu);
        }
    }

    // 壊れるアニメーション
    public void BreakEffect()
    {
        GameObject flagment = (GameObject)Resources.Load("breakBox");
        Instantiate(flagment, currentWall.transform.position + new Vector3(0, 1, 0), currentWall.transform.rotation);
        //Debug.Log("BoxBreak!");
    }

    // クイズを表示する
    void QuizShow() {
		QuizUI.SetActive (true);
        // ランダムな問題番号を取得
        quizNum  = UnityEngine.Random.Range(0, 17);
        QuizData = QuizList[quizNum][0];
        Debug.Log(quizNum);
        if(QuizList[quizNum][1] == "1"){
            correctAnswer = true;
        }else{
            correctAnswer = false;
        }
        quizText.text = QuizData;

        //Debug.Log(QuizData);
        //Debug.Log(correctAnswer);
    }
    // クイズの正答を確認（ボタンから呼び出し）
    public void CheckAnswer(bool answer)
    {
        // QuizFlagがtrueなら（ボタンのチャタリング防止）
        if(QuizFlag){
            // 正解
            if (answer == correctAnswer)
            {
                // 正解の場合、オブジェクトを削除
                Destroy(currentWall, 0.1f);
                BreakEffect();
                //isAnswered = true;
            }
            // 不正解
            else
            {
                // 不正解の場合、何らかの処理を行う
            }

            // クイズパネルを非表示にする
            QuizUI.SetActive(false);
            QuizFlag = false;
            //Debug.Log(QuizFlag);
        }
    }

	//playerがオブジェクトに当たったら発火
	void OnTriggerEnter(Collider collider) {
        Debug.Log(collider.gameObject.name);
		if (collider.gameObject.tag == "Player") {
            // Quiz状態に他のクイズ壁に当たった場合は無効化
            if(QuizFlag){
                return;
            }else{
                QuizFlag = true;
                // 当たった壁情報を保存
                currentWall = this.gameObject;
                //QuizFlag = true;
                QuizShow();
            }
		}
	}
}
