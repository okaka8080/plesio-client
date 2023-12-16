using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public class Player
    {
        public int Coin = 0;
        public int Level = 1;
        public int HP = 1;
        public float Attack = 0;
        public string WeaponID = "";
        public List<string> ItemList = new List<string>();
    }

    public class User
    {
        public string id;
        public GameObject obj;
        public Animator anim;
    }

    public List<User> users = new List<User>();
    public List<string> idList = new List<string>();

    void Start()
    {
    }

    void Update()
    {
        
    }
}
