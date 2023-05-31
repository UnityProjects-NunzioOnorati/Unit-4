using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float bound = 20f;
    public float speed = 3f;
    public float shootingCooldown = .2f;
    private Rigidbody body;
    private GameObject player;
    private Vector3 lookDirection;
    public GameObject projectile;
    public GameManager gameScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");    
        body = GetComponent<Rigidbody>();
        gameScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        lookDirection = (player.transform.position - transform.position).normalized;
        body.AddForce(lookDirection * speed);
        
        if(!gameScript.isGameOver)
        {
            if(Mathf.Abs(transform.position.x)>bound || Mathf.Abs(transform.position.y)>bound || Mathf.Abs(transform.position.z)>bound)
            {
                Destroy(gameObject);
            }
            if(shootingCooldown>0)
            {
                shootingCooldown -= Time.deltaTime;
            }
            else
            {
                Shoot();
                shootingCooldown = 1.5f;
            }
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Projectile"))
        {
            Vector3 knockbackDirection = transform.position - other.gameObject.transform.position;
            Destroy(other.gameObject);
            knockbackDirection.Normalize();
            GetComponent<Rigidbody>().AddForce(knockbackDirection * 50, ForceMode.Impulse);
        }
    }

    void Shoot() {
        Vector3 position = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation *= Quaternion.AngleAxis(90, Vector3.right);
        projectile.tag = "BossProjectile";
        Instantiate(projectile, position, rotation);
    }
}
