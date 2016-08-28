using UnityEngine;
using System.Collections;

public class RotateAnimation : MonoBehaviour {

    Transform skybox;

	// Use this for initialization
	void Start () {

        skybox = GetComponent<Transform>();
	
	}
	
	// Update is called once per frame
	void Update () {

        skybox.RotateAround(transform.position, transform.up, Time.deltaTime * 1.5f);
	}
}
