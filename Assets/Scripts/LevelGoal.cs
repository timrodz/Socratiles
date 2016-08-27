using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour {

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	/// <summary>
	/// Loads the next level.
	/// </summary>
	void OnEnable() {

		print("Loading next level");

		// Load the next level //
		Transform level = GameObject.Find("PLAYER").GetComponent<Transform>().parent.parent.parent;

		// Get the current level's string and load the next level based on hierarchy
		string str = TrimString(level.ToString(), true);

		// Find the level prefab by loading the resources directly
		Object nextLevel = Resources.Load(str);

		// If there's a next level, create it
		if (nextLevel) {

			print("Found the next level");

			Transform player = GameObject.Find("PLAYER").GetComponent<Transform>().parent.parent;

			player.SetParent(null);

			Destroy(level.gameObject);

			StartCoroutine(LoadNextLevel(player, nextLevel));

			print("Done loading level");

		}
		// Otherwise, go back to the menu
		else {

			SceneManager.LoadScene("Level Selection");

		}

	}

	private IEnumerator LoadNextLevel(Transform player, Object levelObject) {

		print("Before");

		for (int i = 0; i <= 2; i++) {

			GameObject.Find("Camera Target").GetComponent<Transform>().position = new Vector3(0, -27, 0);
			player.position = new Vector3(0, -27, 0);
			yield return null;

		}

		CreateLevel(player, levelObject);

		print("Starting");

		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / 0.95f)) {
			
			player.position = Vector3.MoveTowards(player.position, Vector3.zero, t);
			yield return null;

		}

		// Round the transform's position
		player.position = Vector3.zero;

//		yield return new WaitForSeconds(1.0f);
		print("Destroying the winning trigger");

		Destroy(this.gameObject);

	}

	void CreateLevel(Transform _t, Object _o) {
		
		GameObject levelToInstantiate = Instantiate(_o, Vector3.zero, new Quaternion(0, 0, 0, 1)) as GameObject;
		levelToInstantiate.name = TrimString(_o.ToString(), false);
		_t.SetParent(levelToInstantiate.GetComponent<Transform>());

	}

	/// <summary>
	/// Trims the string.
	/// _containsNumber works in two ways for false:
	/// 1: The user wants to grab any string and trim it
	/// 2: The user wants to reset the level
	/// </summary>
	string TrimString(string _stringToTrim, bool _containsNumber) {

		print(">>> Trimming the string + " + _stringToTrim);

		int length = _stringToTrim.Length - 1;

		// Find the first parentheses and delete everything
		// that follows after it
		int index = _stringToTrim.IndexOf('(');
		_stringToTrim = _stringToTrim.Remove(index - 1, length - index + 2);
		print(_stringToTrim);

		// TODO: Modify this section so it takes numbers higher than 9

		if (_containsNumber) {

			// Recalculate the new length
			length = _stringToTrim.Length - 1;
			print(length);

			// store the number it has and remove it
			string number = Regex.Match(_stringToTrim, @"\d+").Value;

			// parse the number from the string and add 1 to it
			int num = int.Parse(number);
			num++;

			// TODO: Count the amount of digits that the number has

			// Remove the number at the top
			_stringToTrim = _stringToTrim.Remove(length);

			_stringToTrim += (num);

		}

		print(_stringToTrim);

		return _stringToTrim;

	}

}