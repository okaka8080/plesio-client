using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _menuCursor;
    [SerializeField] private GameObject _itemMenu;
    [SerializeField] private GameObject _strengthMenu;
    [SerializeField] private TextMeshProUGUI _headerText;

    public bool IsOpenMenu = false;
    private bool _isOpenOtherWindow = false;
    private List<Transform> _menuList = new List<Transform>();
    private int _currentPosIndex = 0;

    void Start()
    {
        if (_menuUI != null)
        {
            _menuUI.SetActive(false);
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
                                break;
                            case 1:
                                _itemMenu.SetActive(true);
                                _headerText.text = "‚Ô‚«";
                                _isOpenOtherWindow = true;
                                break;
                            case 2:
                                break; 
                            case 3:
                                _itemMenu.SetActive(true);
                                _headerText.text = "ƒOƒbƒY";
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
}
