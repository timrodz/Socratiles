using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerActivator : MonoBehaviour {

	// The object to apply transformations to
	public Transform transformObj;

	public enum TriggerType {
		Rotation,
		Translation
	};

	public TriggerType trigger;

	public VectorDirection.directions triggerDirection;

	public float distance = 1.0f;


	private float transformLength = 0.99f;

	private bool hasActivatedTrigger = false;

	// States
	private Vector3
	transformVector,
	orgTransformVector,
	oppTransformVector;

	private int triggerState = 0;

	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start() {

		// Make the transform the current parent if no object has been selected
		if (!transformObj)
			transformObj = this.transform.parent;
		
		orgTransformVector = VectorDirection.DetermineDirection(triggerDirection);
		oppTransformVector = VectorDirection.DetermineOppositeDirection(triggerDirection);

	}

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	private void OnTriggerEnter(Collider other) {

		if (!hasActivatedTrigger) {

			print("Trigger activated");

			PlayerMovement.isTileMoving = true;
			PlayerMovement.isPlayerMoving = false;

			if (triggerState == 0)
				transformVector = orgTransformVector;
			else
				transformVector = oppTransformVector;

			switch (trigger) {

				case TriggerType.Rotation:
					print("Rotation trigger at " + transform.position);
					StartCoroutine(RotateBy(transformVector * -90));
					break;
				case TriggerType.Translation:
					print("Translation trigger at " + transform.position);
					StartCoroutine(TranslateTo());
					break;

			}

			print("Finished the trigger event");

		}

	}

	/// <summary>
	/// Translates to a desired position.
	/// </summary>
	private IEnumerator TranslateTo() {

		Vector3 target = transformObj.position + (distance * transformVector);

		yield return new WaitForSeconds(0.0f);

		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / transformLength)) {

			transformObj.position = Vector3.MoveTowards(transformObj.position, target, t);
			yield return null;

		}

		// Round the transform's position
		transformObj.position = target;

		UpdateStates();

	}

	/// <summary>
	/// Rotates by a desired angle.
	/// </summary>
	public IEnumerator RotateBy(Vector3 anglesInDegrees) {

		Quaternion fromAngle = transformObj.rotation; // Get the transform's current rotation coordinates
		Quaternion toAngle = Quaternion.Euler(transformObj.eulerAngles + anglesInDegrees); // Convert byAngles to radians

		// Process a loop that lasts for the prompted time
		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / transformLength)) {

			// Make a slerp from the current rotation's coordinates to the desired rotation
			transformObj.rotation = Quaternion.Slerp(fromAngle, toAngle, t * 1.5f);
			yield return null;

		}

		// Round the rotation at the end
		transformObj.rotation = toAngle;

		UpdateStates();

	}

	/// <summary>
	/// Updates the general boolean states.
	/// </summary>
	void UpdateStates() {

		PlayerMovement.isTileMoving = false;
		PlayerMovement.isPlayerMoving = true;
		hasActivatedTrigger = false;

		if (triggerState == 0)
			triggerState = 1;
		else
			triggerState = 0;

	}

}