using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Material defaultMaterial;
    void Start()
    {
        GameObject planet = new GameObject("Planet");
        PlanetGenerator planetGenerator = planet.AddComponent<PlanetGenerator>();
        planetGenerator.RecursionLevel = 1;
        planetGenerator.DefaultMaterial = defaultMaterial;
        planetGenerator.Radius = 1;
        planetGenerator.CreatePlanet();
    }
}
