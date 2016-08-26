using UnityEngine;
using System.Collections;

public class CustomFollow : MonoBehaviour {
	
	public Transform target;
	//an Object to lock on to
	public float damping = 6.0f;
	//to control the rotation
	public bool smooth = true;

	public float offsetX = -23.00f;
	public float offsetY = 20.00f;
	public float offsetZ = -22.00f;

	private Transform _myTransform;

	void Awake() {
		_myTransform = transform;
	}

	// Use this for initialization
	void Start() {


	}

	// Update is called once per frame
	void Update() {

	}

	void LateUpdate() {
		if (target) {

			//Look at and dampen the rotation
			//Quaternion rotation = Quaternion.LookRotation(target.position - _myTransform.position);
			//_myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, rotation, Time.deltaTime * damping);

			Vector3 targetPosition = new Vector3(target.position.x + offsetX, target.position.y + offsetY, target.position.z + offsetZ);


			transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * damping);


		}
	}
}
