using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinRootState : RootState {

   
    //<JPK> @Espen made public and now exist only for goblins
    public Transform ball;
    public Collider ballCollider;
    public SpinState spinstate;

    
    // Use this for initialization prior to anything else happening
    void Awake () {


        //EXAMPLE CODE: buffer a pointer to some transform we may need
        //GetComponent is expensive (particularly if we get by name)
        //so we get all the things we need in advance. For some reason
        //unity does not have its own "find by name" so we make our own
        //"recursive" search

        //we may want to find a transform, lets say "TARGETS"
        m_someSpecificTransform = Search(transform, "TARGETS");

        targetManager = new TargetManager(this, m_someSpecificTransform); 

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

        //<JPK> made spin state public
        spinstate = new SpinState(this);
        walkbackstate.addChildState(spinstate);

		DodgeState dodgestate = new DodgeState(this);
		spinstate.addChildState(dodgestate);

        StunState stunstate = new StunState(this);
        dodgestate.addChildState(stunstate);

        //add more states here...
        spinstate.soundtarget = Search(transform, "SoundObject");
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

  
}
