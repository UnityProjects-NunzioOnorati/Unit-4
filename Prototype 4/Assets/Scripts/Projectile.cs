using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float bound;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if(Mathf.Abs(transform.position.x)>bound || Mathf.Abs(transform.position.y)>bound || Mathf.Abs(transform.position.z)>bound)
        {
            Destroy(gameObject);
        }
    }
}
