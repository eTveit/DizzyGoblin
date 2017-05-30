using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KD_RatWalkState : StateNode
{

    RatRootState m_ratRootState;

    KD_RatLeftArm leftArm;
    KD_RatRightArm rightArm;
    KD_RatLeftLeg leftLeg;
    KD_RatRightLeg rightLeg;
    KD_RatSpine spine;
    KD_RatTail tail;

    public KD_RatWalkState(RatRootState _rs)
    {
        m_childStates = new List<StateNode>();
        m_rootState = _rs;
        m_ratRootState = _rs;

        m_transform = m_rootState.transform;
        m_gameObject = m_transform.gameObject;
        
        leftArm = m_ratRootState.leftArm.GetComponent<KD_RatLeftArm>();
        rightArm = m_ratRootState.rightArm.GetComponent <KD_RatRightArm>();
        leftLeg = m_ratRootState.leftFoot.GetComponent<KD_RatLeftLeg>();
        rightLeg = m_ratRootState.rightFoot.GetComponent<KD_RatRightLeg>();
        spine = m_ratRootState.spine.GetComponent<KD_RatSpine>();

        //upcast to rat root state which exposes the tail - see also goblin ball
        tail = m_ratRootState.tail.GetComponent<KD_RatTail>();


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

