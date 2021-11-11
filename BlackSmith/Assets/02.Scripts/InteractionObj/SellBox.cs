using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellBox : MonoBehaviour
{
    GameManager gm;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            switch (other.tag)
            {
                case "Wood":                    
                    gm.money += 20;
                    Destroy(other);
                    break;
                case "Stone":
                    gm.money += 40;
                    Destroy(other);
                    break;
            }
        }
    }
}
