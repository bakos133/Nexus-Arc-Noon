﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GManager : MonoBehaviour
{

    public static List<GalacticBody> galacticBodies = new List<GalacticBody>();
    public const float gConstant = 6.674f;

    float closestDist = Mathf.Infinity;

    public int maxSpawn = 0;
    int curSpawn = 0;

    float minDist = 0f;
    float maxDist = 0f;

    public GameObject universe;
    public List<GameObject> galaxy = new List<GameObject>();
    public List<GameObject> solarSystem = new List<GameObject>();
    public List<GameObject> planet = new List<GameObject>();
    public List<GameObject> moon = new List<GameObject>();
    public List<GameObject> nebulae = new List<GameObject>();

    public Dictionary<string, float> liquids = new Dictionary<string, float>();
    public Dictionary<string, float> solids = new Dictionary<string, float>();
    public Dictionary<string, float> gases = new Dictionary<string, float>();

    public GameObject nebula;

    #region noob addition

    public static SpawnSettings SpawnSettings = new SpawnSettings();
    [SerializeField]
    public GalacticBody UniversePrefab = null;

    GalacticBody universeBody;

    #endregion noob addition




    private void Start()
    {


        #region noob spawning
        // comment this out to go to the previous version
        universeBody = Instantiate(UniversePrefab);
        universeBody.SpawnChildren();
        return;

        #endregion


        maxSpawn = 1;                                                                               //how many are allowed to spawn (in this case, universes) - only 1
        universe = GameObject.CreatePrimitive(PrimitiveType.Capsule);                         //Spawn the universe into the game at the center of everything. (coord. x0 y0 z0)
        universe.transform.name = "Universe";                                                            //Name it universe so we can refer to it later

        minDist = -100;
        maxDist = 100000;

        curSpawn = 0;                                                                               //Set the current spawned objects to 0 - used in Galaxy
        maxSpawn = Random.Range(1, 1);                                                             //Set the max spawnable objects to a random number between 1 & 10. (anywhere between 1 - 10 galaxies will be spawned)
        Galaxy();                                                                                   //Call Galaxy() function to start the universe spawning process.
    }

    private void Galaxy()
    {
        //So, while the currently spawned objects is less than the max spawnable set, keep adding galaxies to the universe (& catalogue each one).
        //This is very long winded for a reason - The way the spawner works from here on out is precisely random. 
        //By this I mean, it will spawn a galaxy in a random spot, between -10,000 & +10,000, all while maintaining 100 units of distance from it's parent object. (Whatever that parent may be). 
        //Simply put, it selects a point in space anywhere from 100 - 10,000 units of distance away from it's parent at random, then spawns a galaxy there.
        //Rotation of body is also randomly set between -45 & 45 degrees.
        while (curSpawn < maxSpawn)
        {
            GameObject tempObj = new GameObject();
            GameObject tempChild = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //tempChild.transform.parent = tempObj.transform;
            tempObj.transform.parent = universe.transform;
            tempObj.transform.SetPositionAndRotation(universe.transform.position + new Vector3(Random.Range(Random.Range(-maxDist, -minDist), Random.Range(minDist, maxDist)), Random.Range(Random.Range(-maxDist, -minDist), Random.Range(minDist, maxDist)), Random.Range(Random.Range(-maxDist, -minDist), Random.Range(minDist, maxDist))), Quaternion.identity);
            tempObj.transform.name = "Galaxy" + (Random.Range(0, galaxy.Count) + 1);
            galaxy.Add(tempObj);                                                                    //Add a new Galaxy to the galaxy list
            curSpawn++;                                                                             //Increment curSpawn by 1
        }

        maxDist = maxDist/4;
        curSpawn = 0;                                                                               //Set curSpawn to 0 again.
        maxSpawn = Random.Range(10, 20);                                                             //Set maxSpawn to a random number between 1 & 10 again.
        SolarSystem();                                                                              //Move onto solar systems.
    }

    void SolarSystem()
    {
        //Every function from here (SolarSystem()) to Moons() is identical - only difference being the max distance from parent object.
        for (int i = 0; i < galaxy.Count; i++)                                                      //for each galaxy..
        {
            while (curSpawn < maxSpawn)
            {
                GameObject tempObj = new GameObject();
                GameObject tempChild = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                tempChild.transform.parent = tempObj.transform;
                GameObject tempNeb = Instantiate(nebula, tempChild.transform.position, Quaternion.identity);
                tempNeb.transform.parent = tempChild.transform;
                tempObj.transform.SetPositionAndRotation(galaxy[i].transform.position + new Vector3(Random.Range(Random.Range(-maxDist, -minDist), Random.Range(minDist, maxDist)), Random.Range(Random.Range(-maxDist, -minDist), Random.Range(minDist, maxDist)), Random.Range(Random.Range(-maxDist, -minDist), Random.Range(minDist, maxDist))), Quaternion.identity);
                tempObj.transform.name = "Solar System" + i;
                tempObj.AddComponent<GalacticBody>();
                solarSystem.Add(tempObj);                                                                    //Add a new Galaxy to the galaxy list
                curSpawn++;                                                                             //Increment curSpawn by 1
            }

            curSpawn = 0;                                                                           //IMPORTANT: setting curSpawn to 0 here allows this for-loop to properly spawn each galaxy with it's own random set of solar systems. Otherwise every galaxy will look exactly alike.
            maxSpawn = Random.Range(10, 20);                                                         //IMPORTANT: this is what allows every galaxy to be random - we are re-using this int so 
        }

        maxDist = maxDist/15;
        curSpawn = 0;                                                                               //Repeat the process of setting curSpawn to 0 so we can start fresh.
        maxSpawn = Random.Range(1, 20);                                                             //Repeat the process of setting maxSpawn to a random number between 1 & 10
        Planets();                                                                                  //Start the next function. (Please Note: Planets() & Moons() are identical to SolarSystem, minus the distances, so i will not be commenting on them.
    }

    void Planets()
    {
        for (int i = 0; i < solarSystem.Count; i++)
        {
            while (curSpawn < maxSpawn)
            {
                GameObject tempObj = new GameObject();
                GameObject tempChild = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                tempChild.transform.parent = tempObj.transform;
                tempObj.transform.SetPositionAndRotation(solarSystem[i].transform.position + new Vector3(Random.Range(Random.Range(-maxDist, -minDist), Random.Range(minDist, maxDist)), Random.Range(-20, 20), Random.Range(Random.Range(-maxDist, -minDist), Random.Range(minDist, maxDist))), Quaternion.identity);
                tempObj.transform.eulerAngles = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15));
                tempObj.transform.name = "Planet" + i;
                tempObj.AddComponent<GalacticBody>();
                planet.Add(tempObj);                                                                    
                curSpawn++;                                                                             
            }

            curSpawn = 0;
            maxSpawn = Random.Range(1, 20);
        }

        maxDist = maxDist/15;
        curSpawn = 0;
        maxSpawn = Random.Range(Random.Range(0,3), Random.Range(1,4));
        Moons();
    }

    void Moons()
    {
        for (int i = 0; i < planet.Count; i++)
        {
            while (curSpawn < maxSpawn)
            {
                GameObject tempObj = new GameObject();
                GameObject tempChild = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                tempChild.transform.parent = tempObj.transform;
                tempObj.transform.SetPositionAndRotation(planet[i].transform.position + new Vector3(Random.Range(Random.Range(-maxDist, -minDist), Random.Range(minDist, maxDist)), Random.Range(-10,10), Random.Range(Random.Range(-maxDist, -minDist), Random.Range(minDist, maxDist))), Quaternion.identity);
                tempObj.transform.rotation = Quaternion.Euler(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15));
                tempObj.transform.name = "Moon" + i;
                tempObj.AddComponent<GalacticBody>();
                moon.Add(tempObj);
                curSpawn++;                                                                             
            }

            curSpawn = 0;
            maxSpawn = Random.Range(Random.Range(0, 3), Random.Range(1, 4));
        }

        maxDist = 0;
        curSpawn = 0;
        maxSpawn = Random.Range(0, 0);
        FinishUp();
    }

    void FinishUp()
    {
        foreach (GameObject sol in solarSystem)
        {
            sol.transform.eulerAngles = new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
        }


    }

    void GetGalacticBodyByType(EntityType type)
    {
        universe.GetComponent<GalacticBody>().GetGalacticBodyByType(type);

    }

    
}

public class SpawnSettings{
    public int minUniverse = 1;
    public int maxUniverse = 1;

    public int minGalaxy = 1;
    public int maxGalaxy = 1;

    public int minSolarSystem = 10;
    public int maxSolarSystem = 20;

    public int minPlanet = 1;
    public int maxPlanet = 20;

    public int minMoonLow = 0;
    public int minMoonHigh = 3;
    public int maxMoonLow = 1;
    public int maxMoonHigh = 4;

    public float maxDistanceGalaxyK = 1f;
    public float maxDistanceSolarSystemK = 4f;
    public float maxDistancePlanetK= 4f * 15f;
    public float maxDistanceMoonK = 4f * 15f * 15f;

    public float maxDistance = 100000f;
    public float minDistacne = -100f;


}
