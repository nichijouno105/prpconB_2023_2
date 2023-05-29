using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {

    public void BreakEffect()
    {
        GameObject flagment = (GameObject)Resources.Load("breakBox");
        Instantiate(flagment, this.transform.position + new Vector3(0, 1, 0), this.transform.rotation);
    }

	//playerがオブジェクトに当たったら発火
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			// 0.2秒後に消える
			Destroy(this.gameObject, 0.1f);
            BreakEffect();
            Debug.Log("ObjectDestroy");
		}
	}
}
