using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{

    public Rigidbody kinematicObject;

    private void Start()
    {
        kinematicObject = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Manually simulate physics collisions
        Physics.Simulate(Time.deltaTime);

        // Check for collisions with the kinematic object
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider collider in colliders)
        {
            Rigidbody otherRigidbody = collider.attachedRigidbody;
            if (otherRigidbody != null && otherRigidbody == kinematicObject)
            {
                Debug.Log("Object " + otherRigidbody.gameObject.name + " collided with the sphere.");
            }
        }
    }

}
