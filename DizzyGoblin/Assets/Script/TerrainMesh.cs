﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainMesh : MonoBehaviour {


    public int xSize, zSize;

    public Mesh mesh;
    private Vector3[] verts;  // I want a buffer of my initial verts

    //to debug the wave
    public float yplus = 0;
    public bool passWave = false;
    public float lastGoodY = 0;

    // Use this for initialization
    void Awake()
    {


        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Terrain";


        Generate();

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //here we can animate our surface
        if(passWave)
            PassZWave();


    }
	void placeTrees()
	{
		//mesh.vertices
	}

    void Generate()
    {
        
       
        Vector2[] uvs;
        Vector4[] tangents;
        verts = new Vector3[(xSize + 1) * (zSize + 1)];
        
        uvs = new Vector2[verts.Length];
        tangents = new Vector4[verts.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                float y = 0; // Random.Range(0.0f, 1.0f);
               
                verts[i] = new Vector3(x, y, z);

                //UV is just a percent of the vertex to the total, so just divide
                uvs[i] = new Vector2(x / (float)xSize * 4, z / (float)zSize * 4);

                //just use the exemplar tanget, we *could* manipulate them later 
                tangents[i] = tangent;
            }
        }

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.tangents = tangents;
 
        //to complete a quad, we need 6 verts - 2 tris per quad
        /*
        int[] triangles = new int[12];
        triangles[0] = 0;
        triangles[1] = xSize + 1; 
        triangles[2] = 1;

        triangles[3] = 1;
        triangles[4] = xSize + 1;
        triangles[5] = xSize + 2;

        triangles[6] = 1;
        triangles[7] = xSize + 2;
        triangles[8] = 2;

        triangles[9] = 2;
        triangles[10] = xSize + 2;
        triangles[11] = xSize + 3;
        */

        /*
        triangles[0] = 0;
        triangles[3] = triangles[2] = 1;
        triangles[4] = triangles[1] = xSize + 1;
        triangles[5] = xSize + 2;
        */




        //to complete a quad, we need 6 verts - 2 tris per quad
        int[] triangles = new int[xSize * zSize * 6];

        for (int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        mesh.triangles = triangles;

        PerlinNoisePlane pnp = new PerlinNoisePlane();
        pnp.scale = 1.5f;
        pnp.power = 1.5f;
        pnp.MakeSomeNoise(mesh);

		//make specific hills



        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        //reallocate to our buffer
        verts = mesh.vertices;

    }


    public float getHeightAt(Vector3 pos)
    {
        //get the one vertex closest to our rounded integer position
        float h = 0;
        int ix = Mathf.RoundToInt(pos.x);
        int iz = Mathf.RoundToInt(pos.z);
        int v1 = iz * (xSize + 1) + ix; //that extra vert is anoying...

        if (v1 > xSize * zSize || v1 < 0)
            return 1;

        //this is the "simple" height
        h = mesh.vertices[ v1 ].y;

        //if we are really close to it, "early out" so as to prevent the nasty NaN (see below)
        //AND we need to compare 2d distance because our current Y will cause inaccuracy
        Vector2 vc1 = new Vector2(mesh.vertices[v1].x, mesh.vertices[v1].z);
        Vector2 vc2 = new Vector2(pos.x, pos.z);
        if (Vector2.Distance(vc1, vc2) < 0.01f)
            return h;

        //we need to find the triangle we are standing on
        //eg. the two other vertexes closest to us from 4 possible mesh.vertices, 
        //thus:
        
        Vector3[] cvs = new Vector3[4];
        
        /*        
        cvs[0] = mesh.vertices[iz * (xSize + 1) + ix - 1];
        cvs[1] = mesh.vertices[iz * (xSize + 1) + ix + 1];
        cvs[2] = mesh.vertices[iz-1 * (xSize + 1) + ix ];
        cvs[3] = mesh.vertices[iz + 1 * (xSize + 1) + ix];
        */

        //use the ternary operator for convenience, don't wanna poke out of the array bounds
        cvs[0] = ix-1 < 0 ? mesh.vertices[0] : mesh.vertices[ iz * (xSize + 1) + (ix - 1) ];
        cvs[1] = ix+1 > xSize + 1 ? mesh.vertices[0] : mesh.vertices[iz * (xSize + 1) + (ix + 1) ];
        cvs[2] = iz-1 < 0 ? mesh.vertices[0] : mesh.vertices[ (iz - 1) * (xSize + 1) + ix ];
        cvs[3] = iz+1 > zSize + 1 ? mesh.vertices[0] : mesh.vertices[ (iz + 1) * (xSize + 1) + ix];

        //now find the two closest mesh vertices to the float x and z (ignoring y for both)
        //stupid Unity forces me to make lots of "news," because i cant do vector.y = blah
        float dist = 1000000.0f;
        int v2 = 0;
        int v3 = 0;

        //we need to do this twice to get the two closest 
        //ignoring the Y, because it could produce errors
        for(int i = 0; i < 4; i++)
        {

            //neutralize the Y axis
            pos.Set(pos.x, 0, pos.z);
            cvs[i].Set(cvs[i].x, 0, cvs[i].z);

            float test = Vector3.Distance(pos, cvs[i]);

            if ( test < dist )
            {            
                v2 = i;
                dist = test;
            }
        }

        //got my first, do it again - this time our points have zero Y
        dist = 1000000.0f;
        for (int i = 0; i < 4; i++)
        {
            float test = Vector3.Distance(pos, cvs[i]);
            if (test < dist && i != v2)
            {
                v3 = i;
                dist = test;
            }
        }


        //so I now have v1, v2, and v3 as INDEXES, I got my triangle
        h = calcY(mesh.vertices[v1], cvs[v2], cvs[v3], pos.x, pos.z);

        return h;

    }



    float calcY(Vector3 p1, Vector3 p2, Vector3 p3, float x, float z)
    {
        float det = (p2.z - p3.z) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.z - p3.z);

        //make sure I never divide by zero;
        if (det == 0)
            det = 0.0001f;
               

        float l1 = ((p2.z - p3.z) * (x - p3.x) + (p3.x - p2.x) * (z - p3.z)) / det;
        float l2 = ((p3.z - p1.z) * (x - p3.x) + (p1.x - p3.x) * (z - p3.z)) / det;
        float l3 = 1.0f - l1 - l2;

        return  l1 * p1.y + l2 * p2.y + l3 * p3.y;
    }

    
    
    public class PerlinNoisePlane 
    {
        public float power = 3.0f;
        public float scale = 1.0f;
        private Vector2 v2SampleStart = new Vector2(0f, 0f);
  

        public void MakeSomeNoise(Mesh mesh)
        {

            v2SampleStart = new Vector2(Random.Range(0.0f, 100.0f), Random.Range(0.0f, 100.0f));

            
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                float xCoord = v2SampleStart.x + vertices[i].x * scale;
                float yCoord = v2SampleStart.y + vertices[i].z * scale;
                vertices[i].y = (Mathf.PerlinNoise(xCoord, yCoord) - 0.5f) * power;
            }
            mesh.vertices = vertices;
            //mesh.RecalculateBounds();
            //mesh.RecalculateNormals();
        }
    }

    //modulate the terrain on the Z axis
    void PassZWave()
    {
        Vector3[] vertices = mesh.vertices;
        for (int z = 0; z <= zSize; z++)
        {
            yplus = calcYDisplace(z);

            for (int x = 0; x <= xSize; x++)
            {
                int i = z * (xSize + 1) + x;
                Vector3 newpos = mesh.vertices[i];
                newpos = new Vector3(x, verts[i].y + yplus, z);
                vertices[ z * (xSize + 1) + x] = newpos;

            }
        }
        mesh.vertices = vertices;
        //mesh.RecalculateNormals();

    }
    
    float calcYDisplace(float z)
    {

        //place z within the modulus of the span over time 
        //so the wave "moves" across the surface.
        int modz = ((int)(z + Time.time * 2)) % zSize;
        float nz = modz + ((float)z + Time.time * 2 - modz);


        //frequency of the wave
        float period = zSize / 6;   //where 6 is number of repeats across the surface
        float fp = nz / period;

        float radpi = (2 * Mathf.PI) * fp;  //where fp is from 0.0 to 1.0


        float ysin = Mathf.Sin(radpi);
        return (ysin  * 0.5f);     //amplitude (how high to go)

    }
}