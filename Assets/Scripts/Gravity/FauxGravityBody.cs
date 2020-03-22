using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FauxGravityBody : MonoBehaviour
{
    [SerializeField]
    private FauxGravityAttractor planet;
    private Rigidbody bodyRigidbody;
    
    private void Awake()
    {
        bodyRigidbody = GetComponent<Rigidbody>();
        bodyRigidbody.useGravity = false;
        bodyRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        planet.Attract(transform, bodyRigidbody);
    }
}
