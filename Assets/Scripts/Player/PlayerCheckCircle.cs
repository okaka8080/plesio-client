using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckCircle : MonoBehaviour
{
    public bool canCheck = false;
    public bool canGatya = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Shoper")
        {
            canCheck = true;
        } 
        else if (other.gameObject.tag == "Gatya")
        {
            canGatya = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Shoper")
        {
            canCheck = true;
        }
        else if (other.gameObject.tag == "Gatya")
        {
            canGatya = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Shoper")
        {
            canCheck = false;
        }
        else if (other.gameObject.tag == "Gatya")
        {
            canGatya = false;
        }
    }
}
