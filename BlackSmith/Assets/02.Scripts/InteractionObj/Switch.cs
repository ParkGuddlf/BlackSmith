using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    BowlMix parentObj;
    Animator ani;

    private void Start()
    {
        parentObj = FindObjectOfType<BowlMix>();
        ani = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        ani.SetBool("LeverUp", parentObj.start);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hammer")
        {
            parentObj.start = true;            
        }
    }
}
