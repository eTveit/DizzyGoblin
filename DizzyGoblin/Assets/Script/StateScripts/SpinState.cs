using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinState : StateNode {

    public Transform soundtarget;
    private bool boostRotation = false;


    //we specify by type the animations we need for this state
    //they could be any animations in the Avatar's targets list 
    private targetMoveSpin leftFootAnim = null;
    private targetMoveSpin rightFootAnim = null;
    private ET_targetArmsHoldBall leftArmAnim = null;
    private ET_targetArmsHoldBall rightArmAnim = null;
    private ET_targetMoveChain ballAnim = null;

    private float rotationSpeed = 100;
    private float rotationBoost = 1000;

    private Collider ballCollider = null;
    private Rigidbody ballRB = null;

    //how much time has passed since we started this state
    private float accumTime = 0;

    //ctor
    public SpinState(RootState _rs) {
        m_childStates = new List<StateNode>();
        m_rootState = _rs;
        m_transform = m_rootState.transform;
        m_gameObject = m_transform.gameObject;

        //<JK> maybe we want to sync accumTime to the system time, maybe not.
        //I usually dont (see below state process)
        //accumTime = Time.time;

        //get the target animations for Spin by type
        rightFootAnim = m_rootState.rightFoot.GetComponent<targetMoveSpin>();
        leftFootAnim = m_rootState.leftFoot.GetComponent<targetMoveSpin>();
        leftArmAnim = m_rootState.leftArm.GetComponent<ET_targetArmsHoldBall>();
        rightArmAnim = m_rootState.rightArm.GetComponent<ET_targetArmsHoldBall>();

        //<JPK> @espen - upcasted root state to goblin root state cause he has the ball
        ballAnim = ((GoblinRootState)m_rootState).ball.GetComponent<ET_targetMoveChain>();
        ballCollider = ((GoblinRootState)m_rootState).ballCollider;
        ballRB = ((GoblinRootState)m_rootState).ballRB;
        


        /*
        //because all we must do is enable them, we could access them as a base object
        //if we dont need to read specific property values. so we can do this, by name
        //and then cast it if we need to  
        leftArmAnim = m_rootState.targetManager.getTargetByName("targetLeftArm", "targetArmMove");
        rightArmAnim = m_rootState.targetManager.getTargetByName("targetRightArm", "targetArmMove");
        */
    }

    bool wasChildOverride = false;  //<JPK> debug why this does not continue??
    public override bool advanceTime(float dt) {


        if(advanceState(dt) == true) {
            //if any child state is true, I am false
            p_isInState = false;
            m_isDoingItsState = false;

            wasChildOverride = true;

            //disable my anims
            leftFootAnim.enabled = false;
            rightFootAnim.enabled = false;
            leftArmAnim.enabled = false;
            rightArmAnim.enabled = false;

            //disable ball and chain sound
            soundtarget.gameObject.SetActive(false);

            //drop ball
            ballAnim.isSpinning = false;
            ballCollider.enabled = false;

            Debug.Log("SPIN CHILD TRUE");

            //since a child state is true, return this fact!
            return true;
        }

        if(wasChildOverride)
        {
            Debug.Log("Child was true, no longer true");
            wasChildOverride = false;
            //<JPK> maybe re-init state here? but to be honest, keydown below is better


        }

        //TODO: Modify to spin only when holding SPACE
        //if no child state is true, see if I need to be true
        if(!Input.GetKey(KeyCode.Space))
        {
            //this will toggle states for testing
            p_isInState = false;
            if(m_isDoingItsState) {
                leftFootAnim.enabled = false;
                rightFootAnim.enabled = false;
                leftArmAnim.enabled = false;
                rightArmAnim.enabled = false;
                ballAnim.isSpinning = false;
                ballCollider.enabled = false;
                m_isDoingItsState = false;

                //disable ball and chain sound
                soundtarget.gameObject.SetActive(false);

                //<JPK> maybe we want to sync accumTime to the system time, maybe not.
                //I usually dont (see below state process)- zero start time is useful.
                accumTime = 0; // Time.time;
                
                //<JPK> log to see if my key is canceled by another key - Not multi???
                Debug.Log("SPIN KEY UP");
            }
        }

        //<JPK> we need to remain holding this after a child is true, to return to state
        //      changed from getkeydown to getkey
        if(Input.GetKey(KeyCode.Space)) {
            p_isInState = true;
            Debug.Log("SPIN KEY DOWN");
        }


        if(p_isInState) {
            if(!m_isDoingItsState) {
                Debug.Log("SPIN STATE");
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
                ballCollider.enabled = true;

                //enable ball and chain sound
                soundtarget.gameObject.SetActive(true);


            }

            //refresh speeds with default from inspector
            //we use the left by default. could have globals
            //or some other organizing priciple
            rotationSpeed = leftFootAnim.rotationSpeed;
            rotationBoost = leftFootAnim.rotationBoost;

            Rotate(dt);

            ballRB.AddForce(((m_transform.forward + new Vector3(0, 0.2f, 0))*100));
        }

        return p_isInState;
    }

    void Rotate(float dt) {
        //Espen, you probably want dt here so we can do "slow-mo" or "fast-mo"
        //I tried first using just dt, but that is always a small value,
        //so the result of the cos was always very high, causing him to boost constantly
        //Using a float that increments using dt, we get a cos wave, 
        //while still being able to affect it through dt

        //<JPK> exactly, sin/cos always need "what time is it?" not "how much since last time?"
        //but dt should proliferate anywhere you use Time.deltaTime, or Time.time thus,
        //dt is our slow/fast mo value.

        //at this point we are not yet implementing that, but it makes things easier if in the 
        //future, we do. So leftFootAnim.incrementingDT should be incremented from a place that has 
        //access to our "true" delta time if and when we build it. this is why i usually pass dt
        //to time dependent functions as a param, rather than using Time.deltaTime 

        //That said, alternatively you could do this (accumTime a property of the state)
        //and indeed, one often wants to keep track in a state of overall time passage:
        accumTime += dt;
        if(Mathf.Cos((accumTime * leftFootAnim.speed) + Mathf.PI) > 0.9f)
        //if (Mathf.Cos((leftFootAnim.incrementingDT * leftFootAnim.speed) + Mathf.PI) > 0.9f) 
        {
            boostRotation = true;
        }
        else
        {
            boostRotation = false;
        }

        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        // this is not physics, it should be rebuilt
        if(boostRotation) {
            rotation = Quaternion.AngleAxis((rotationSpeed + rotationBoost) * dt, m_transform.up);
        }
        else {
            rotation = Quaternion.AngleAxis((rotationSpeed) * dt, m_transform.up);
        }

        m_transform.rotation *= rotation;
    }

    public void SwitchRotateDirection() {
        //temp storage of values
        float lPhase = leftFootAnim.phase;
        float lSpeed = leftFootAnim.speed;
        float lRange = leftFootAnim.range;
        float lCircularHeight = leftFootAnim.circularHeight;
        float rPhase = rightFootAnim.phase;
        float rSpeed = rightFootAnim.speed;
        float rRange = rightFootAnim.range;
        float rCircularHeight = rightFootAnim.circularHeight;

        //assign values to opposite legs
        leftFootAnim.phase = rPhase;
        leftFootAnim.speed = rSpeed;
        leftFootAnim.range = rRange;
        leftFootAnim.circularHeight = rCircularHeight;
        leftFootAnim.isKickingFoot = !leftFootAnim.isKickingFoot;
        rightFootAnim.phase = lPhase;
        rightFootAnim.speed = lSpeed;
        rightFootAnim.range = lRange;
        rightFootAnim.circularHeight = lCircularHeight;
        rightFootAnim.isKickingFoot = !rightFootAnim.isKickingFoot;

        leftFootAnim.rotationSpeed = -leftFootAnim.rotationSpeed;
        leftFootAnim.rotationBoost = -leftFootAnim.rotationBoost;

    }

}


