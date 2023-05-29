using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        // if (other.gameObject.tag == "Ground")
        // {
            // 物体に当たった3秒後に消える
            Destroy(this.gameObject, 3f);
        // }
    }
}
