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
	public float interpRate = 1.0f;

	//keep a reference to the character's forward facing
	//and the thing that actually owns me
	public Transform hips;
	
	//compensation/clamping values on each axis
    public float Xcomp = 0;
    public float Ycomp = 0;
	public float Zcomp = 0;
	public float Xrange = 1.0f;
	public float Yrange = 0.5f;
	public float Zrange = 0.2f;

	public float Xaccum = 0;

	//dot product to the next joint
	public float dotNext;

	public bool isRight = false;
	public bool isTerminus = false;

	//if interpolated
	private Vector3 goalEuler;
	private float lastx = 0;

	private void Awake()
	{
		//calculate the length from me to my child, based on my initial
		//pivot positions that we manually manipulated in the editor
		if (child)
		{
			length = Vector3.Distance(transform.position, child.transform.position);
		}

	}
    void Start()
    {
		//WARNING: If you don't have a start method, you don't get the enabled box in the inspector! 	

    }

    public void updateSegmentAndChildren()
    {

        updateSegment();

        //update its children!!!
        if (child)
            child.updateSegmentAndChildren();
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
        transform.LookAt(target);

		Quaternion rot = transform.localRotation;
		Vector3 euler = rot.eulerAngles;

		float x = euler.x;
		float z = euler.z;
		float y = euler.y;

		float xt = x ;
		if (xt > 180)
			xt -= 360;

		if (parent )
		{
			if (xt < 0)
			{
				float d = lastx - xt;
				x +=  (d/2);
				parent.Xaccum = -d/2;
			}
			else
				lastx = x;
		}
			

		//left or right leg invert the clamp/z rot
		//perhaps a setable value per joint?
		euler.Set(x * Xrange - Xcomp + Xaccum, y * Yrange - Ycomp, z * Zrange - Zcomp);

		if (interpolate)
		{
			goalEuler = euler;			//set the goal rotation
			euler = rot.eulerAngles;    //reset the current rotation
			euler = Vector3.Slerp(euler, goalEuler, Time.deltaTime * interpRate);
		}

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
}
