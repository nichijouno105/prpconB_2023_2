using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceAvata : MonoBehaviour
{
    public GameObject avatar;
    // public GameObject PlayerPrefab;
    // public GameObject PlayerInstance;
    //Dungeon script;
    void Start()
    {
    //script = GameObject.Find("Dungeon").GetComponent<Dungeon>();
    GameObject avatarObj = Instantiate(avatar, new Vector3(Dungeon.startPos.x, -0.5f, Dungeon.startPos.y), Quaternion.identity) as GameObject;
    avatarObj.transform.parent = transform;

    // PlayerInstance = Instantiate(PlayerPrefab, transform.position, transform.rotation);
    // PlayerAction playerAction = PlayerInstance.GetComponent<PlayerAction>();

    // // handColliderを直接設定する
    // playerAction.handCollider = GetComponentInChildren<SphereCollider>();
    
    }
}
