using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    struct TriggerInformation
    {
        public int numTriggers;

        public void Start()
        {
            numTriggers = 1;
        }

        public void TriggerDetect()
        {

        }
    }

    TriggerInformation triggerInformation;

    List<Transform> nodeChildren;
    List<Transform> triggerChildren;

    bool nodeDetected;
    bool rotating;
    GameObject detectedNode;

    private float rotationDegreesPerSecond = 45f;
    private float rotationDegreesAmount = 90f;
    private float totalRotation = 0;

    void Start()
    {
        triggerInformation.Start();

        nodeDetected = false;
        rotating = false;
        nodeChildren = new List<Transform>();
        triggerChildren = new List<Transform>();
        detectedNode = new GameObject();


        for (int i = 0; i < transform.childCount; ++i)
        {
            if(transform.GetChild(i).name == "Trigger" )
            {
                for(int j = 0; j < transform.GetChild(i).childCount; ++j)
                {
                    triggerChildren.Add(transform.GetChild(i).GetChild(j));
                }
            }
            nodeChildren.Add(transform.GetChild(i));
        }

        // Debug.Log(triggerChildren.Length);

        for(int i = 0; i < triggerChildren.Count; ++i)
        {
            if (triggerChildren[i].name == "TriggerDirection")
                triggerChildren[i].GetComponent<Renderer>().material.color = Color.green;
        }
    }

    void Update()
    {
        for (int i = 0; i < triggerChildren.Count; ++i)
        {
            if (triggerChildren[i].name == "TriggerDirection")
                if(triggerInformation.numTriggers <= 0)
                    triggerChildren[i].GetComponent<Renderer>().material.color = Color.red;
                else
                    triggerChildren[i].GetComponent<Renderer>().material.color = Color.green;
        }

        if(nodeDetected && triggerInformation.numTriggers > 0)
        {
            if (Mathf.Abs(totalRotation) < Mathf.Abs(rotationDegreesAmount))
            {
                Rotate();
                rotating = true;
            }
            else
            {
                rotating = false;
                nodeDetected = false;
                triggerInformation.numTriggers--;
            }
        }
    }

    void Rotate()
    {
        float currentAngle = detectedNode.transform.rotation.eulerAngles.y;
        detectedNode.transform.rotation = Quaternion.AngleAxis(currentAngle + (Time.deltaTime * rotationDegreesPerSecond), Vector3.up);
        totalRotation += Time.deltaTime * rotationDegreesPerSecond;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Platform")
        {
            if(!rotating)
            {
                nodeDetected = true;
                detectedNode = other.transform.parent.gameObject;

                rotationDegreesAmount = 90.0f;

            }
        }   
    }
}
