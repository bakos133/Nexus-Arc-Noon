using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyGenerator : MonoBehaviour
{

    [Range(2, 256)]
    public int resolution  = 20;

    [HideInInspector]
    public bool shapeSF;
    [HideInInspector]
    public bool colorSF;

    public bool autoUpdate = true;

    GalacticBody shapeSettings;
    public ColorSettings colorSettings;

    public ShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    Color surfaceColor;

    private void Start()
    {
        if (autoUpdate)
            GeneratePlanet();
    }



    void Initialize()
    {
        Debug.Log("init");
        shapeGenerator = new ShapeGenerator(GetComponent<GalacticBody>());
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.position = transform.position + meshObj.transform.position;
                meshObj.transform.parent = transform;
                meshObj.transform.SetAsFirstSibling();
                meshObj.transform.localScale = Vector3.one;

               
              meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));

                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    GalacticBody gb;
    public void GeneratePlanet()
    {
        gb = gameObject.GetComponent<GalacticBody>();
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdated()
    {
        Initialize();
        GenerateMesh();
    }
    public void OnColorSettingsUpdated()
    {

        if (!Application.isPlaying)
        {
            return;
        }
        Initialize();
        GenerateColors();
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {

            face.ConstructShape();
        }
    }

    void GenerateColors()
    {
        if (gb.EntityType == EntityType.SolarSystem)
        {

            surfaceColor = new Color(1f,1f,0);
        }
        else
        {
            surfaceColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }
        foreach (MeshFilter m in meshFilters)
        {
            var mr = m.GetComponent<MeshRenderer>();
            if (gb.EntityType == EntityType.SolarSystem)
            {


                mr.sharedMaterial.color = surfaceColor;// colorSettings.planetColor;
                mr.sharedMaterial.EnableKeyword("_EMISSION");
                mr.sharedMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                mr.sharedMaterial.SetColor("_EmissionColor", surfaceColor);
            }
        }
    }
}

public class TerrainFace
{
    ShapeGenerator shapeGenerator;
    int resolution;
    Mesh mesh;

    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;

    public bool updShape = false;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructShape()
    {
        Vector3[] vertices = new Vector3[(resolution * resolution)];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pOnUnitCube = localUp + (percent.x - .50001f) * 2 * axisA  + (percent.y - .50001f) * 2 * axisB;
                Vector3 pOnUnitSphere = pOnUnitCube.normalized;

                vertices[i] = shapeGenerator.CalcPOnPlanet(pOnUnitSphere);

                if(x != resolution-1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex+1] = i + resolution + 1;
                    triangles[triIndex+2] = i + resolution;

                    triangles[triIndex+3] = i;
                    triangles[triIndex+4] = i + 1;
                    triangles[triIndex+5] = i + resolution + 1;

                    triIndex += 6;
                }
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}


public class ShapeGenerator
{
    GalacticBody galBod;

    public ShapeGenerator(GalacticBody galBod)
    {
        this.galBod = galBod;
        offset = Random.Range(-10f, 10f);
    }
    float offset = 0;
    float magnitude = 0.2f;
    public Vector3 CalcPOnPlanet(Vector3 pOnUnitSphere)
    {
        var c = pOnUnitSphere * (galBod.pRadius / (galBod.pRadius / 2)) + Vector3.one * magnitude * Mathf.PerlinNoise(pOnUnitSphere.x + offset,pOnUnitSphere.y + offset);
        return c;
    }
}