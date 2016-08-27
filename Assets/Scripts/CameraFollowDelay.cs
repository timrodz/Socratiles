using UnityEngine;
using System.Collections;

public class CameraFollowDelay : MonoBehaviour {
	
	// The object to lock on to
	public Transform target;

	// Controls the rotation
	public float damping = 6.0f;

	public bool smooth = true;

	public float offsetX = 0.0f;
	public float offsetY = 0.0f;
	public float offsetZ = 0.0f;

	void LateUpdate() {
		if (target) {

			//Look at and dampen the rotation
			Vector3 targetPosition = new Vector3(target.position.x + offsetX, target.position.y + offsetY, target.position.z + offsetZ);

			transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * damping);

		}

	}

}