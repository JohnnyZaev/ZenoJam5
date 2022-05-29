using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed;

    private Material matBlink;
    private Material matDefault;
    private SpriteRenderer spriteRend;

    public int positionOfPatrol;
    public Transform startPoint;
    private bool movingRight;

    private Transform player;
    public float stoppingDistance;

    private bool chill = false;
    private bool angry = false;
    private bool goBack = false;

    private Animator anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRend = GetComponent<SpriteRenderer>();
        matBlink = Resources.Load("EnemyBlink", typeof(Material)) as Material;
        matDefault = spriteRend.material;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    void Update()
    {
        if (health <= 0)
        {
            anim.Play("dealthEye");
            Invoke("DestroyEnemy", 0.2f);
        }
            
        if (Vector2.Distance(transform.position, startPoint.position) < positionOfPatrol && !angry)
        {
            chill = true;
        }

        if (Vector2.Distance(transform.position, player.position) < stoppingDistance)
        {
            if (transform.position.y - player.position.y >  0.3f || transform.position.y - player.position.y < -0.3f)
                ;
            else
            {
                angry = true;
                chill = false;
                goBack = false;
            }
            
        }

        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            goBack = true;
            angry = false;
        }

        if (chill)
            Chill();
        else if (angry)
            Angry();
        else if (goBack)
            GoBack();
    }

    void Chill()
    {
        if (transform.position.x > startPoint.position.x + positionOfPatrol)
        {
            movingRight = false;
            Flip();
        }
        else if (transform.position.x < startPoint.position.x - positionOfPatrol)
        {
            movingRight = true;
            Flip();
        }

        if (movingRight)
        {
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        }
        else
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
    }

    void Angry()
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(player.position.x, transform.position.y), speed * Time.deltaTime);
    }

    void GoBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, startPoint.position, speed * Time.deltaTime);
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
         //зеркально отражаем персонажа по оси Х
         theScale.x *= -1;
         //задаем новый размер персонажа, равный старому, но зеркально отраженный
         transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("attackBorder"))
        {
            spriteRend.material = matBlink;
            Invoke("ResetMaterial", 0.2f);
        }
    }

    void ResetMaterial()
    {
        spriteRend.material = matDefault;
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

}
