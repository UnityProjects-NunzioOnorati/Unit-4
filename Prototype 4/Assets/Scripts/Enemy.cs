using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float bound = 20f;
    public float speed = 3f;
    private Rigidbody body;
    private GameObject player;
    private Vector3 lookDirection;
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
        if(!gameScript.isGameOver)
        {
            lookDirection = (player.transform.position - transform.position).normalized;
            body.AddForce(lookDirection * speed);
            if(Mathf.Abs(transform.position.x)>bound || Mathf.Abs(transform.position.y)>bound || Mathf.Abs(transform.position.z)>bound)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
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
}
