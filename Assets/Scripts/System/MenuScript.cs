using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using static GameManager;
using static MenuScript.ItemList;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _menuCursor;
    [SerializeField] private GameObject _itemMenu;
    [SerializeField] private GameObject _strengthMenu;
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private PlayerCheckCircle _checkCircle;
    [SerializeField] private GameObject _talkMenu;
    [SerializeField] private GameObject _shopMenu;
    [SerializeField] private GameObject _shopMenuPanel;
    [SerializeField] private GameObject _shopItem;
    [SerializeField] private GameObject _cursol2;
    [SerializeField] private GameObject _gatyaModal;
    [SerializeField] private GameObject _gatyaSelect;
    [SerializeField] private TextMeshProUGUI _gatyaText;
    [SerializeField] private GameObject _gatyaCursol;
    [SerializeField] private Transform _yes;
    [SerializeField] private Transform _no;

    public bool IsOpenMenu = false;
    public bool IsTalking = false;
    public bool IsShopping = false;
    private bool _isOpenOtherWindow = false;
    private bool _isOpenGatya = false;
    private List<Transform> _menuList = new List<Transform>();
    private int _currentPosIndex = 0;
    private ItemList _shopItemList = new ItemList();
    private int _currentCursolPos = 0;
    private bool _gatyaWait = false;

    private class ItemObj
    {
        public string id;
        public GameObject obj;
    }
    private List<ItemObj> objs= new List<ItemObj>();
    private int _currentCursol2PosIndex = 0;

    [Serializable]
    public struct ItemList : IEnumerable<ItemData>
    {
        public List<ItemData> items;

        public IEnumerator<ItemData> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [Serializable]
        public struct ItemData
        {
            public string id;
            public string name;
            public string type;
            public int amount;
            public int reality;
            public int price;
            public string created_at;
            public string updated_at;
        }
    }

    [Serializable]
    public struct Weapon 
    {
        public string id;
        public string name;
        public int type;
        public int atk;
        public int reality;
        public string created_at;
        public string updated_at;
    }

    [Serializable]
    public struct BuyItemData
    {
        public string item_id;
        public int count;
    }



    void Start()
    {
        if (_menuUI != null)
        {
            _menuUI.SetActive(false);
            _talkMenu.SetActive(false);
            _shopMenu.SetActive(false);
            foreach (Transform child in _menuUI.GetComponent<Transform>())
            {
                if (child.gameObject.name != "Cursor")
                {
                    _menuList.Add(child);
                } 
            }
        }
        if (_itemMenu!= null) 
        {
            _itemMenu.SetActive(false);
        }
        if (_strengthMenu != null)
        {
            _strengthMenu.SetActive(false);
        }
    }

    void Update()
    {
        bool isChecked = _checkCircle.canCheck;
        bool canGatya = _checkCircle.canGatya;
        if (!IsTalking && !_isOpenGatya)
        {
            if (_menuUI != null && _itemMenu != null)
            {
                if (!IsOpenMenu)
                {
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        _menuUI.SetActive(true);
                        IsOpenMenu = true;
                    }
                }
                else
                {
                    if (!_isOpenOtherWindow)
                    {
                        if (Input.GetKeyDown(KeyCode.Z))
                        {
                            switch (_currentPosIndex)
                            {
                                case 0:
                                    if (isChecked)
                                    {
                                        _talkMenu.SetActive(true);
                                        IsTalking = true;
                                    }
                                    break;
                                case 1:
                                    _itemMenu.SetActive(true);
                                    _headerText.text = "ぶき";
                                    _isOpenOtherWindow = true;
                                    break;
                                case 2:
                                    if (canGatya)
                                    {
                                        _gatyaModal.SetActive(true);
                                        _gatyaSelect.SetActive(true);
                                        _gatyaText.text = ("ガチャをひきますか？\r\n50コイン消費");
                                        _isOpenGatya = true;
                                    }
                                    break;
                                case 3:
                                    _itemMenu.SetActive(true);
                                    _headerText.text = "つよさ";
                                    _isOpenOtherWindow = true;
                                    break;
                                case 4:
                                    _strengthMenu.SetActive(true);
                                    _isOpenOtherWindow = true;
                                    break;
                                case 5:
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (Input.GetKeyDown(KeyCode.X))
                        {
                            _menuUI.SetActive(false);
                            _currentPosIndex = 0;
                            _menuCursor.transform.position = _menuList[(0)].position;
                            IsOpenMenu = false;
                        }
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            if (_currentPosIndex != 0 && _currentPosIndex != 3)
                            {
                                _currentPosIndex--;
                                _menuCursor.transform.position = _menuList[(_currentPosIndex)].position;
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            if (_currentPosIndex != 2 && _currentPosIndex != 5)
                            {
                                _currentPosIndex++;
                                _menuCursor.transform.position = _menuList[(_currentPosIndex)].position;
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (_currentPosIndex > 2)
                            {
                                _currentPosIndex -= 3;
                                _menuCursor.transform.position = _menuList[(_currentPosIndex)].position;
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (_currentPosIndex < 3)
                            {
                                _currentPosIndex += 3;
                                _menuCursor.transform.position = _menuList[(_currentPosIndex)].position;
                            }
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.X))
                        {
                            _itemMenu.SetActive(false);
                            _strengthMenu.SetActive(false);
                            _isOpenOtherWindow = false;
                        }
                    }

                }
            } 
        }
        else
        {
            if (_isOpenGatya)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (_currentCursolPos == 1)
                    {
                        _gatyaModal.SetActive(false);
                        _gatyaSelect.SetActive(false);
                        _menuUI.SetActive(false);
                        IsOpenMenu = false;
                        _isOpenGatya = false;
                    } 
                    else
                    {
                        _gatyaSelect.SetActive(false);
                        StartCoroutine("GetGatyaData", (gameManager.BaseURL + "weapons/draw"));
                        _isOpenGatya = false;
                    }
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (_currentCursolPos != 0)
                    {
                        _gatyaCursol.transform.position = _yes.position;
                        _currentCursolPos = 0;
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (_currentCursolPos != 1)
                    {
                        _gatyaCursol.transform.position = _no.position;
                        _currentCursolPos =    1;
                    }
                }
            }
            else if (IsShopping)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    BuyItemData data = new BuyItemData();
                    data.item_id = objs[_currentCursol2PosIndex].id;
                    data.count = 1;
                    StartCoroutine(BuyItem(data, gameManager.BaseURL + "items/buy"));
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    _shopMenu.SetActive(false);
                    _menuUI.SetActive(false);
                    _talkMenu.SetActive(false);
                    IsShopping = false;
                    IsTalking = false;
                    IsOpenMenu= false;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (_currentCursol2PosIndex != 0)
                    {
                        _cursol2.transform.position = objs[_currentCursol2PosIndex - 1].obj.transform.position;
                        _currentCursol2PosIndex--;
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (_currentCursol2PosIndex != 4)
                    {
                        _cursol2.transform.position = objs[_currentCursol2PosIndex + 1].obj.transform.position;
                        _currentCursol2PosIndex++;
                    }
                }
            } 
            else
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    StartCoroutine("GetShopData", (gameManager.BaseURL + "items/"));
                    IsShopping = true;
                }
            }
        }
        if (gameManager.isOpenBounasText)
        {
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Z)) 
            {
                gameManager.isOpenBounasText= false;
            }
        }
    }

    private IEnumerator GetGatyaData(string URL)
    {

        Weapon weapon = new Weapon();

        WWWForm form  = new WWWForm();

        UnityWebRequest response = UnityWebRequest.Post(URL, form);


        string Bearertext = ($"Bearer {gameManager.Bearer}");
        Debug.Log(Bearertext);


        response.SetRequestHeader("Authorization", Bearertext);
        response.SetRequestHeader("Content-Type", "application/json");

        //byte[] bodyRaw = Encoding.UTF8.GetBytes("");
        //response.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        //response.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return response.SendWebRequest();
        switch (response.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("送信中");
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log(response.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(response.error);
                Debug.Log("purotocol Err");
                break;
            case UnityWebRequest.Result.DataProcessingError:
                break;
            case UnityWebRequest.Result.ConnectionError:
                break;
        }

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

                _gatyaText.text = (weapon.name + "が当たりました。");

                _gatyaWait = true;

                break;
        }
    }

    private IEnumerator GetShopData(string URL)
    {

        ItemList itemList = new ItemList();
        UnityWebRequest response = UnityWebRequest.Get(URL);
        Debug.Log("Bearer " + gameManager.Bearer);

        response.SetRequestHeader("Authorization", "Bearer "+ gameManager.Bearer);
        


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
                jsontext = "{\"items\":" + jsontext + "}";
                Debug.Log(jsontext);

                itemList = JsonUtility.FromJson<ItemList>(jsontext);
                foreach (var item in itemList)
                {
                    Debug.Log(item.name);
                    var newItem = Instantiate(_shopItem, this.transform.position, Quaternion.identity, _shopMenuPanel.transform);
                    TextMeshProUGUI text = newItem.GetComponent<TextMeshProUGUI>();
                    text.text = item.name;
                    TextMeshProUGUI price = newItem.transform.Find("PriceNum").GetComponent<TextMeshProUGUI>();
                    price.text = item.price.ToString();
                    ItemObj obj = new ItemObj();
                    obj.id = item.id;
                    obj.obj = newItem;
                    objs.Add(obj);
                }

                _shopItemList = itemList;

                _shopMenu.SetActive(true);


                break;
        }
    }

    IEnumerator BuyItem(BuyItemData Data,string url)
    {
        string jsonbody = JsonUtility.ToJson(Data);
        UnityWebRequest response = UnityWebRequest.PostWwwForm(url, jsonbody);
        string Bearertext = ($"Bearer {gameManager.Bearer}");
        Debug.Log(Bearertext);

        response.SetRequestHeader("Authorization", Bearertext);
        response.SetRequestHeader("Content-Type", "application/json");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonbody);
        response.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        response.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return response.SendWebRequest();
        switch (response.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("送信中");
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log(response.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(response.error);
                Debug.Log("purotocol Err");
                break;
            case UnityWebRequest.Result.DataProcessingError:
                break;
            case UnityWebRequest.Result.ConnectionError:
                break;
        }
    }

}
