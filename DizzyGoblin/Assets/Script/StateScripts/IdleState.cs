using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateNode
{

    private ET_targetMoveChain ballAnim = null;


    float m_idleChooseTimer = -1;
    public float idleSwitchTime = 3.0f;
    public IdleState(RootState _rs)
    {
        m_childStates = new List<StateNode>();
        m_rootState = _rs;
        m_transform = m_rootState.transform;
        m_gameObject = m_transform.gameObject;

        ballAnim = m_rootState.ball.GetComponent<ET_targetMoveChain>();

    }


    public override bool advanceTime(float dt)
    {
        ballAnim.enabled = true;
        //if any child state is true, set my state and return
        //do not continue to process state tree (this can be overridden if desired)
        if (advanceState(dt) == true)
        {
            //if any child state is true, I am false
            p_isInState = false;
            m_isDoingItsState = false;

            //since a child state is true, return this fact!
            return true;
        }

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
                
                //we have no idle animations yet


                //flag that we did our one-shot, so don't do it again
                m_isDoingItsState = true;

            }
        }

        return p_isInState;
    }


}
