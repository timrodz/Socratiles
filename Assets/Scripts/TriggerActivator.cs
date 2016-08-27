using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerActivator : MonoBehaviour {
	
	public Transform transformObj;

	public enum TriggerType {
		Rotation,
		Translation
	};

	public TriggerType trigger;

	public VectorDirection.directions triggerDirection;

	public float distanceScale = 1.0f;


	private float fDuration = 0.99f;

	private Vector3 translationVector;

	private bool hasActivatedTrigger = false;

	private void Start() {

		if (!transformObj)
			transformObj = this.transform.parent;

	}

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	private void OnTriggerEnter(Collider other) {

		if (!hasActivatedTrigger) {

			PlayerMovement.isTileMoving = true;

			print(PlayerMovement.isTileMoving);

			switch (trigger) {

				case TriggerType.Rotation:
					print("Rotation trigger at " + transform.position);
					StartCoroutine(RotateBy(VectorDirection.DetermineDirection(triggerDirection) * -90));
					break;
				case TriggerType.Translation:
					print("Translation trigger at " + transform.position);
					StartCoroutine(TranslateTo());
					break;

			}

		}

	}

	/// <summary>
	/// Translates to a desired position.
	/// </summary>
	private IEnumerator TranslateTo() {

		distanceScale = transformObj.FindChild("Floor").GetComponent<Transform>().localScale.x;

		translationVector = VectorDirection.DetermineDirection(triggerDirection);

		Vector3 target = transformObj.position + (distanceScale * translationVector);

		yield return new WaitForSeconds(0.0f);

		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / fDuration)) {

			transformObj.position = Vector3.MoveTowards(transformObj.position, target, t);
			yield return null;

		}

		// Round the transform's position
		transformObj.position = target;

		PlayerMovement.isTileMoving = false;
		hasActivatedTrigger = false;

	}

	/// <summary>
	/// Rotates by a desired angle.
	/// </summary>
	public IEnumerator RotateBy(Vector3 anglesInDegrees) {

		Quaternion fromAngle = transformObj.rotation; // Get the transform's current rotation coordinates
		Quaternion toAngle = Quaternion.Euler(transformObj.eulerAngles + anglesInDegrees); // Convert byAngles to radians

		// Process a loop that lasts for the prompted time
		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / fDuration)) {

			// Make a slerp from the current rotation's coordinates to the desired rotation
			transformObj.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
			yield return null;

		}

		// Round the rotation at the end
		transformObj.rotation = toAngle;

		PlayerMovement.isTileMoving = false;
		hasActivatedTrigger = false;

	}

}