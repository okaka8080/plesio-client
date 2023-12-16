using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckCircle : MonoBehaviour
{
    public bool canCheck = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Shoper")
        {
            Debug.Log("Shoper");
            canCheck = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Shoper")
        {
            Debug.Log("Shoper");
            canCheck = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Shoper")
        {
            Debug.Log("Shoper");
            canCheck = false;
        }
    }
}
