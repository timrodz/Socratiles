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
	public float smoothMove = 2.0f;
	public float duration = 1.0f;
	Transform nextTile;
	public static bool isTileMoving;
	public static bool isPlayerMoving;

	// Update is called once per frame
	void Update() {

		if (!isTileMoving) {
			ProcessInput();
		}

		StayOnTile();

	}

	void ProcessInput() {

		// Move left because the camera is positioned at y: 225
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			Turn(Direction.Backwards);
			StartCoroutine(MoveForward());
		}

		if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
			Turn(Direction.Forward);
			StartCoroutine(MoveForward());
		}

		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
			Turn(Direction.Left);
			StartCoroutine(MoveForward());
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
			Turn(Direction.Right);
			StartCoroutine(MoveForward());
		}

	}

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
			
			Vector3 targetPosition = new Vector3(nextTile.position.x, transform.position.y, nextTile.position.z);

			isPlayerMoving = true;

			yield return new WaitForSeconds(0.0f);
			for (float t = 0.0f; t < 0.5f; t += (Time.deltaTime / duration)) {

				if (!isPlayerMoving) {
					print("Encountered a trigger");
					transform.position = targetPosition;
//					isPlayerMoving = true;
					yield break;
				}
				
				transform.position = Vector3.MoveTowards(transform.position, targetPosition, t);
				yield return null;

			}

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
			transform.parent = nextTile;
		}

	}

}