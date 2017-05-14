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
    
        Vector3 sysright = IKsys.transform.right ; 
        Vector3 segright = transform.right ;

        Vector3 sysup = IKsys.transform.forward;
        Vector3 segup = transform.forward;
        
                
        //get an alignment to the parent transform (the ik system itself)
        float alignZ =  Vector3.Angle(segright, sysright);
        float alignX = 0; //Vector3.Angle(right, IKsys.transform.right);
        float alignY = 0; //Vector3.Angle(right, IKsys.transform.right);

        Xcomp = alignX;
        Zcomp = alignZ;
        Ycomp = alignY;

        //invert where needed based on the direction of the angle
        int zc = AngleDirInt(segright, sysright, sysup);
        int xc = 1;
        int yc = 1;

        //get my new rotation in local
        rot = transform.localRotation;
        euler = rot.eulerAngles;
        
        
        float x = euler.x;
		float z = euler.z;
		float y = euler.y;

		float xt = x;
        float yt = y;
        float zt = z;

        //convert to +/- rangable values
        if (xt > 180)
			xt -= 360;

        if (yt > 180)
            yt -= 360;
        
        if (zt > 180)
            zt -= 360;

        euler.Set(x * Xrange - alignX * xc, y * Xrange - alignY * yc, z + alignZ *zc);


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

    //return angle direction as a float (useful to know "how much" to left or right)
    float AngleDirIntFloat(Vector3 v1, Vector3 v2, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(v1, v2);
        float dir = Vector3.Dot(perp, up);

        return dir;
        
    }
}
