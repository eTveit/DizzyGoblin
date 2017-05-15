using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSystem3d : MonoBehaviour
{
    public Segment3d[] segments;
    public int childcount = 0;
    public Transform target = null;

    public bool isReaching = false;
    public bool isDragging = false;

    private Segment3d lastSegment = null;
    private Segment3d firstSegment = null;

    private bool wasDragging = false;
    private bool wasReaching = false;
 

    // Use this for initialization
    void Awake()
    {

        //lets buffer our segements in an array
        childcount = transform.childCount;
        segments = new Segment3d[childcount];
        int i = 0;
        foreach (Transform child in transform)
        {
            segments[i] = child.GetComponent<Segment3d>();
            segments[i].IKsys = this;
            i++;
        }


        firstSegment = segments[0];
		//terminus is a dummy segement used only to find the length of the
		//true last segement such as the case for the foot or hand, it maintains
        //consistent angular relationship with it's parent, unless its own 
        //component tels it otherwise
		if (segments[childcount - 1].isTerminus)    
			lastSegment = segments[childcount - 2];
		else
			lastSegment = segments[childcount - 1];
		  
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

