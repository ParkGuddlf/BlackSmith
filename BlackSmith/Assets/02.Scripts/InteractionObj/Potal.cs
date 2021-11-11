using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal : MonoBehaviour
{
    [SerializeField] Transform potalPos;
    [SerializeField] int x, y, z;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.transform.position = potalPos.position + new Vector3(x,y,z);
            Camera.main.transform.position = potalPos.position + new Vector3(x, y, z) + new Vector3(0, 3, 8);

        }
    }
}
