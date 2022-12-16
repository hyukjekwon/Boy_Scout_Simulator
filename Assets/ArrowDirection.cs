using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDirection : MonoBehaviour
{
    private float xAngle;
    private float zAngle;

    // Start is called before the first frame update
    void Start()
    {
        xAngle = transform.localRotation.eulerAngles.x;
        zAngle = transform.localRotation.eulerAngles.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Set's the compass rotation to be the rotation of the First Person Controller rotation
        transform.localRotation = Quaternion.Euler(xAngle,  transform.root.gameObject.transform.localRotation.eulerAngles.y-180, zAngle);
    }
}
