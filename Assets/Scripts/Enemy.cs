using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed;

    private Material _matBlink;
    private Material _matDefault;
    private SpriteRenderer _spriteRend;

    public int positionOfPatrol;
    public Transform startPoint;
    private bool _movingRight;

    private Transform _player;
    public float stoppingDistance;

    private bool _chill = false;
    private bool _angry = false;
    private bool _goBack = false;

    private Animator _anim;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _spriteRend = GetComponent<SpriteRenderer>();
        _matBlink = Resources.Load("EnemyBlink", typeof(Material)) as Material;
        _matDefault = _spriteRend.material;
        _anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void Update()
    {
        if (health <= 0)
        {
            _anim.Play("dealthEye");
            Invoke(nameof(DestroyEnemy), 0.2f);
        }
            
        if (Vector2.Distance(transform.position, startPoint.position) < positionOfPatrol && !_angry)
        {
            _chill = true;
        }

        if (Vector2.Distance(transform.position, _player.position) < stoppingDistance)
        {
            if (transform.position.y - _player.position.y >  0.3f || transform.position.y - _player.position.y < -0.3f)
                ;
            else
            {
                _angry = true;
                _chill = false;
                _goBack = false;
            }
            
        }

        if (Vector2.Distance(transform.position, _player.position) > stoppingDistance)
        {
            _goBack = true;
            _angry = false;
        }

        if (_chill)
            Chill();
        else if (_angry)
            Angry();
        else if (_goBack)
            GoBack();
    }

    private void Chill()
    {
        if (transform.position.x > startPoint.position.x + positionOfPatrol)
        {
            _movingRight = false;
            Flip();
        }
        else if (transform.position.x < startPoint.position.x - positionOfPatrol)
        {
            _movingRight = true;
            Flip();
        }

        if (_movingRight)
        {
	        var transform1 = transform;
	        var position = transform1.position;
	        position = new Vector2(position.x + speed * Time.deltaTime, position.y);
	        transform1.position = position;
        }
        else
        {
	        var transform1 = transform;
	        var position = transform1.position;
	        position = new Vector2(position.x - speed * Time.deltaTime, position.y);
	        transform1.position = position;
        }
    }

    private void Angry()
    {
	    var position = transform.position;
	    position = Vector2.MoveTowards(new Vector2(position.x, position.y), new Vector2(_player.position.x, position.y), speed * Time.deltaTime);
	    transform.position = position;
    }

    private void GoBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, startPoint.position, speed * Time.deltaTime);
    }

    private void Flip()
    {
	    var transform1 = transform;
	    var theScale = transform1.localScale;
         //зеркально отражаем персонажа по оси Х
         theScale.x *= -1;
         //задаем новый размер персонажа, равный старому, но зеркально отраженный
         transform1.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
	    if (!col.CompareTag("attackBorder")) return;
	    _spriteRend.material = _matBlink;
	    Invoke(nameof(ResetMaterial), 0.2f);
    }

    private void ResetMaterial()
    {
        _spriteRend.material = _matDefault;
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

}
