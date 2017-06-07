using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : StateNode
{
    
    //we specify by type the animations we need for this state
    //they could be any animations in the Avatar's targets list 
    private targetMoveLeft leftFootAnim = null;
    private targetMoveRight rightFootAnim = null;
	private targetMoveRight spineAnim = null;  //re-using the same anim on a different joint
	private targetRightArmIdle rightArmAnim = null;
	private targetLeftArmIdle leftArmAnim = null;
    private GoblinGlobals m_globs = null;


    //private IKAnimationTarget leftArmAnim = null;
    //private IKAnimationTarget rightArmAnim = null;

    private float accumTime = 0;

    private float speedModifier = 2.8f;
       
    //ctor
    public DodgeState(RootState _rs)
    {
        m_childStates = new List<StateNode>();
        m_rootState = _rs;
        m_transform = m_rootState.transform;
        m_gameObject = m_transform.gameObject;


        m_globs = m_transform.GetComponent<GoblinGlobals>();
        
        
        //get the target animations for Dodge by type
        rightFootAnim = m_rootState.rightFoot.GetComponent<targetMoveRight>();
        leftFootAnim = m_rootState.leftFoot.GetComponent<targetMoveLeft>();
		leftArmAnim = m_rootState.leftArm.GetComponent<targetLeftArmIdle>();
		rightArmAnim = m_rootState.rightArm.GetComponent<targetRightArmIdle>();
		spineAnim = m_rootState.spine.GetComponent<targetMoveRight>();
    

		//because all we must do is enable them, we could access them as a base object
		//if we dont need to read specific property values. so we can do this, by name
		//and then cast it if we need to  
		//leftArmAnim = m_rootState.targetManager.getTargetByName("targetLeftArm", "targetArmMove");

	}


    public override bool advanceTime(float dt)
    {


        if (rightFootAnim.cycleCount > 8)
            p_isInState = false;


        if (advanceState(dt) == true)
        {
            //if any child state is true, I am false
            p_isInState = false;
            m_isDoingItsState = false;

			//disable my anims
			leftFootAnim.enabled = false;
			rightFootAnim.enabled = false;
			leftArmAnim.enabled = false;
			rightArmAnim.enabled = false;
			spineAnim.enabled = false;
    
			//since a child state is true, return this fact!
			return true;
        }

        //if no child state is true, see if I need to be true
        if (Input.GetKeyUp(KeyCode.J))
        {
            //this will toggle states for testing
            p_isInState = !p_isInState;

			//this flags the initialization of the state (below) thus
			//it knows to turn OFF the state from the keypress toggle
			if (m_isDoingItsState)
			{
				m_isDoingItsState = false;
				//disable my anims
				leftFootAnim.enabled = false;
				rightFootAnim.enabled = false;
				leftArmAnim.enabled = false;
				rightArmAnim.enabled = false;
				spineAnim.enabled = false;

                accumTime = 0;
            }
        }




        if (p_isInState)
        {
            if (!m_isDoingItsState)
            {
                Debug.Log("DODGE STATE");

                

                rightFootAnim.cycleCount = 0;

                m_isDoingItsState = true;

                //Do something, here we make a one-shot to initialize

                //often we will simply want to switch everything else off
                m_rootState.targetManager.disableAllTargetAnimations();

                //and enable my specific
                leftFootAnim.enabled = true;
                rightFootAnim.enabled = true;
                leftArmAnim.enabled = true;
                rightArmAnim.enabled = true;
                spineAnim.enabled = true;
    
            }

            accumTime += dt;
            moveLeft(dt);
            if(accumTime > 0.5f) {
                p_isInState = false;
                leftFootAnim.enabled = false;
                rightFootAnim.enabled = false;
                leftArmAnim.enabled = false;
                rightArmAnim.enabled = false;
                spineAnim.enabled = false;

                rightFootAnim.cycleCount = 0;

                m_isDoingItsState = false;

                accumTime = 0;
                
            }
        }
        
            return p_isInState;
        
    }


    void moveLeft(float dt)

    {
        
        Vector3 right = m_transform.right;
        m_transform.position -= right * dt * m_globs.speed * speedModifier;

    }

}


