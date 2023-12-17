using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MenuScript;
using UnityEngine.Networking;

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

    public Player player;
    public List<User> users = new List<User>();
    public List<string> idList = new List<string>();

    [SerializeField] public string BaseURL;
    [SerializeField] public string Bearer;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI roginText;
    [SerializeField] private GameObject roginModal;


    public bool isRogin = false;
    public int logInBounas = 100;
    public bool isOpenBounasText = false;

    void Start()
    {
        roginModal.SetActive(false);
        player = new Player();
    }

    void Update()
    {
        coinText.text = player.Coin.ToString();
        if (!isOpenBounasText)
        {
            roginModal.SetActive(false);
        }
    }

    public void SetBearer(string b)
    {
        Bearer = b;

        if (!isRogin)
        {
            isOpenBounasText = true;
            roginModal.SetActive(true);
            roginText.text = "ログインボーナス！！\r\n\r\n"+ logInBounas.ToString() + "コインをゲット";
            isRogin = true;
        } 

    }

    private IEnumerator GetUserData(string URL)
    {

        Weapon weapon = new Weapon();
        UnityWebRequest response = UnityWebRequest.Get(URL);
        Debug.Log("Bearer " + Bearer);

        response.SetRequestHeader("Authorization", "Bearer " + Bearer);
        yield return response.SendWebRequest();

        Debug.Log(response.responseCode);
        Debug.Log(response.error);

        switch (response.result)
        {
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.ConnectionError:
                break;
            case UnityWebRequest.Result.Success:
                var jsontext = response.downloadHandler.text;
                //jsontext = "{\"items\":" + jsontext + "}";
                Debug.Log(jsontext);

                weapon = JsonUtility.FromJson<Weapon>(jsontext);


                break;
        }
    }
}
