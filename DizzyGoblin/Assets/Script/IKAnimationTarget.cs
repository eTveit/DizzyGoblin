using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class IKAnimationTarget : MonoBehaviour
{

    public abstract string getAnimName();
    public string animationName = "DONT_FORGET_TO_NAME_IT";

	public Vector3 startPosition;
	private bool madeStartPosition = false;
	
	//returns true when near the initial position of the animation 
	public bool interpolateToStartPosition(float dt, float speed)
	{
		if (madeStartPosition)
			return true;

		//we need to transition smoothly from one target script to the next  
		Vector3 lpos = transform.localPosition;
		float dist = Vector3.Distance(lpos, startPosition);

		//this threshold probably needs a tweak, if I am close enough, complete the task
		if (dist < 0.1f)
		{
			madeStartPosition = true;
			return true;
		}
		//move quickly to that position
		lpos = Vector3.Lerp(lpos, startPosition, dt * speed);

		//set it
		transform.localPosition = lpos;


		return false;
	}

	public void reset()
	{
		madeStartPosition = false;
	}

}
