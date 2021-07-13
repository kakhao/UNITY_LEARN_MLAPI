using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript SingleTone;
    public Transform target;

    public float smoothSpeed = 1000f;
    public Vector3 offsetA = new Vector3(0,2,-3);
    public Vector3 LookAtOffset=new Vector3(0,1,0);
    public Vector3 offsetB;
    public bool modeB = false;


    // Start is called before the first frame update
    void Start()
    {
        SingleTone = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (target!=null)
        {
            Vector3 dest;
            if (modeB)
            {
                dest = target.position + offsetB;
            }
            else
            {
                dest = target.TransformPoint(offsetA);
            }

            transform.position = Vector3.Lerp(transform.position, dest, Time.deltaTime * smoothSpeed);
            transform.LookAt(target.position + LookAtOffset);

        }
    }
}
