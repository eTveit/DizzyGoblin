using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepBallAboveGround : MonoBehaviour {

    public TerrainMesh mesh;
    
    void Update() {

        //get the global, keep the target on the terrain surface
        Vector3 pos = transform.position;
        float y = mesh.getHeightAt(pos);
        if(pos.y <y) {
            pos.y = y;
        }

        //set the final position
        transform.position = pos;
    }
}
