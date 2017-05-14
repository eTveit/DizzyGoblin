using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this one is the "generic" state, all others inherit from, included is a
//"dummy" state to be used as a template for new states, in their own files please!!
public abstract class StateNode {
    
    //the list of child states
    protected List<StateNode> m_childStates;

    //is it currently in this state?
    public bool p_isInState;
    
    //is it's "animation" currently playing
    protected bool m_isDoingItsState = false;

    //we probably want a references to some specific things    
    protected RootState m_rootState;        //the root state that owns me
    protected Transform m_transform;        //the root state's transform
    protected GameObject m_gameObject;      //the transform's game object

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

    //this searches for a Transform specifically
    //TODO: make it search for anything, and cast it when it returns 
    public Transform SearchTransform(Transform target, string name)
    {
        if (target.name == name) return target;

        for (int i = 0; i < target.childCount; i++)
        {
            //we use "var" because the component could be anything
            var result = SearchTransform(target.GetChild(i), name);

            if (result != null) return result;
        }

        return null;
    }


}

//below is a "Dummy State" you can copy and paste from, 
//to start building your new state
public class DummyState : StateNode
{
    //this is the constructor, it gets called any time you use 
    //the "new" keyword to instantiate the state
    public DummyState(RootState _rs)
    {
        m_childStates = new List<StateNode>();
        m_rootState = _rs;
        m_transform = m_rootState.transform;
        m_gameObject = m_transform.gameObject;
    }

    public override bool advanceTime(float dt)
    {


        if (advanceState(dt) == true)
        {
            //if any child state is true, I am false
            p_isInState = false;
            m_isDoingItsState = false;

            //since a child state is true, return this fact!
            return true;
        }

        //if no child state is true, see if I am true

        //lets just say I am true to begin with
        p_isInState = true;

        //if some condition - here we simply use a key press
        //the condition can be potentially anything, you need only
        //devise a way to feed that condition to the state. The state
        //could "examine the world" from within (meaning here in code)
        //or some external script could tell it to "do its thing"
        if (Input.GetKey(KeyCode.C))
        {
            p_isInState = true;
        }
        else
        {
            p_isInState = false;
            m_isDoingItsState = false;
        }

        if (p_isInState)
        {
            Debug.Log("DUMMY STATE");
            if (!m_isDoingItsState)
            {
                m_isDoingItsState = true;
                //Do something
            }
        }
        return p_isInState;
    }
}
