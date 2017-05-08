using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootState : MonoBehaviour {


    protected List<StateNode> m_childStates ;
    protected Animation m_anim;
    protected Transform m_neck;
    protected Quaternion m_neckRot;


    // Use this for initialization prior to anything else happening
    void Awake () {


        //buffer a pointer to the specific neck joint 
        //GetComponent is expensive (particularly if we get by name)
        //so we get all the things we need in advance. For some reason
        //unity does not have its own "find by name" so we make our own
        //"recursive" search
        m_neck = Search(GetComponent<Transform>(), "Bip01_Head1");

        //we also need to buffer the initial neck rotation if we want to 
        //override the keyframe animation on that bone
        m_neckRot = m_neck.rotation;


        m_anim = GetComponent<Animation>(); 
        m_childStates = new List<StateNode>();

        //lets kick it off in idle
        //m_anim.Play("Idle_L");

        //we must always add at least one, if we want the graph to run
        IdleState idlestate = new IdleState(m_anim);
        m_childStates.Add(idlestate);

        WalkState walkstate = new WalkState(m_anim);
        idlestate.addChildState(walkstate);

        WalkBackState walkbackstate = new WalkBackState(m_anim);
        walkstate.addChildState(walkbackstate);


        
    }

    void Start()
    {
        //place the dude
        transform.position.Set(5, 0, 5);
    }

    // Update is called once per frame
    void Update ()
    {
        
        //iterate the child states, calling advanceTime
        foreach (StateNode child in m_childStates)
        {
            child.advanceTime(Time.deltaTime);
        }
        
    }

    //here we will override any keyframe animations on specific nodes
    void LateUpdate()
    {
        m_neckRot *= Quaternion.Euler(Vector3.right * 10);
        m_neck.rotation = m_neckRot;
    }


    public Transform Search(Transform target, string name)
    {
        if (target.name == name) return target;

        for (int i = 0; i < target.childCount; i++)
        {
            //we use "var" because the component could be anything
            var result = Search(target.GetChild(i), name);

            if (result != null) return result;
        }

        return null;
    }
    
}
