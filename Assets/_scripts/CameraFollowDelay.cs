using UnityEngine;
using System.Collections;

public class CameraFollowDelay : MonoBehaviour {
	
	// The object to lock on to
	public Transform targetTransform;

	// Controls the rotation
	public static float damping = 6.0f;

	void LateUpdate() {
		
		if (targetTransform) {

			//Look at and dampen the rotation
			Vector3 targetPosition = new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z);

			transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * damping);

		}

	}

}