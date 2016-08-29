using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	enum Direction {
		Forward,
		Backwards,
		Left,
		Right
	}

	float verticalAxis;
	float horizontalAxis;
    Transform nextTile;

	public static bool isTileMoving;
	public static bool isPlayerMoving;
	private bool canMove = true;
	private float duration = 1.0f;

	// Update is called once per frame
	void Update() {

		ProcessAnimationStates();

		if (!isTileMoving && canMove) {
			ProcessInput();
		} 
		else {
			StayOnTile();
		}

		transform.position = new Vector3(
			(float)System.Math.Round(transform.position.x, 2),
			(float)System.Math.Round(transform.position.y, 2),
			(float)System.Math.Round(transform.position.z, 2)
		);

	}

	/// <summary>
	/// Process the input of the player
	/// </summary>
	void ProcessInput() {

		// Move left because the camera is positioned at y: 225
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			Turn(Direction.Backwards);
			StartCoroutine(MoveForward());
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
			Turn(Direction.Forward);
			StartCoroutine(MoveForward());
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)){
            Turn(Direction.Left);
			StartCoroutine(MoveForward());
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
			Turn(Direction.Right);
			StartCoroutine(MoveForward());
		}

	}

	/// <summary>
	/// Detects the next waypoint to move to
	/// </summary>
	/// <param name="direction"> The direction vector to check the path at</param>
	/// <returns>true if detected a path, otherwise return false</returns>
	bool DetectPath(Vector3 direction) {

		RaycastHit hit;

		if (Physics.Raycast(transform.position, direction, out hit, 10)) {
			
			if (hit.collider.gameObject.CompareTag("TilePath")) {

				nextTile = hit.collider.GetComponent<Transform>();
				return true;

			}

		}

		return false;

	}

	public IEnumerator MoveForward() {

		if (DetectPath(transform.forward)) {

			canMove = false;
			isPlayerMoving = true;

			GetComponentInChildren<Animator>().SetBool("isMoving", true);
            
			Vector3 targetPosition = new Vector3(nextTile.position.x, transform.position.y, nextTile.position.z);

			while (transform.position != targetPosition) {

				transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 2.5f);
				yield return null;

			}
			// yield return new WaitForSeconds(0.0f);
			// for (float t = 0.0f; t < duration; t += (Time.deltaTime)) {

			// 	if (!isPlayerMoving) {
			// 		transform.position = targetPosition;
			// 		canMove = true;
			// 		yield break;
			// 	}
				
			// 	transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
			// 	yield return null;

			// }

			GetComponentInChildren<Animator>().SetBool("isMoving", false);
			transform.position = targetPosition;
			print("Player stopped moving");

			canMove = true;
			isPlayerMoving = false;

			

        }

	}

	void Turn(Direction direction) {

		switch (direction) {
			case Direction.Forward:
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
				break;
			case Direction.Backwards:
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, 0);
				break;
			case Direction.Left:
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90, 0);
				break;
			case Direction.Right:
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, 0);
				break;
			default:
				break;
		}

	}

	void StayOnTile() {
		
		if (nextTile) {
			// print("Reached tile");
            // GetComponentInChildren<Animator>().SetBool("isMoving", false);
            transform.parent = nextTile;
		}

	}

	void ProcessAnimationStates() {

		if (!canMove) {

			// GetComponentInChildren<Animator>().SetBool("isMoving", true);

		}
		else {

			// GetComponentInChildren<Animator>().SetBool("isMoving", false);

		}

	}

}