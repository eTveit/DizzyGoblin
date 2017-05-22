//State Base class created by John Klima
//Modified for this state by Espen Tveit

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : StateNode {
    
    //we specify by type the animations we need for this state
    //they could be any animations in the Avatar's targets list 
    private ET_targetLegsHitTree leftFootAnim = null;
    private ET_targetLegsHitTree rightFootAnim = null;
    private ET_targetArmsHitTree leftArmAnim = null;
    private ET_targetArmsHitTree rightArmAnim = null;
    private ET_targetMoveChain ballAnim = null;
    

    private float rotationSpeed = 100;
    private float rotationBoost = 1000;

    //how much time has passed since we started this state
    private float accumTime = 0;

    //ctor
    public StunState(RootState _rs) {
        m_childStates = new List<StateNode>();
        m_rootState = _rs;
        m_transform = m_rootState.transform;
        m_gameObject = m_transform.gameObject;

        //<JK> maybe we want to sync accumTime to the system time, maybe not.
        //I usually dont (see below state process)
        //accumTime = Time.time;

        //get the target animations for Spin by type
        rightFootAnim = m_rootState.rightFoot.GetComponent<ET_targetLegsHitTree>();
        leftFootAnim = m_rootState.leftFoot.GetComponent<ET_targetLegsHitTree>();
        leftArmAnim = m_rootState.leftArm.GetComponent<ET_targetArmsHitTree>();
        rightArmAnim = m_rootState.rightArm.GetComponent<ET_targetArmsHitTree>();
        ballAnim = m_rootState.ball.GetComponent<ET_targetMoveChain>();
        
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

            //drop ball
            ballAnim.isSpinning = false;

            //since a child state is true, return this fact!
            return true;
        }

        //if no child state is true, see if I need to be true
        if(Input.GetKeyDown(KeyCode.Y)) {
            //this will toggle states for testing
            p_isInState = !p_isInState;
            if(m_isDoingItsState) {
                leftFootAnim.enabled = false;
                rightFootAnim.enabled = false;
                leftArmAnim.enabled = false;
                rightArmAnim.enabled = false;
                ballAnim.isSpinning = false;
                m_isDoingItsState = false;

                //<JK> maybe we want to sync accumTime to the system time, maybe not.
                //I usually dont (see below state process)- zero start time is useful.
                accumTime = 0; // Time.time;
            }
        }

        if(p_isInState) {
            if(!m_isDoingItsState) {
                Debug.Log("STUN STATE");
                m_isDoingItsState = true;

                //Do something, here we make a one-shot to initialize

                //often we will simply want to switch everything else off
                //m_rootState.targetManager.disableAllTargetAnimations();

                //and enable my specific
                leftFootAnim.enabled = true;
                rightFootAnim.enabled = true;
                leftArmAnim.enabled = true;
                rightArmAnim.enabled = true;
                ballAnim.enabled = true;
                ballAnim.isSpinning = true;




            }


            //<ET> Okay, the idea was to quit the state, and go back to what he was doing, but he doesn't seem to return to some other state.
            //Probably because the previous state in inactive? Could we get active state before activating this, and then reactivate it?
            accumTime += dt;
            if(accumTime > 2.0f) {
                p_isInState = false;
                leftFootAnim.enabled = false;
                rightFootAnim.enabled = false;
                leftArmAnim.enabled = false;
                rightArmAnim.enabled = false;
                ballAnim.isSpinning = false;
                m_isDoingItsState = false;

                accumTime = 0;
            }
        }
        return p_isInState;
    }



}