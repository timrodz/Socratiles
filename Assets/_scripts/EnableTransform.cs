using UnityEngine;
using System.Collections;

public class EnableTransform : MonoBehaviour {

	public Transform transformObject;

	public void EnableObject() {

		transformObject.gameObject.SetActive(true);

	}

}