using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;

    public float speed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        EnemyAttack();
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
        }
    }

    void EnemyAttack()
    {
        int attackModifier = 0;
        if (gameObject.name == "Enemy(Clone)")
        {
            attackModifier = 1;
        }
        else if (gameObject.name == "Hard Enemy(Clone)")
        {
            attackModifier = 2;
        }
        else if (gameObject.name == "Boss(Clone)")
        {
            attackModifier = 5;
        }
        else if (gameObject.name == "Boss")
        {
            attackModifier = 3;
        }
        Vector3 lookDirection = (player.transform.position - enemyRb.transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed * attackModifier);
    }
}
