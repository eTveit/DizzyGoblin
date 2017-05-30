using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateNode
{

    private KT_idleMoveChain ballAnim = null;
    private KT_Goblin_LeftLegIdle leftLegIdle;
    private KT_Goblin_RightLegIdle rightLegIdle;
    private KT_Goblin_LeftArmIdle leftArmIdle;
    private KT_Goblin_RightArmIdle rightArmIdle;
    private KT_Goblin_HipSlideIdle hipIdle;

    float m_idleChooseTimer = -1;
    public float idleSwitchTime = 3.0f;
    public IdleState(RootState _rs)
    {
        m_childStates = new List<StateNode>();
        m_rootState = _rs;
        m_transform = m_rootState.transform;
        m_gameObject = m_transform.gameObject;
   
        //<JPK> @espen - upcasted root state to goblin root state cause he has the ball
        ballAnim = ((GoblinRootState)m_rootState).ball.GetComponent<KT_idleMoveChain>();

        //get the target animations for idle by type
        rightLegIdle = m_rootState.rightFoot.GetComponent<KT_Goblin_RightLegIdle>();
        leftLegIdle = m_rootState.leftFoot.GetComponent<KT_Goblin_LeftLegIdle>();
        leftArmIdle= m_rootState.leftArm.GetComponent<KT_Goblin_LeftArmIdle>();
        rightArmIdle = m_rootState.rightArm.GetComponent<KT_Goblin_RightArmIdle>();

        //get the hip component
        Transform hips = SearchTransform(m_transform, "HIPS");
        hipIdle = hips.GetComponent<KT_Goblin_HipSlideIdle>();
    }


    public override bool advanceTime(float dt)
    {

        //ball should always be enabled. At least until we maybe give it some other animation
        ballAnim.enabled = true;


        //if any child state is true, set my state and return
        //do not continue to process state tree (this can be overridden if desired)
        if(advanceState(dt) == true)
        {
            //if any child state is true, I am false
            p_isInState = false;
            m_isDoingItsState = false;
            leftArmIdle.enabled = false;
            rightArmIdle.enabled = false;
            leftLegIdle.enabled = false;
            rightLegIdle.enabled = false;
            hipIdle.enabled = false;

            //since a child state is true, return this fact!
            return true;
        }

        //lets enable the state and find some anims for it
        //return false;

        //lets just say I am true, which in fact I always am if none of my children are true
        //as IDLE is the first state under root...
        p_isInState = true;  //we assume it is true if we got this far

        if (p_isInState)
        {
            
            ballAnim.isSpinning = false;
            //we can use this to trigger different idles every so often
            if (m_idleChooseTimer < 0)
                m_idleChooseTimer = Time.time;

            //do something different every three seconds
            if (!m_isDoingItsState || Time.time - m_idleChooseTimer > 3.0f)
            {

                Debug.Log("IDLE STATE");

                //reset timer should we want to play different idles
                m_idleChooseTimer = -1;

                m_rootState.targetManager.disableAllTargetAnimations();

                //play
                leftArmIdle.enabled = true;
                rightArmIdle.enabled = true;
                leftLegIdle.enabled = true;
                rightLegIdle.enabled = true;
                hipIdle.enabled = true;

                //flag that we did our one-shot, so don't do it again
                m_isDoingItsState = true;

            }
        }

        return p_isInState;
    }


}
