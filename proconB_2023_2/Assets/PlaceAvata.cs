using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceAvata : MonoBehaviour
{
    public GameObject avatar;
    //Dungeon script;
    void Start()
    {
    //script = GameObject.Find("Dungeon").GetComponent<Dungeon>();
    GameObject avatarObj = Instantiate(avatar, new Vector3(Dungeon.startPos.x, -0.5f, Dungeon.startPos.y), Quaternion.identity) as GameObject;
    avatarObj.transform.parent = transform;
    }
}
