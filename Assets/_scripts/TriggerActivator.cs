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

	public float translateDistance = 1.0f;

	public float duration = 1.0f;

	private float amountToWaitBeforeActivation = 0.2f;

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
					StartCoroutine(RaiseAndLowerBy(1.5f));
					StartCoroutine(RotateBy(transformVector * -90));
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
					StartCoroutine(RotateBy(transformVector * 180));
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

		yield return new WaitForSeconds(amountToWaitBeforeActivation);

		targetTransform = tileTransform.position + (translateDistance * transformVector);

		if (hasReachedGoal) {
			yield return new WaitForSeconds(0.2f);
            CameraFollowDelay.damping = 100;
		}

		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / duration)) {

			tileTransform.position = Vector3.MoveTowards(tileTransform.position, targetTransform, t);

			if ((t >= 0.85f) && (hasReachedGoal)) {

             	print("Reached Goal at " + tileTransform.position);
             	hasReachedGoal = !hasReachedGoal;
				EnableTransform et = transform.gameObject.GetComponent<EnableTransform>();
				et.EnableObject();

            }

			yield return null;

		}

		// Round the transform's position
		// tileTransform.position = targetTransform;

		UpdateStates();

        if (hasReachedGoal)
            CameraFollowDelay.damping = 6;

    }

	/// <summary>
	/// Rotates by a desired angle.
	/// </summary>
	private IEnumerator RotateBy(Vector3 angleToRotate) {

		yield return new WaitForSeconds(amountToWaitBeforeActivation);

        if (hasReachedGoal)	{
            yield return new WaitForSeconds(0.5f);
        }

		// Get the transform's current rotation coordinates
        Quaternion fromAngle = tileTransform.rotation;

		// Convert byAngles to radians
		Quaternion toAngle = Quaternion.Euler(tileTransform.eulerAngles + angleToRotate);

		// Process a loop that lasts for the prompted time
		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / duration)) {

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
	/// <param name="distance"> The distance to raise and lower by </param>
	private IEnumerator RaiseAndLowerBy(float distance) {

		yield return new WaitForSeconds(amountToWaitBeforeActivation);

		// Raise up
		Vector3 target = tileTransform.position + (distance * Vector3.up);
		print("Rising up");

		for (float t = 0.0f; t < 0.5f; t += (Time.deltaTime / (duration))) {

			tileTransform.position = Vector3.MoveTowards(tileTransform.position, target, t);
			yield return null;

		}

		target = tileTransform.position + (distance * Vector3.down);
		print("Lowering down");

		for (float t = 0.0f; t < 0.5f; t += (Time.deltaTime / (duration))) {

			tileTransform.position = Vector3.MoveTowards(tileTransform.position, target, t);
			yield return null;

		}

	}

}