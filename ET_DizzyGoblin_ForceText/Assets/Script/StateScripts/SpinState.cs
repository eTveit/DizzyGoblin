using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinState : StateNode
{
    
    private bool boostRotation = false;


    //we specify by type the animations we need for this state
    //they could be any animations in the Avatar's targets list 
    private targetMoveSpin leftFootAnim = null;
    private targetMoveSpin rightFootAnim = null;

    private IKAnimationTarget leftArmAnim = null;
    private IKAnimationTarget rightArmAnim = null;


    private float rotationSpeed = 100;
    private float rotationBoost = 1000;

    //ctor
    public SpinState(RootState _rs)
    {
        m_childStates = new List<StateNode>();
        m_rootState = _rs;
        m_transform = m_rootState.transform;
        m_gameObject = m_transform.gameObject;

        
        //get the target animations for Spin by type
        rightFootAnim = m_rootState.rightFoot.GetComponent<targetMoveSpin>();
        leftFootAnim = m_rootState.leftFoot.GetComponent<targetMoveSpin>();

        //because all we must do is enable them, we could access them as a base object
        //if we dont need to read specific property values. so we can do this, by name
        //and then cast it if we need to  
        leftArmAnim = m_rootState.targetManager.getTargetByName("targetLeftArm", "targetArmMove");
        rightArmAnim = m_rootState.targetManager.getTargetByName("targetRightArm", "targetArmMove");

    }


    public override bool advanceTime(float dt)
    {


        if (advanceState(dt) == true)
        {
            //if any child state is true, I am false
            p_isInState = false;
            m_isDoingItsState = false;

            //disable my anims
            leftFootAnim.enabled = false;
            rightFootAnim.enabled = false;

            //since a child state is true, return this fact!
            return true;
        }

        //if no child state is true, see if I need to be true
        if (Input.GetKeyUp(KeyCode.Q))
        {
            //this will toggle states for testing
            p_isInState = !p_isInState;
            if (m_isDoingItsState)
                m_isDoingItsState= false;
        }
         
        if (p_isInState)
        {
            if (!m_isDoingItsState)
            {
                Debug.Log("SPIN STATE");
                m_isDoingItsState = true;

                //Do something, here we make a one-shot to initialize

                //often we will simply want to switch everything else off
                //m_rootState.targetManager.disableAllTargetAnimations();

                //and enable my specific
                leftFootAnim.enabled = true;
                rightFootAnim.enabled = true;




            }

            //refresh speeds with default from inspector
            //we use the left by default. could have globals
            //or some other organizing priciple
            rotationSpeed = leftFootAnim.rotationSpeed;
            rotationBoost = leftFootAnim.rotationBoost;

            Rotate();

            if (Input.GetKeyDown(KeyCode.F))
            {
                SwitchRotateDirection();
            }
            

        }
        return p_isInState;
    }

    void Rotate()
    {

        //Espen, you probably want dt here so we can do "slow-mo" or "fast-mo"
        //also need this to sync up with kicking foot
        if (Mathf.Cos((Time.deltaTime * 4) + 3.141593f) > 0.9f)
        {
            boostRotation = true;
        }
        else
        {
            boostRotation = false;
        }
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        // this is not physics, it should be rebuilt
        if (boostRotation)
        {
            rotation = Quaternion.AngleAxis((rotationSpeed + rotationBoost) * Time.deltaTime, m_transform.up);
        }
        else
        {
            rotation = Quaternion.AngleAxis((rotationSpeed) * Time.deltaTime, m_transform.up);
        }

        m_transform.rotation *= rotation;
    }

    void SwitchRotateDirection()
    {
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


