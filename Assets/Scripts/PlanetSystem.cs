using System.Collections.Generic;
using UnityEngine;

public class PlanetSystem : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private List<Hexsphere> planets;

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

    #endregion

    #region Private Methods

    
    
    

    #endregion
}