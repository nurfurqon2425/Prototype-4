using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody projectileRb;
    //private GameObject[] enemies;

    private float speed = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        projectileRb = GetComponent<Rigidbody>();        
    }

    // Update is called once per frame
    void Update()
    {
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        ProjectileMovement();
    }

    void ProjectileMovement()
    {
        //Vector3 lookDirection;
        //for(int i = 0 ; i < enemies.Length ; i++)
        //{
        //    lookDirection = (enemies[i].transform.position - transform.position).normalized;
        //    projectileRb.AddForce(lookDirection * speed);
        //}

        projectileRb.AddForce(Vector3.forward * speed);
    }
}
