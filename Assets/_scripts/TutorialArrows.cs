using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialArrows : MonoBehaviour {

	public Sprite[] arrowImages;

	public Image topRight;
	public Image topLeft;
	public Image btmRight;
	public Image btmLeft;

	// Update is called once per frame
	void Update() {
		
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {

			StartCoroutine(Highlight(topLeft));

		}
        else {

            StartCoroutine(UndoHighlight(topLeft));

        }
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			
			StartCoroutine(Highlight(topRight));

		}
        else {

            StartCoroutine(UndoHighlight(topRight));

        }
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			
			StartCoroutine(Highlight(btmRight));

		}
        else {

            StartCoroutine(UndoHighlight(btmRight));

        }
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			
			StartCoroutine(Highlight(btmLeft));

		}
        else {

            StartCoroutine(UndoHighlight(btmLeft));

        }

	}

	IEnumerator Highlight(Image img) {

		img.GetComponent<Outline>().enabled = true;

		yield return null;

	}

    IEnumerator UndoHighlight(Image img) {

		img.GetComponent<Outline>().enabled = false;

		yield return null;

	}

}