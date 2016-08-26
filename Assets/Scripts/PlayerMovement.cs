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

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {


        ProcessInput();
        StayOnTile();

    }

    void ProcessInput()
    {
        // Move left because the camera is positioned at y: 225
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Turn(Direction.Backwards);
            MoveForward();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Turn(Direction.Forward);
            MoveForward();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Turn(Direction.Left);
            MoveForward();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Turn(Direction.Right);
            MoveForward();
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

	void MoveForward() {
		
		if (DetectPath(transform.forward)) {
			transform.position = new Vector3(nextTile.position.x, transform.position.y, nextTile.position.z);
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

    void StayOnTile()
    {
        transform.parent = nextTile.parent;
    }

}
