using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2Grounded : MonoBehaviour
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
            player.GetComponent<P2FightScript>().isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.tag == "Ground")
        {
            player.GetComponent<P2FightScript>().isGrounded = false;
        }
    }
}
