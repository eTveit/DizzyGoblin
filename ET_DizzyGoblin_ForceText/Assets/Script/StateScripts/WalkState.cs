using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WalkState : StateNode
{

    public WalkState(RootState _rs)
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

        if (Input.GetKey(KeyCode.W))
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
            Debug.Log("WALK STATE");
            if (!m_isDoingItsState)
            {
                m_isDoingItsState = true;
                //do something
            }
        }
        return p_isInState;
    }


}

