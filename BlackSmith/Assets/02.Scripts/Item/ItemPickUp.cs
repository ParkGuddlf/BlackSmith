using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemType item;
    Rigidbody rigi;

    private void Start()
    {
        rigi = GetComponent<Rigidbody>();
        GetComponent<ParticleSystem>().Stop();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rigi.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            GetComponent<ParticleSystem>().Play();
        }
    }
}    




//https://ansohxxn.github.io/unity%20lesson%203/ch5-1/
