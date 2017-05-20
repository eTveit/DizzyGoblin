using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootState : MonoBehaviour {



    public Transform leftFoot;
    public Transform rightFoot;
    public Transform leftArm;
    public Transform rightArm;
    public Transform spine;
    public Transform ball;
    public Transform Avatar;

    protected List<StateNode> m_childStates ;

    //to find and hold a game object by name - makes it easy to access
    //specific children at build-time
    protected Transform m_someSpecificTransform;

    public TargetManager targetManager = null;
    
    // Use this for initialization prior to anything else happening
    void Awake () {


        //EXAMPLE CODE: buffer a pointer to some transform we may need
        //GetComponent is expensive (particularly if we get by name)
        //so we get all the things we need in advance. For some reason
        //unity does not have its own "find by name" so we make our own
        //"recursive" search

        //we may want to find a transform, lets say "TARGETS"
        m_someSpecificTransform = Search(transform, "TARGETS");

        targetManager = new TargetManager(m_someSpecificTransform); 

        m_childStates = new List<StateNode>();

        //now lets build some states:
        //we must always add at least one, if we want the graph to run
        //here we pass the root state of our game object,
        //from that we can indeed get anything. we use the keyword "this"
        //meaning, "this" root state 
        IdleState idlestate = new IdleState(this);
        m_childStates.Add(idlestate);

        WalkState walkstate = new WalkState(this);
        idlestate.addChildState(walkstate);

        WalkBackState walkbackstate = new WalkBackState(this);
        walkstate.addChildState(walkbackstate);

        SpinState spinstate = new SpinState(this);
        walkbackstate.addChildState(spinstate);

		DodgeState dodgestate = new DodgeState(this);
		spinstate.addChildState(dodgestate);

        StunState stunstate = new StunState(this);
        dodgestate.addChildState(stunstate);

		//add more states here...

	}

    void Start()
    {
        //place the dude
        transform.position.Set(5, 0, 5);
    }

    // Update is called once per frame
    void Update ()
    {
        
        //iterate the child states, calling advanceTime
        foreach (StateNode child in m_childStates)
        {
            child.advanceTime(Time.deltaTime);
        }
        
    }

    //here we will override any keyframe animations on specific nodes
    void LateUpdate()
    {


    }

    
    //this searches for a Transform specifically
    //TODO: make it search for anything, and cast it when it returns 
    public Transform Search(Transform target, string name)
    {
        if (target.name == name) return target;

        for (int i = 0; i < target.childCount; i++)
        {
            //we use "var" because the component could be anything
            var result = Search(target.GetChild(i), name);

            if (result != null) return result;
        }

        return null;
    }

}

public class TargetManager
{
    protected List<Transform> m_Targets;
    public int targetCount = 0;
    
    //ctor
    public TargetManager(Transform _targets)
    {
        //get all of our targets, and put them into an easily accessable list
        targetCount = _targets.childCount;

        //with my own list I can access by index though this might not be needed
        m_Targets = new List<Transform>();  

        foreach(Transform t in _targets)
        {
            m_Targets.Add(t);
        }
    }
    
    public void disableAllTargetAnimations()
    {
        //iterate each target object and disable its IKAS
        foreach( Transform target in m_Targets)
        {
            IKAnimationTarget[] IKAS = target.GetComponents<IKAnimationTarget>() as IKAnimationTarget[];
            foreach (IKAnimationTarget ika in IKAS)
            {
                ika.enabled = false;
            }
        }

    }

    //needs the name of the target and the name of the component
    public IKAnimationTarget getTargetByName(string target_name, string anim_name)
    {
        foreach (Transform target in m_Targets)
        {
            if (target.name == target_name)
            {
                IKAnimationTarget[] IKAS = target.GetComponents<IKAnimationTarget>() as IKAnimationTarget[];
                foreach (IKAnimationTarget ika in IKAS)
                {
                    if (ika.getAnimName() == anim_name)
                        return ika;
                }
            }
        }
        return null;
    }
    public void disableTargetAnimation(string anim_name)
    {
        //iterate each target object
        foreach (Transform target in m_Targets)
        {
            IKAnimationTarget[] IKAS = target.GetComponents<IKAnimationTarget>() as IKAnimationTarget[];
            foreach (IKAnimationTarget ika in IKAS)
            {

                if (ika.getAnimName() == anim_name) 
                {
                    ika.enabled = false;
                }
                
            }
        }

    }

    public void enableTargetAnimation(string anim_name)
    {
        //iterate each target object
        foreach (Transform target in m_Targets)
        {
            IKAnimationTarget[] IKAS = target.GetComponents<IKAnimationTarget>() as IKAnimationTarget[];
            foreach (IKAnimationTarget ika in IKAS)
            {

                Debug.Log(ika.name);
                Debug.Log(ika);
                
                if (ika.getAnimName() == anim_name)
                    ika.enabled = true;

            }
        }

    }

}
