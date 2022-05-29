using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
            other.GetComponent<Enemy>().TakeDamage(20);
        
    }
}
