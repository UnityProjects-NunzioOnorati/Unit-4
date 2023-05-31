using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float bound = 20f;
    private Rigidbody body;
    private GameObject focalPoint;
    public GameObject powerupIndicator;
    public GameObject projectilePrefab;
    public GameManager gameScript;

    public float speed = 5f;
    private bool hasStrengthPowerup = false;
    private bool hasShootingPowerup = false;
    private bool hasSmashPowerup = false;
    private float powerupStrength = 15f;
    private float shootingCooldown = 0;
    private float smashingCooldown = 0;
    private bool isOnGround = true;
    public bool isSmashing = false;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
        gameScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput;
        if(isOnGround)
            verticalInput = Input.GetAxis("Vertical");
        else
            verticalInput = 0;
        body.AddForce(focalPoint.transform.forward * speed * verticalInput);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        if(Input.GetKey(KeyCode.Space) && hasShootingPowerup && shootingCooldown <= 0){
            Shoot();
            shootingCooldown = 1.5f;
        }else if(shootingCooldown > 0){
            shootingCooldown -= Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.Space) && hasSmashPowerup && smashingCooldown <= 0 && isOnGround){
            isOnGround=false;
            transform.position+=new Vector3(0, 5, 0);
            body.angularVelocity=Vector3.zero;
            body.velocity=Vector3.zero;
            isSmashing = true;
            transform.position-=new Vector3(0, -5, 0);
            smashingCooldown = 2f;
        }else if(smashingCooldown > 0){
            smashingCooldown -= Time.deltaTime;
        }

        if(Mathf.Abs(transform.position.x)>bound || Mathf.Abs(transform.position.y)>bound || Mathf.Abs(transform.position.z)>bound)
        {
            gameScript.isGameOver = true;
            Destroy(gameObject);
            Destroy(powerupIndicator);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup") && other.gameObject.name.Contains("Powerup 1") && !hasShootingPowerup && !hasSmashPowerup){
            hasStrengthPowerup = true;
            Destroy(other.gameObject);
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine(7,0));
        }       
        else if(other.CompareTag("Powerup") && other.gameObject.name.Contains("Powerup 2") && !hasStrengthPowerup && !hasSmashPowerup){
            hasShootingPowerup = true;
            Destroy(other.gameObject);
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine(7,1));
        }
        else if(other.CompareTag("Powerup") && other.gameObject.name.Contains("Powerup 3") && !hasStrengthPowerup && !hasShootingPowerup){
            hasSmashPowerup = true;
            Destroy(other.gameObject);
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine(7,2));
        }
        else if(other.CompareTag("BossProjectile"))
        {
            Vector3 knockbackDirection = transform.position - other.gameObject.transform.position;
            Destroy(other.gameObject);
            knockbackDirection.Normalize();
            body.AddForce(knockbackDirection * 50, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Enemy") && hasStrengthPowerup) {
            
            Rigidbody enemyBody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            enemyBody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            
        }
        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnGround=true;
            isSmashing = false;
        }
    }

    IEnumerator PowerupCountdownRoutine(int secondsToWait, int powerupToStop) {
        yield return new WaitForSeconds(secondsToWait);
        switch(powerupToStop)
        {
            case 0:
                hasStrengthPowerup = false;
                break;
            case 1:
                hasShootingPowerup = false;
                break;
            case 2:
                hasSmashPowerup = false;
                break;
        }
        powerupIndicator.gameObject.SetActive(false);
    }

    void Shoot(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for(int i=0; i<enemies.Length; i++)
        {
            Vector3 position = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 direction = (enemies[i].transform.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation *= Quaternion.AngleAxis(90, Vector3.right);
            projectilePrefab.tag = "Projectile";
            var instantiatedProjectile = Instantiate(projectilePrefab, position, rotation);
        }
    }

    public float detectionRadius = 3.0f;

    private void FixedUpdate()
    {

        if(isSmashing)
        {
            // Get all colliders within the detection radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

            // Check for collisions with each collider
            foreach (Collider collider in colliders)
            {
                GameObject collided = collider.gameObject;
                if (collided != gameObject && collided.CompareTag("Enemy"))
                {
                    Debug.Log("Collision detected with " + collider.gameObject.name);
                    Rigidbody enemyBody = collided.GetComponent<Rigidbody>();
                    Vector3 direction = (collided.transform.position - gameObject.transform.position).normalized;
                    enemyBody.AddForce(direction * 100, ForceMode.Impulse);
                }
            }
        }

    }

}
