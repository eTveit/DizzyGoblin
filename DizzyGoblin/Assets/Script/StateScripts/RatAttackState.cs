//State Base class created by John Klima
//Modified for this state by Espen Tveit

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAttackState : StateNode {

    //we specify by type the animations we need for this state
    //they could be any animations in the Avatar's targets list 
    private ET_ratLeftLegAttack leftFootAnim = null;
    private ET_ratRightLegAttack rightFootAnim = null;
    private ET_ratLeftArmAttack leftArmAnim = null;
    private ET_ratRightArmAttack rightArmAnim = null;
    private ET_ratSpineAttack spine = null;
    private ET_ratTailAttack tail = null;



    private float accumTime = 0;

    private bool hasAttacked = false;
    private bool completeCycle = false;

    //ctor
    public RatAttackState(RatRootState _rs) {
        m_childStates = new List<StateNode>();
        m_rootState = _rs;
        m_transform = m_rootState.transform;
        m_gameObject = m_transform.gameObject;



        //get the target animations for stun/hit by type
        rightFootAnim = m_rootState.rightFoot.GetComponent<ET_ratRightLegAttack>();
        leftFootAnim = m_rootState.leftFoot.GetComponent<ET_ratLeftLegAttack>();
        leftArmAnim = m_rootState.leftArm.GetComponent<ET_ratLeftArmAttack>();
        rightArmAnim = m_rootState.rightArm.GetComponent<ET_ratRightArmAttack>();
        spine = m_rootState.spine.GetComponent<ET_ratSpineAttack>();
        tail = ((RatRootState)m_rootState).tail.GetComponent<ET_ratTailAttack>();

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
            spine.enabled = false;

            //since a child state is true, return this fact!
            return true;
        }

        //if no child state is true, see if I need to be true
        if(((RatRootState)m_rootState).steer.state != steering.STATES.HIT && m_isDoingItsState) {
            //state complete
            p_isInState = false;
            leftFootAnim.enabled = false;
            rightFootAnim.enabled = false;
            leftArmAnim.enabled = false;
            rightArmAnim.enabled = false;
            spine.enabled = false;
            tail.enabled = false;

            m_isDoingItsState = false;

            p_isInState = false;

        }
        else if(((RatRootState)m_rootState).steer.state == steering.STATES.HIT) {
            p_isInState = true;
        }

        if(p_isInState) {
            if(!m_isDoingItsState) {
                Debug.Log("RAT Attack STATE");
                m_isDoingItsState = true;

                //Do something, here we make a one-shot to initialize

                //and enable my specific
                leftFootAnim.enabled = true;
                rightFootAnim.enabled = true;
                leftArmAnim.enabled = true;
                rightArmAnim.enabled = true;
                spine.enabled = true;
                tail.enabled = true;

                accumTime = 0;

            }

            //When the spine has these positions, the rat attacks
            if(spine.curPos == 2 || spine.curPos == 4) {
                if(accumTime < 0) {
                    accumTime = 0;
                }
                if(accumTime > 0.2f && !hasAttacked) {
                    ((RatRootState)m_rootState).steer.hit(dt);
                    hasAttacked = true;
                }
                accumTime += dt;
            }
            else {
                hasAttacked = false;
                accumTime = -1;
                if(((RatRootState)m_rootState).steer.CheckDistance() > 5.0f) {
                    ((RatRootState)m_rootState).steer.state = steering.STATES.FLEE;
                }
            }
            if(spine.curPos == 4) {
                completeCycle = true;
            }
            if(spine.curPos == 0 && completeCycle) {
                completeCycle = false;
                ((RatRootState)m_rootState).steer.state = steering.STATES.FLEE;
            }


        }
        return p_isInState;
    }



}