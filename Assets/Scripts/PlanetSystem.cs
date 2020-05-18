using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlanetSystem : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private List<Planet> planets;

    [SerializeField]
    private int currentIteration;

    #endregion

    #region Private Fields

    private List<Tile> currentTiles;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        Physics.queriesHitBackfaces = true;
    }
    
    void OnSceneGUI () {
        Handles.BeginGUI();
 
        if (GUI.Button(new Rect(10, 10, 100, 50), "Button"))
        {
            Debug.Log("Pressed");
        }
   
        Handles.EndGUI();
    }

    #endregion

    public void ShowIteration(int index)
    {
        foreach (Planet planet in planets)
        {
            planet.gameObject.SetActive(false);   
        }

        currentIteration = index;
        planets[index].gameObject.SetActive(true);
    }
}