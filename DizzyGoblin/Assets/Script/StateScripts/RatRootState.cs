using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatRootState : RootState
{

    //root state will offer-up the steering handler to all child states.
    public steering steer;
    public Transform tail;

    // Use this for initialization prior to anything else happening
    void Awake()
    {


        //EXAMPLE CODE: buffer a pointer to some transform we may need
        //GetComponent is expensive (particularly if we get by name)
        //so we get all the things we need in advance. For some reason
        //unity does not have its own "find by name" so we make our own
        //"recursive" search

        //we may want to find a transform, lets say "TARGETS"
        m_someSpecificTransform = Search(transform, "TARGETS");

        //and then use it some way - this is not stricly needed in our game
        //but has some advanced functionality
        targetManager = new TargetManager(this, m_someSpecificTransform);

        m_childStates = new List<StateNode>();

        //now lets build some states:
        //we must always add at least one, if we want the graph to run
        //here we pass the root state of our game object,
        //from that we can indeed get anything. we use the keyword "this"
        //meaning, "this" root state 
        
        //RatIdleState idlestate = new RatIdleState(this);
        //m_childStates.Add(idlestate);

        RatWalkState walkstate = new RatWalkState(this);
        m_childStates.Add(walkstate);
                       
        /*
        RatDodgeState dodgestate = new RatDodgeState(this);
        walkstate.addChildState(dodgestate);

        RatStunState stunstate = new RatStunState(this);
        dodgestate.addChildState(stunstate);
        */
        //add more states here...

    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
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
    
}
