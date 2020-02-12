using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlanetWizzard : ScriptableWizard
{
    public int recursionLevel;
    public float radius;
    public Material defaultMaterial;
    
    private static Camera cam;
    private static Camera lastUsedCam;

    [MenuItem("GameObject/Planet/planet...")]
    static void CreateWizard()
    {
        cam = Camera.current;
        // Hack because camera.current doesn't return editor camera if scene view doesn't have focuss
        if (!cam)
            cam = lastUsedCam;
        else
            lastUsedCam = cam;
        ScriptableWizard.DisplayWizard("Create IcoSphere",typeof(PlanetWizzard));
    }
    
    void OnWizardCreate()
    {
        GameObject planet = new GameObject("Planet");
        PlanetGenerator planetGenerator = planet.AddComponent<PlanetGenerator>();
        planetGenerator.RecursionLevel = recursionLevel;
        planetGenerator.DefaultMaterial = defaultMaterial;
        planetGenerator.Radius = radius;
        planetGenerator.CreatePlanet();
    }
}
