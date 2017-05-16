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

    void Start()
    {
       
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
        transform.LookAt(target);
		
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
