using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Segment3dTrees : MonoBehaviour
{
    public Vector3 Apos = new Vector3(0, 0, 0);
    public Vector3 Bpos = new Vector3(0, 0, 0);

    public float length = 0;

    public Segment3dTrees parent = null;
    public Segment3dTrees child = null;
    
    private Vector3 goalTarget;

    //the ik system will tell it's children who owns it
    public IKSystem3dTrees IKsys = null;
    private Quaternion initialRotation;

    void Start()
    {
       
    }
    private void Awake()
    {
        //calculate the length from me to my child, based on my initial
        //pivot positions that we manually manipulated in the editor
        if (child)
        {
            length = Vector3.Distance(transform.position, child.transform.position);
        }

    }
    public void updateSegmentAndChildren()
    {
		
		updateSegment();

		//update its children
		if (child)
		{
			child.updateSegmentAndChildren();
		}
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
    
    public void pointAt(Vector3 target)
    {

        //get current rot if we interp    
        Quaternion prot = transform.localRotation;

        //get unclamped desire rotation as a quat, an euler, and a direction;
        transform.LookAt(target);

        Vector3 sysright = IKsys.transform.right;
        Vector3 segright = transform.right;

        Vector3 sysup = IKsys.transform.forward;
        Vector3 segup = transform.forward;


        //get an alignment to the parent transform (the ik system itself)
        float aZ = Vector3.Angle(segright, sysright);
       
        //invert where needed based on the direction of the angle
        int iZ = AngleDirInt(segright, sysright, sysup);
        
        aZ *= iZ;
        
        //get my new rotation in local
        Quaternion  rot = transform.localRotation;
        Vector3 euler = rot.eulerAngles;


        float x = euler.x;
        float z = 0; // euler.z;
        float y = euler.y;

      
        euler.Set( x ,
                   y ,
                   z * aZ );

        /*
        if (interpolate)
        {
            goalEuler = euler;          //set the goal rotation
            euler = prot.eulerAngles;    //reset the current rotation
            euler = Vector3.Slerp(euler, goalEuler, Time.deltaTime * interpRate);
        }
        */

        transform.localRotation = Quaternion.Euler(euler.x, euler.y, euler.z);

    }

    public void drag(Vector3 target)
    {

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

    //cross/dot product method returns v2 is "left" or "right" of v1 
    int AngleDirInt(Vector3 v1, Vector3 v2, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(v1, v2);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1;
        }
        else if (dir < 0f)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
