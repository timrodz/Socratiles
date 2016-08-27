using UnityEngine;
using System.Collections;

public class UnstableTile : MonoBehaviour {

    public int playerSteppedOnMe = 0;
    public MeshFilter floor;
    public Mesh crumbled;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (playerSteppedOnMe > 1)
        {
            transform.parent.gameObject.SetActive(false);
        }
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerSteppedOnMe++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            floor.mesh = crumbled;
        }
    }
}
