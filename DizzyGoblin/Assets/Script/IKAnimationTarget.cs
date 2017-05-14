using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class IKAnimationTarget : MonoBehaviour
{

    public abstract string getAnimName();
    public string animationName = "DONT_FORGET_TO_NAME_IT";
	
}
