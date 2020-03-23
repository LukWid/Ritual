using System.Collections.Generic;
using UnityEngine;

//Hex Sphere Tile
public class Tile : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private int id;
    [SerializeField]
    private Vector3 center;
    [SerializeField]
    private GameObject[] Neighbors;

    #endregion

    #region Private Fields

    private static int ID;
    private static float tileRadius = .8f;
    private List<Tile> neighborTiles;
    private float maxTileRadius;

    #endregion

    #region Public Properties

    //Tile Attributes
    public Vector3 Center { get => center; set => center = value; }

    #endregion

    #region Unity Callbacks

    private void OnMouseEnter()
    {
        Pointer.instance.setPointer(PointerStatus.TILE, Center);
    }

    private void OnMouseExit()
    {
        Pointer.instance.unsetPointer();
    }

    #endregion

    #region Public Methods

    public void Initialize(Vector3 coordinates)
    {
        Center = coordinates;
        id = ID;
        ID++;
    }

    //NEW FIND NEIGHBORS
    public void FindNeighbors()
    {
        //Extend a sphere around this tile to find all adjacent tiles within the spheres radius
        Collider[] neighbors = Physics.OverlapSphere(Center, maxTileRadius);
        //OverlapSphere detects the current tile so we must omit this tile from the list
        Neighbors = new GameObject[neighbors.Length - 1];
        neighborTiles = new List<Tile>();
        int j = 0;
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i] != GetComponent<Collider>())
            {
                Neighbors[j] = neighbors[i].gameObject;
                neighborTiles.Add(neighbors[i].gameObject.GetComponent<Tile>());
                j++;
            }
        }
    }

    public void SetMaterial(Material material)
    {
        GetComponent<Renderer>().sharedMaterial = material;
    }

    public void setTileRadius(float r)
    {
        maxTileRadius = r;
    }

    #endregion
}