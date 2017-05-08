using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Segment3d : MonoBehaviour
{
    public Vector3 Apos = new Vector3(0, 0, 0);
    public Vector3 Bpos = new Vector3(0, 0, 0);

    public float length = 0;

    public Segment3d parent = null;
    public Segment3d child = null;
    public bool interpolate = false;
    public float limitX = 0;
    public float xtraX = 0;

    private Vector3 goalTarget;

    void Start()
    {
       
    }

    public void updateSegmentAndChildren()
    {

        updateSegment();

        //update its children
        if (child)
            child.updateSegment();
    }

    public void updateSegment()
    {

       
        if (parent)
        {
            Apos = parent.Bpos;         //could also use parent endpoint...
            transform.position = Apos;  //move me to Bpos (parent endpoint)
        }
        else
        {
            //Apos is always my position
            Apos = transform.position;
        }

        //Bpos is always where the endpoint will be, as calculated from length 
        calculateBpos();
    }

    void calculateBpos()
    {   
        Bpos = Apos + transform.forward * length;
    }
    
    public void pointAtInterpolated(Vector3 target)
    {
        goalTarget = target;
        Quaternion rotA = transform.rotation;  //get current
        transform.LookAt(target);              //look at target to get our goal rotation
        Quaternion rotB = transform.rotation;
                
        //slerp the rotation
        rotA = Quaternion.Slerp(rotA, rotB, Time.deltaTime * 10.0f );
        //set the rotation to the slerped value (undo the previous lookat)
        transform.rotation = rotA;

    }
    public void pointAtLimitX(Vector3 target)
    {
        //get our proposed rotation
        transform.LookAt(target);

        Quaternion rot = transform.localRotation;
        Vector3 euler = rot.eulerAngles;

        euler.Set(euler.x, 0, 0);  //we can clamp all other axes

        if (euler.x > limitX && limitX !=0 )
        {
            transform.localRotation = Quaternion.Euler(limitX + xtraX, euler.y, euler.z);
            //probably need to pass the difference up to the parent
            if (parent != null)
            {
                parent.xtraX = euler.x - limitX;
            }
        }
        else
        {

            transform.localRotation = Quaternion.Euler(euler.x + xtraX, euler.y, euler.z);
            if (parent != null)
            {
                parent.xtraX = 0;
            }
        }

    }
    public void pointAt(Vector3 target)
    {
        transform.LookAt(target);
       
    }

    public void drag(Vector3 target)
    {
        if (interpolate)
            pointAtInterpolated(target);
        else if (limitX > 0)
            pointAtLimitX(target);
        else
            pointAt(target);


        transform.position = target - transform.forward * length;

        if (parent)
            parent.drag(transform.position);


    }

    public void reach(Vector3 target)
    {
        drag(target);
        updateSegment();
    }
}
