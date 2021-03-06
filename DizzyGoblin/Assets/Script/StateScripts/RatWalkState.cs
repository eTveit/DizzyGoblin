﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RatWalkState : StateNode
{

    RatRootState m_ratRootState;

    ET_ratArmsIdle leftArm;
    ET_ratArmsIdle rightArm;
    ET_ratLegsWalk leftLeg;
    ET_ratLegsWalk rightLeg;
    ET_ratSpineWalk spine;
    ET_ratTailIdle tail;

    public RatWalkState(RatRootState _rs)
    {
        m_childStates = new List<StateNode>();
        m_rootState = _rs;
        m_ratRootState = _rs;

        m_transform = m_rootState.transform;
        m_gameObject = m_transform.gameObject;
        
        leftArm = m_ratRootState.leftArm.GetComponent<ET_ratArmsIdle>();
        rightArm = m_ratRootState.rightArm.GetComponent <ET_ratArmsIdle>();
        leftLeg = m_ratRootState.leftFoot.GetComponent<ET_ratLegsWalk>();
        rightLeg = m_ratRootState.rightFoot.GetComponent<ET_ratLegsWalk>();
        spine = m_ratRootState.spine.GetComponent<ET_ratSpineWalk>();
        tail = m_ratRootState.tail.GetComponent<ET_ratTailIdle>();


    }

    public override bool advanceTime(float dt)
    {


        if (advanceState(dt) == true)
        {
            //if any child state is true, I am false
            p_isInState = false;
            m_isDoingItsState = false;
            setEnabled(false);

            //since a child state is true, return this fact!
            return true;
        }

        //if no child state is true, see if I am true

        //check the velocity of the rat to see if walking should be enabled
        if (m_ratRootState.steer.velocity.magnitude > 0.5f)
        {
            p_isInState = true;
        }
        else
        {
            p_isInState = false;
            m_isDoingItsState = false;
            setEnabled(false);
        }

        if (p_isInState)
        {
            if (!m_isDoingItsState)
            {
                Debug.Log("RAT WALK STATE");

                m_isDoingItsState = true;

                setEnabled(true);
            }
        }

        return p_isInState;
    }


    void setEnabled(bool enable)
    {

        leftArm.enabled = enable;
        rightArm.enabled = enable;
        leftLeg.enabled = enable;
        rightLeg.enabled = enable;
        spine.enabled = enable;
        tail.enabled = enable;

    }

}

