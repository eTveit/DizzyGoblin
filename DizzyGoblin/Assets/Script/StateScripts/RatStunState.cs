//State Base class created by John Klima
//Modified for this state by Espen Tveit

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatStunState : StateNode {
    
    //we specify by type the animations we need for this state
    //they could be any animations in the Avatar's targets list 
    private EA_ratHitLeftLeg leftFootAnim = null;
    private EA_ratHitRightLeg rightFootAnim = null;
    private EA_ratHitLeftArm leftArmAnim = null;
    private EA_ratHitRightArm rightArmAnim = null;
    private EA_ratHitSpine spine = null;


    private TeleportObject teleport = null;
    
    //ctor
    public RatStunState(RatRootState _rs)
    {
        m_childStates = new List<StateNode>();
        m_rootState = _rs;
        m_transform = m_rootState.transform;
        m_gameObject = m_transform.gameObject;

        //get the teleport component so we can examine the hit state
        //
        teleport = m_transform.GetComponent<TeleportObject>();


        //get the target animations for stun/hit by type
        rightFootAnim = m_rootState.rightFoot.GetComponent<EA_ratHitRightLeg>();
        leftFootAnim = m_rootState.leftFoot.GetComponent<EA_ratHitLeftLeg>();
        leftArmAnim = m_rootState.leftArm.GetComponent<EA_ratHitLeftArm>();
        rightArmAnim = m_rootState.rightArm.GetComponent<EA_ratHitRightArm>();
        spine = m_rootState.rightArm.GetComponent<EA_ratHitSpine>();
        
    }


    public override bool advanceTime(float dt) {


        if(advanceState(dt) == true) {
            //if any child state is true, I am false
            p_isInState = false;
            m_isDoingItsState = false;

            //disable my anims
            leftFootAnim.enabled = false;
            rightFootAnim.enabled = false;
            leftArmAnim.enabled = false;
            rightArmAnim.enabled = false;

            
            //since a child state is true, return this fact!
            return true;
        }

        //if no child state is true, see if I need to be true
        if (!teleport.isHitByBall && m_isDoingItsState)
        {
            //state complete
            p_isInState = false;
            leftFootAnim.enabled = false;
            rightFootAnim.enabled = false;
            leftArmAnim.enabled = false;
            rightArmAnim.enabled = false;
            m_isDoingItsState = false;

            p_isInState = false;

        }
        else if (teleport.isHitByBall)
        {
            p_isInState = true;
        }

        if(p_isInState)
        {
            if(!m_isDoingItsState)
            {
                Debug.Log("RAT STUN STATE");
                m_isDoingItsState = true;

                //Do something, here we make a one-shot to initialize

                //and enable my specific
                leftFootAnim.enabled = true;
                rightFootAnim.enabled = true;
                leftArmAnim.enabled = true;
                rightArmAnim.enabled = true;

            }


        }
        return p_isInState;
    }



}