﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSystem3dTrees : MonoBehaviour
{
    public Segment3dTrees[] segments;
    public int childcount = 0;
    public Transform target = null;

    public bool isReaching = false;
    public bool isDragging = false;

    private Segment3dTrees lastSegment = null;
    private Segment3dTrees firstSegment = null;

    private bool wasDragging = false;
    private bool wasReaching = false;
 

    // Use this for initialization
    void Awake()
    {

        //lets buffer our segements in an array
        childcount = transform.childCount;
        segments = new Segment3dTrees[childcount];
        int i = 0;
        foreach (Transform child in transform)
        {
            segments[i] = child.GetComponent<Segment3dTrees>();
            i++;
        }


        firstSegment = segments[0];
        lastSegment = segments[childcount - 1];

		//Debug.Log(firstSegment.transform.name);
		//Debug.Log(lastSegment.transform.name);
	}

    // Update is called once per frame
    void Update()
    {


        if ((wasDragging && !isDragging) || ( isReaching && wasDragging) )
        {
            isDragging = false;    //if reaching brought use here, we should reset.
            wasDragging = false;

            transform.position = firstSegment.transform.position;
            firstSegment.transform.position = transform.position;


            
			//do one reach cycle to get things in a nice state
            lastSegment.reach(target.position);
            firstSegment.transform.position = transform.position;
            firstSegment.updateSegmentAndChildren();
                        

        }
        
        if (isDragging)
        {
            
            lastSegment.drag(target.position);
            wasDragging = true;
        }       
        else if (isReaching)
        {


            wasReaching = true;

            //call reach on the last
            lastSegment.reach(target.position);
            
            //and forward update on the first
            //we needed to maintain that first segment original position
            //which is the position of the IK system itself
            firstSegment.transform.position = transform.position;
            firstSegment.updateSegmentAndChildren();
            
        }

        
    }
}

