// 自作した落下判定を行うスクリプトです //
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 落下判定時にシーン切り替えを行うために使用するライブラリ
using UnityEngine.SceneManagement;

public class Out : MonoBehaviour
{
    // is Trigar = trueのオブジェクトのあたり判定
    void OnTriggerEnter( Collider col ) {
        // playerが当たったら実行
		if (col.gameObject.tag == "Player") {
            // 現在のシーンを読み込み直す
			SceneManager.LoadScene (
				SceneManager.GetActiveScene ().name);
		}
	}
}
