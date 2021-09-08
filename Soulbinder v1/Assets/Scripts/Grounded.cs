using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        player = gameObject.transform.parent.gameObject;
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Ground")
        {
            player.GetComponent<FightScript>().isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.tag == "Ground")
        {
            player.GetComponent<FightScript>().isGrounded = false;
        }
    }
}
