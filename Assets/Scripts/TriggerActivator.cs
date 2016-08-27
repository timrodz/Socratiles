using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerActivator : MonoBehaviour {

	// The object to apply transformations to
	public Transform tileTransform;

	public enum TriggerType {
		TurnRotation,
		Translation,
		WinningRotation,
		WinningTranslation
	};

	public TriggerType trigger;

	public VectorDirection.directions triggerDirection;

	public float distance = 1.0f;

	public float transformLength = 0.99f;

	private bool hasActivatedTrigger = false;

	// States
	private Vector3
	transformVector,
	orgTransformVector,
	oppTransformVector;

	private int triggerState = 0;

	bool hasReachedGoal = false;

	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start() {

		// Make the transform the current parent if no object has been selected
		if (!tileTransform)
			tileTransform = this.transform.parent;
		
		orgTransformVector = VectorDirection.DetermineDirection(triggerDirection);
		oppTransformVector = VectorDirection.DetermineOppositeDirection(triggerDirection);

	}

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	private void OnTriggerEnter(Collider other) {

		if (!hasActivatedTrigger) {

			PlayerMovement.isTileMoving = true;
			PlayerMovement.isPlayerMoving = false;

			if (triggerState == 0)
				transformVector = orgTransformVector;
			else
				transformVector = oppTransformVector;

			switch (trigger) {

				case TriggerType.TurnRotation:
					StartCoroutine(RotateByAxis(transformVector * -90));
					break;
				case TriggerType.Translation:
					StartCoroutine(TranslateTo());
					break;
				case TriggerType.WinningRotation:
					hasReachedGoal = true;
					StartCoroutine(RotateByAxis(transformVector * 180));
					break;
				case TriggerType.WinningTranslation:
					hasReachedGoal = true;
					StartCoroutine(TranslateTo());
					break;

			}

		}

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

	/// <summary>
	/// Translates to a desired position.
	/// </summary>
	private IEnumerator TranslateTo() {

		Vector3 target = tileTransform.position + (distance * transformVector);

		yield return new WaitForSeconds(0.0f);

		for (float t = 0.0f; t < 0.75f; t += (Time.deltaTime / transformLength)) {

			tileTransform.position = Vector3.MoveTowards(tileTransform.position, target, t);
			yield return null;

		}

		UpdateStates();

		if (hasReachedGoal) {

			print("Reached goal");
			hasReachedGoal = !hasReachedGoal;
			EnableTransform et = transform.gameObject.GetComponent<EnableTransform>();
			et.Enable();

		} else {
			
			// Round the transform's position
			tileTransform.position = target;

		}

	}

	/// <summary>
	/// Rotates by a desired angle.
	/// </summary>
	public IEnumerator RotateByAxis(Vector3 anglesInDegrees) {

		Quaternion fromAngle = tileTransform.rotation; // Get the transform's current rotation coordinates
		Quaternion toAngle = Quaternion.Euler(tileTransform.eulerAngles + anglesInDegrees); // Convert byAngles to radians

		// Process a loop that lasts for the prompted time
		for (float t = 0.0f; t < 0.75f; t += (Time.deltaTime / transformLength)) {

			// Make a slerp from the current rotation's coordinates to the desired rotation
			tileTransform.rotation = Quaternion.Slerp(fromAngle, toAngle, t * 1.5f);
			yield return null;

		}

		// Round the rotation at the end
		tileTransform.rotation = toAngle;

		UpdateStates();

		if (hasReachedGoal) {

			print("Reached goal");
			hasReachedGoal = !hasReachedGoal;
			EnableTransform et = transform.gameObject.GetComponent<EnableTransform>();
			et.Enable();

		}

	}

}