using UnityEngine;
using System.Collections;

public class TriggerActivator : MonoBehaviour {

	public enum TriggerType {
		TurnRotation,
		Translation,
		WinningRotation,
		WinningTranslation
	};

	// The object to apply transformations to
	public Transform tileTransform;

	public TriggerType trigger;

	public VectorDirection.directions triggerDirection;

	[HideInInspector]
    public Vector3 targetTransform;

	public float distance = 1.0f;

	public float transformLength = 0.99f;

	private bool hasActivatedTrigger = false;

    public bool shouldHaveStates = true;

    // States
    private Vector3
	transformVector,
	orgTransformVector,
	oppTransformVector;

	private int triggerState = 0;

	bool hasReachedGoal = false;

    // Audio
    private AudioSource aSource;

	public bool playSound;

	private void Awake() {

		if (playSound)
			aSource = GetComponent<AudioSource>();

	}


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

			// Rotations won't have states
			if (!shouldHaveStates && (trigger == TriggerType.TurnRotation))
				transformVector = orgTransformVector;

			// Play the audio clip
			if (playSound)
				aSource.PlayOneShot(aSource.clip, 1);

			switch (trigger) {

				// rotation
				case TriggerType.TurnRotation:
					StartCoroutine(RaiseAndLower(1.1f));
					StartCoroutine(RotateByAxis(transformVector * -90));
					break;
				case TriggerType.Translation:
					StartCoroutine(TranslateTo());
					break;
				case TriggerType.WinningRotation:
                    // StartCoroutine(RotateByAxis(transformVector * 180));
					break;
				case TriggerType.WinningTranslation:
                    hasReachedGoal = true;
					StartCoroutine(TranslateTo());
					StartCoroutine(RotateByAxis(transformVector * 180));
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

		targetTransform = tileTransform.position + (distance * transformVector);

		if (hasReachedGoal) {
			yield return new WaitForSeconds(0.2f);
            CameraFollowDelay.damping = 100;
		}

		for (float t = 0.0f; t < 0.75f; t += (Time.deltaTime / transformLength)) {

			tileTransform.position = Vector3.MoveTowards(tileTransform.position, targetTransform, t);

			if ((t >= 0.45f) && (hasReachedGoal)) {

             	print("Reached Goal at " + tileTransform.position);
             	hasReachedGoal = !hasReachedGoal;
				EnableTransform et = transform.gameObject.GetComponent<EnableTransform>();
				et.EnableObject();

            }

			yield return null;

		}

		// Round the transform's position
		tileTransform.position = targetTransform;

		UpdateStates();

        if (hasReachedGoal)
            CameraFollowDelay.damping = 6;

    }

	/// <summary>
	/// Rotates by a desired angle.
	/// </summary>
	private IEnumerator RotateByAxis(Vector3 angleToRotate) {

        if (hasReachedGoal)	{
            yield return new WaitForSeconds(0.5f);
        }

        Quaternion fromAngle = tileTransform.rotation; // Get the transform's current rotation coordinates
		Quaternion toAngle = Quaternion.Euler(tileTransform.eulerAngles + angleToRotate); // Convert byAngles to radians

		// Process a loop that lasts for the prompted time
		for (float t = 0.0f; t < 0.75f; t += (Time.deltaTime / transformLength)) {

			// Make a slerp from the current rotation's coordinates to the desired rotation
			tileTransform.rotation = Quaternion.Slerp(fromAngle, toAngle, t * 1.5f);

			yield return null;

		}

		// Round the rotation at the end
		tileTransform.rotation = toAngle;

		UpdateStates();

	}

	/// <summary>
	/// Raises and lowers by a desired distance (for rising and lowering)
	/// </summary>
	private IEnumerator RaiseAndLower(float distance) {

		// Raise up
		Vector3 target = tileTransform.position + (distance * Vector3.up);
		print("Rising up");

		for (float t = 0.0f; t < 0.5f; t += (Time.deltaTime / (transformLength))) {

			tileTransform.position = Vector3.MoveTowards(tileTransform.position, target, t * 1.25f);
			yield return null;

		}

		target = tileTransform.position + (distance * Vector3.down);
		print("Lowering down");

		for (float t = 0.0f; t < 0.5f; t += (Time.deltaTime / (transformLength))) {

			tileTransform.position = Vector3.MoveTowards(tileTransform.position, target, t);
			yield return null;

		}

	}

}