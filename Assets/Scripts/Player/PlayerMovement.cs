using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private MenuScript _menuScript;
    [SerializeField] private GameManager _gameManager;
    private Rigidbody2D _rb;
    private float _speed;
    private Animator _animator;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _speed = 8;
        _animator = GetComponent<Animator>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    void Update()
    {
        if (_rb != null)
        {
            if (!_menuScript.IsOpenMenu && !_gameManager.isOpenBounasText)
            {
                _rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * _speed;
            } 
            else
            {
                _rb.velocity = new Vector2(0, 0);
            }
            if (_animator != null)
            {
                _animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
                _animator.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
                if (_rb.velocity == new Vector2(0, 0))
                {
                    _animator.SetBool("IsRun", false);
                }
                else
                {
                    _animator.SetBool("IsRun", true);
                    _animator.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
                    _animator.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
                    
                }
            }
        }
    }
}
