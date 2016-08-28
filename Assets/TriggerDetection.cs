using UnityEngine;
using System.Collections;

public class TriggerDetection : MonoBehaviour {

    public bool detected;

    void Start()
    {
        detected = false;
    }

    void Update()
    {
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Platform")
        {
            Debug.Log("Hi");
        }
    }
}
