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

	
	//compensation/clamping values on each axis
    public float Xcomp = 0;
    public float Ycomp = 0;
	public float Zcomp = 0;
	public float Xrange = 1.0f;
	public float Yrange = 1.0f;
	public float Zrange = 1.0f;


    //the min and max euler values in each axis
    public Vector3 minEulers;
    public Vector3 maxEulers;



    //so we can see what is going on
    public Quaternion rot;
    public Vector3 euler;
    public Vector3 fwd;


    public float Xaccum = 0;

	//dot product to the next joint
	public float dotNext;

	public bool isRight = false;


    //terminus is a dummy segement used only to find the length of the
    //true last segement such as the case for the foot or hand, it maintains
    //consistent angular relationship with it's parent, unless its own 
    //component tells it otherwise
    public bool isTerminus = false;

	//if interpolated
	private Vector3 goalEuler;
	private float lastx = 0;


    //the ik system will tell it's children who owns it
    public IKSystem3d IKsys = null;  
    private Quaternion initialRotation;

	private void Awake()
	{
		//calculate the length from me to my child, based on my initial
		//pivot positions that we manually manipulated in the editor
		if (child)
		{
			length = Vector3.Distance(transform.position, child.transform.position);
		}

        //get the intital rotation as was mapped to the skinned model
        initialRotation = transform.localRotation;



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

        
        //get current rot if we interp    
        Quaternion prot = transform.localRotation;

        //get unclamped desire rotation as a quat, an euler, and a direction;
        transform.LookAt(target);
    
        Vector3 fwd = transform.forward; //its forward in world
        Vector3 right = transform.right; //its perp in world

        //get an alignment to the parent transform (the ik system itself)
        float alignZ = Vector3.Angle(right, IKsys.transform.right);
        float alignX = 0; //Vector3.Angle(right, IKsys.transform.right);
        float alignY = 0; //Vector3.Angle(right, IKsys.transform.right);

        //get my new rotation in local
        rot = transform.localRotation;
        euler = rot.eulerAngles;
        
        
        float x = euler.x;
		float z = euler.z;
		float y = euler.y;

		float xt = x;
        float yt = y;
        float zt = z;

        //convert to rangable values
        if (xt > 180)
			xt -= 360;

        if (yt > 180)
            yt -= 360;
        
        if (zt > 180)
            zt -= 360;

        if (isTerminus)
        {
           //maybe something else        
        }

        
        /*
        //pass contraint corrections to parent, see if he can handle it
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
        */

        //apply alignment and compensation values to each limb
        euler.Set(x  - Xcomp - alignX , y - Ycomp - alignY , z - Zcomp - alignZ);

        if (interpolate)
		{
			goalEuler = euler;			//set the goal rotation
			euler = prot.eulerAngles;    //reset the current rotation
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
