using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonActivator : MonoBehaviour {
	
	public Transform objectTransform;

	public VectorDirection.directions vectorDirection;

	public int distanceScale = 1;

	private float fDuration = 0.99f;

	Vector3 translationVector;

	private bool bHasBegunTranslation = false;

	private void Start() {

//		print("Starting " + transform.name);
//		translationVector = VectorDirection.DetermineDirection(vectorDirection);

	}

	private void OnTriggerEnter(Collider other) {

		print(transform.name + " OnTriggerEnter activated via " + other.name);

		if (!bHasBegunTranslation) {
		StartCoroutine(RotateCamera(Vector3.back.normalized * -90));
//			StartCoroutine(TranslateTo(barrier));
			bHasBegunTranslation = !bHasBegunTranslation;
		}

	}

	/// Translates to the desired position
	private IEnumerator TranslateTo(Transform objectTransform) {

		Vector3 target = objectTransform.position + (distanceScale * translationVector);

		yield return new WaitForSeconds(0.0f);

		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / fDuration)) {

			objectTransform.Translate(translationVector.normalized * Time.deltaTime, Space.World);
			yield return null;

		}

		// Destroy the button after the translation has finished
		objectTransform.position = target;
		bHasBegunTranslation = !bHasBegunTranslation;

	}

	public IEnumerator RotateCamera(Vector3 anglesInDegrees) {

//		canRotateCamera = false;

		Quaternion fromAngle = transform.rotation; // Get the transform's current rotation coordinates
		Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + anglesInDegrees); // Convert byAngles to radians

//		print("fromAngle: " + fromAngle + " toAngle: " + toAngle);

		// Process a loop that lasts for the prompted time
		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / fDuration)) {

			// Make a slerp from the current rotation's coordinates to the desired rotation
			objectTransform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
			yield return null;

		}

		// Round the rotation at the end
		objectTransform.rotation = toAngle;
//		canRotateCamera = true;

		// Update the current move count
		//		GameManager.moveCount++;

	}

}