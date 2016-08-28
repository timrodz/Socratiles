using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour
{
	Transform[] children;

	void Start ()
	{
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; ++i)
            children[i] = transform.GetChild(i);
    }

	void Update ()
	{
	    for(int i = 0; i < transform.childCount; ++i)
        {
            if(children[i].name == "TriggerDirection")
            {
                children[i].GetComponent<Renderer>().material.color = Color.green;
            }


        }
	}

}
