using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using WebSocketSharp;
using WebSocketSharp.Net;
using Cysharp.Threading.Tasks;

public class WsManager : MonoBehaviour
{
    [SerializeField] public string BackendURL;
    [SerializeField] private Transform _userTransform;
    [SerializeField] private Animator _userAnimator;
    [SerializeField] private GameObject _otherUserPrefab;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] RuntimeAnimatorController animationController;
    private WebSocket ws;
    private bool isCreateUser = false;
    private Message responce;
    public string UUID;

    public class Message
    {
        public string id;
        public float posX;
        public float posY;
        public float posZ;
        public float moveX;
        public float moveY;
        public float lastMoveX;
        public float lastMoveY;
        public bool isRun;
    }

     void Start()
    {
        _gameManager = GetComponent<GameManager>();
        Application.targetFrameRate = 30;
        var guid = System.Guid.NewGuid();
        UUID = guid.ToString();

        _userTransform = GameObject.Find("User").gameObject.transform;
        _userAnimator = GameObject.Find("User").gameObject.GetComponent<Animator>();

        ws = new WebSocket(BackendURL);
        ws.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Open");
        };

        ws.OnMessage += (sender, e) =>
        {
            //Debug.Log("gotData" + e.Data);
            var res = JsonUtility.FromJson<Message>(e.Data);
            if (res != null)
            {
                if (res.id != UUID)
                {
                    responce = res;
                    isCreateUser = true;
                }
            }
        };

        ws.Connect();

    }

    void Update()
    {

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("close");
            Debug.Log(e.Reason);
        };
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Open");
        };

        if (isCreateUser)
        {
            CreateUser(responce);
        }
        
        Message message = new Message();

        message.id = UUID;
        message.posX = _userTransform.position.x;
        message.posY = _userTransform.position.y;
        message.posZ = _userTransform.position.z;
        message.moveX = _userAnimator.GetFloat("MoveX");
        message.moveY = _userAnimator.GetFloat("MoveY");
        message.lastMoveX = _userAnimator.GetFloat("LastMoveX");
        message.lastMoveY = _userAnimator.GetFloat("LastMoveY");
        message.isRun = _userAnimator.GetBool("IsRun");
        //毎フレームユーザーの位置を送る。
        string messageJson = JsonUtility.ToJson(message);
        //Debug.Log(messageJson);

        ws.Send(messageJson);
    }

    void OnDestroy()
    {
        ws.Close();
        ws = null;
    }

    private void CreateUser (Message message)
    {
        isCreateUser = false;
        if ((_gameManager.idList.Exists(x => x.Equals(message.id))) == false)
        {
            //存在しない場合生成
            try
            {
                responce = message;
                _gameManager.idList.Add(message.id);
                GameObject obj = Instantiate(_otherUserPrefab, new Vector3(message.posX, message.posY, message.posZ), Quaternion.identity);
                obj.name = message.id;
                GameManager.User user = new GameManager.User();
                user.id = message.id;
                user.obj = obj;
                Animator animator = user.obj.AddComponent<Animator>();
                animator.runtimeAnimatorController = animationController;
                user.anim = animator;
                _gameManager.users.Add(user);

            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }

        }
        else
        {
            //存在する場合移動処理
            try
            {
                foreach (var user in _gameManager.users)
                {
                    if (user.id == message.id)
                    {
                        if (message.isRun)
                        {
                            user.anim.SetBool("IsRun", true);
                            user.anim.SetFloat("MoveX", message.moveX);
                            user.anim.SetFloat("MoveY", message.moveY);
                        }
                        else
                        {
                            user.anim.SetBool("IsRun", false);
                            user.anim.SetFloat("LestMoveX", message.lastMoveX);
                            user.anim.SetFloat("LastMoveY", message.lastMoveY);
                        }

                        user.obj.transform.position = Vector3.MoveTowards(user.obj.transform.position, new Vector3(message.posX, message.posY, message.posZ), 100.0f * Time.deltaTime);
                        
                        
                        break;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }

        }


    }
}
