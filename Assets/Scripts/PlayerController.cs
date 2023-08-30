using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject powerupIndicator;
    public GameObject projectilePrefab;

    public bool hasPowerup = false;
    public bool hasPowerupHoming = false;
    public bool hasPowerupSmash = false;
    public float speed = 5.0f;
    private float powerupStrength = 15.0f;
    private float blastPower = 2000.0f;
    private float upForce = 30.0f;
    private float downForce = 60.0f;
    private float startDelay = 1.0f;
    private float repeatRate = 2.0f;
    private float powerupTime = 7.0f;
    private float projectileSpeed = 100.0f;
    private Vector3 offset = new Vector3(0.0f, -0.5f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed);
        powerupIndicator.transform.position = transform.position + offset;

        if(hasPowerupSmash)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                playerRb.AddForce(Vector3.down * downForce, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
        else if (other.CompareTag("Powerup Homing"))
        {
            hasPowerupHoming = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupHomingCountdownRoutine());
        }
        else if (other.CompareTag("Powerup Smash"))
        {
            hasPowerupSmash = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupSmashCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(powerupTime);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    IEnumerator PowerupHomingCountdownRoutine()
    {
        InvokeRepeating("CreateProjectile", startDelay, repeatRate);
        yield return new WaitForSeconds(powerupTime);
        CancelInvoke();
        hasPowerupHoming = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    IEnumerator PowerupSmashCountdownRoutine()
    {
        yield return new WaitForSeconds(powerupTime);
        hasPowerupSmash = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);

            Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + hasPowerup);            
        }

        if (collision.gameObject.CompareTag("Island") && hasPowerupSmash)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0 ; i < enemies.Length ; i++)
            {
                Rigidbody enemyRigidbody = enemies[i].gameObject.GetComponent<Rigidbody>();
                Vector3 awayFromPlayer = enemies[i].gameObject.transform.position - transform.position;
                enemyRigidbody.AddForce(awayFromPlayer.normalized * blastPower / (awayFromPlayer.magnitude/5.0f));
            }            

            //Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + hasPowerup);
        }
    }


    void CreateProjectile()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 relativePosition = (enemies[i].transform.position - transform.position).normalized;
            float projectileAngle = AngleDirection(transform.position, enemies[i].transform.position);
            GameObject newProjectile = Instantiate(projectilePrefab, transform.position + relativePosition * 2, Quaternion.Euler(90, 0, projectileAngle - 90));
            ShootProjectile(newProjectile, relativePosition);
        }
    }

    void ShootProjectile(GameObject projectile, Vector3 relativePos)
    {
        Rigidbody projectileRb = projectile.gameObject.GetComponent<Rigidbody>();
        projectileRb.AddForce(relativePos * projectileSpeed, ForceMode.Impulse);
    }

    float AngleDirection(Vector3 source, Vector3 target)
    {

        float zAxis = target.z - source.z;
        float xAxis = target.x - source.x;
        float angle = Mathf.Atan2(zAxis, xAxis) * Mathf.Rad2Deg;
        return angle;
    }
}
