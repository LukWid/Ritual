using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour
{

    [SerializeField]
    private float gravity = -10f;

    public void Attract(Transform body, Rigidbody bodyRigidbody)
    {
        Vector3 targetDirection = (body.position - transform.position).normalized;

        body.rotation = Quaternion.FromToRotation(body.up, targetDirection) * body.rotation;
        bodyRigidbody.AddForce(targetDirection * gravity);
        
    }
}
