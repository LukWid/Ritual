using UnityEngine;

//Hex Sphere Tile
public class Tile : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private Vector3 center;

    [SerializeField]
    private Tile nextTile;

    #endregion

    #region Public Properties

    //Tile Attributes
    public Vector3 Center { get => center; set => center = value; }
    public Tile NextTile { get => nextTile; set => nextTile = value; }
    public Planet Planet { get; set; }
    
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

    private void Awake()
    {
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
    }

    private void OnDrawGizmos()
    {
        if (Planet == null)
        {
            return;
        }

        Gizmos.color = Planet.EditorPlanetColor;
        Gizmos.DrawMesh(GetComponent<MeshFilter>().sharedMesh, Planet.transform.position, Planet.transform.rotation, Planet.transform.localScale);
        
    }

    #endregion

    #region Public Methods

    public void Initialize(Vector3 coordinates)
    {
        Center = coordinates;
    }

    public void SetMaterial(Material material)
    {
        GetComponent<Renderer>().sharedMaterial = material;
    }

    #endregion
}