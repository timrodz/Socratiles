using UnityEngine;
using System.Collections;

public class EnableTransform : MonoBehaviour {

	public Transform T;

	public void Enable() {

		T.gameObject.SetActive(true);

	}

}