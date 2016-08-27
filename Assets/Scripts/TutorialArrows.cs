using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialArrows : MonoBehaviour {

    public Image topRight;
    public Image topLeft;
    public Image btmRight;
    public Image btmLeft;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {


        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(FadeOut(topLeft));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(FadeOut(topRight));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(FadeOut(btmRight));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(FadeOut(btmLeft));
        }
    }

    IEnumerator FadeOut(Image img)
    {
        float alpha = img.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.9f)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0, t));
            img.color = newColor;
            yield return null;
        }

    }
}
