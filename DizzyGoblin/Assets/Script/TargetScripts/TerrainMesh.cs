using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainMesh : MonoBehaviour {
    //Made by Ruben Junger, Lars Joar Bjørkeland, Bjørn Johansen

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


    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //here we can animate our surface
        if(passWave)
            PassZWave();

        animateSurface(Time.deltaTime);
    }


    public void Generate(int levelDifficulty)
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
        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = xSize + 1; 
        triangles[2] = 1;

        triangles[3] = 1;
        triangles[4] = xSize + 1;
        triangles[5] = xSize + 2;

        //this will make a second quad
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

        
        //this will make all the quads in a fancy inner/outer loop
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


        //use Ken Perlin's eponymous noise to add a bit of variety on our surface
        PerlinNoisePlane pnp = new PerlinNoisePlane();
        pnp.scale = 1.5f;
        pnp.power = 1.5f;
        pnp.MakeSomeNoise(mesh);


       /* for (int i = 0; i < 10; i++)
        {

            int xp = Random.Range(20, xSize - 20);
            int zp = Random.Range(20, zSize - 20);
            float height = ((float)Random.Range(1, 5)) / 10.0f;
            float radius = Random.Range(10, 20);
            Vector3 bumpPos = new Vector3(xp , 0, zp);
            makeBump(radius, height, bumpPos);
        }
        */
   
        

       

        for (int i = 0; i < levelDifficulty*2; i++)
        {

            int xp = Random.Range(0, xSize - 20);
            int zp = Random.Range(0, zSize - 20);

            Vector3 bumpPos = new Vector3(xp, 0, zp);

            //(radius, height, position of bump)
            makeBump(10, 0.1f * levelDifficulty, bumpPos);
        }



        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        //reallocate to our buffer
        verts = mesh.vertices;

    }

    public void makeBump(float radius, float height, Vector3 pos)
    {

        Vector3[] vertices = mesh.vertices;

        float r = radius; //LD
        r = Random.Range(0.0f, 10.0f);

      

        //make sure we have enough vertices in our terrain to make bumps of this size!!!
        Vector3 center = pos;
        for (float phi = 0.0f; phi < 2 * Mathf.PI; phi += Mathf.PI / 100.0f) // Azimuth [0, 2PI]
        {
            for (float theta = 0.0f; theta < Mathf.PI; theta += Mathf.PI / 100.0f) // Elevation [0, PI]
            {
               
                int x = Mathf.RoundToInt(r * Mathf.Cos(phi) * Mathf.Sin(theta) + center.x);
                float y = Mathf.Abs(r * Mathf.Sin(phi) * Mathf.Sin(theta) + center.y) * height;  //LD
                int z = Mathf.RoundToInt(r * Mathf.Cos(theta) + center.z);

                //give it some detail - we could use perlin here too dont let i be too large
                int i = getVertexIndexFromXZ(x, z);
                if (i < vertices.Length)
                {
                    Vector3 vert = vertices[i];
                    vert.Set(x, y + Random.Range(-1, 1) * 0.3f, z);
                    vertices[i] = vert;
                }
            }
        }

        mesh.vertices = vertices;

    }
    public int getVertexIndexFromXZ (int x, int z)
    {
        return z * (xSize + 1) + x;
    }

    public float getHeightAt(Vector3 pos)
    {
        if (mesh == null)
            return 1;
        if (mesh.vertices == null)
            return 1;
        if (mesh.vertexCount < 1)
            return 1;

        //get the one vertex closest to our rounded integer position
        float h = 0;
        int ix = Mathf.RoundToInt(pos.x);
        int iz = Mathf.RoundToInt(pos.z);
        int v1 = iz * (xSize + 1) + ix; //that extra vert is anoying...

        if (v1 > xSize * zSize || v1 < 0)
            return 1;

        //this is the "simple" height
        h = mesh.vertices[ v1 ].y + 0.3f; //give him a lift, make it a param?

		return h;

		//code below is killing framerate, not sure why yet

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
    float timer = -1.0f;
    float dirx = 1.0f;
    float diry = 1.0f;
    float goaldirx = 1.0f;
    float goaldiry = 1.0f;

    public Vector2 wind;

    public void animateSurface(float dt)
    {




        Vector2[] uvs = mesh.uv;


        if (timer < 0)
            timer = Time.time;

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i].x += Mathf.Sin(Time.time) * dt * 0.01f * dirx;
            uvs[i].y += Mathf.Cos(Time.time) * dt * 0.01f * diry;
        }

        //set uvs
        mesh.uv = uvs;


        //create variety
        if (Time.time - timer > Random.Range(3, 5))
        {

            timer = -1.0f;

            goaldirx = Random.Range(-1, 1);
            goaldiry = Random.Range(-1, 1);

        }

        dirx = Mathf.Lerp(dirx, goaldirx, dt);
        diry = Mathf.Lerp(diry, goaldiry, dt);


    }


}

