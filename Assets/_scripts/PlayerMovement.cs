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

    public float duration = 1.0f;
	public static bool isTileMoving;
	public static bool isPlayerMoving;
	private bool canMove = true;

	// Update is called once per frame
	void Update() {

		transform.position = new Vector3(
			(float)System.Math.Round(transform.position.x, 2),
			(float)System.Math.Round(transform.position.y, 2),
			(float)System.Math.Round(transform.position.z, 2)
		);

		if (!isTileMoving && canMove) {
			
			ProcessInput();

		} else {
			StayOnTile();
		}

	}

	void ProcessInput() {

		// Move left because the camera is positioned at y: 225
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Q)) {
			Turn(Direction.Backwards);
			StartCoroutine(MoveForward());
		}

		if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.C))
        {
			Turn(Direction.Forward);
			StartCoroutine(MoveForward());
		}

		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.W)){
            Turn(Direction.Left);
			StartCoroutine(MoveForward());
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Z)) {
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

			canMove = false;
			isPlayerMoving = true;
            GetComponentInChildren<Animator>().SetBool("isMoving", true);

			Vector3 targetPosition = new Vector3(nextTile.position.x, transform.position.y, nextTile.position.z);

			yield return new WaitForSeconds(0.0f);
			for (float t = 0.0f; t < 0.5f; t += (Time.deltaTime / duration)) {

				if (!isPlayerMoving) {
					transform.position = targetPosition;
					canMove = true;
					yield break;
				}
				
				transform.position = Vector3.MoveTowards(transform.position, targetPosition, t);
				yield return null;

			}

			transform.position = targetPosition;

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
            GetComponentInChildren<Animator>().SetBool("isMoving", false);
            transform.parent = nextTile;
		}

	}

}