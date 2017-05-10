using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this one is the "generic" state all others inherit from
public abstract class StateNode {
    
    //the list of child states
    protected List<StateNode> m_childStates;

    //is it currently in this state?
    public bool p_isInState;
    //is it's animation currently playing
    protected bool p_isAnimPlaying = false;

    public Animation m_anim;

    public void addChildState(StateNode state)
    {

        m_childStates.Add(state);

    }
     
    public abstract bool advanceTime(float dt);

    protected bool advanceState(float dt)
    {
        foreach (var child in m_childStates)
        {
            if (child.advanceTime(dt) == true)
            {
                //if any child is true, stop iterating and exit                
                return true;
            }
        }

        //if no child state is true, return false
        return false;
    }

}

//our first "concrete" state
public class IdleState : StateNode
{


    float m_idleChooseTimer = -1;

    public IdleState(Animation _anim)
    {
        m_childStates = new List<StateNode>();
        m_anim = _anim;
    }


    public override bool advanceTime(float dt)
    {

        //if any child state is true, set my state and return
        //do not continue to process state tree (this can be overridden if desired)
        if (advanceState(dt) == true)
        {
            //if any child state is true, I am false
            p_isInState = false;
            p_isAnimPlaying = false;

            //since a child state is true, return this fact!
            return true;
        }

        //lets just say I am true, which in fact I always am if none of my children are true
        //as IDLE is the first state under root
        p_isInState = false;  //diable for now

        if (p_isInState)
        {
            if(m_idleChooseTimer == -1)
                m_idleChooseTimer = Time.time;

            if (!p_isAnimPlaying || Time.time - m_idleChooseTimer > 3 )
            {
                
                Debug.Log("IDLE STATE");
                if( Random.Range(0.0f,1.0f) < 0.5f)
                    m_anim.CrossFade("Idle_L");
                else
                    m_anim.CrossFade("Idle2_L");
                
                //reset timer
                m_idleChooseTimer = -1;

                p_isAnimPlaying = true;
                
            }
        }
        
        return p_isInState;
    }


}

public class WalkState : StateNode
{
    
    public WalkState(Animation _anim)
    {
        m_childStates = new List<StateNode>();
        m_anim = _anim;
    }

    public override bool advanceTime(float dt)
    {
               
        
        if( advanceState(dt) == true )
        {
            //if any child state is true, I am false
            p_isInState = false;
            p_isAnimPlaying = false;

            //since a child state is true, return this fact!
            return true;
        }

        //if no child state is true, see if I am true

        //lets just say I am true to begin with
        p_isInState = true;

        if (Input.GetKey(KeyCode.W))
        {
            p_isInState = true;
        }
        else
        {
            p_isInState = false;
            p_isAnimPlaying = false;
        }

        if (p_isInState)
        {
            Debug.Log("WALK STATE");
            if (!p_isAnimPlaying)
            {
                p_isAnimPlaying = true;
                m_anim.CrossFade("Walk_L");
            }
        }
        return p_isInState;
    }


}

public class WalkBackState : StateNode
{

    public WalkBackState(Animation _anim)
    {
        m_childStates = new List<StateNode>();
        m_anim = _anim;
    }

    public override bool advanceTime(float dt)
    {


        if (advanceState(dt) == true)
        {
            //if any child state is true, I am false
            p_isInState = false;
            p_isAnimPlaying = false;

            //since a child state is true, return this fact!
            return true;
        }

        //if no child state is true, see if I am true

        //lets just say I am true to begin with
        p_isInState = true;

        if (Input.GetKey(KeyCode.S))
        {
            p_isInState = true;
        }
        else
        {
            p_isInState = false;
            p_isAnimPlaying = false;
        }

        if (p_isInState)
        {
            Debug.Log("WALK BACK STATE");
            if (!p_isAnimPlaying)
            {
                p_isAnimPlaying = true;
                m_anim.CrossFade("WalkBack_L");
            }
        }
        return p_isInState;
    }


}


