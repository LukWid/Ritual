using UnityEngine;

//Hex Sphere Tile
public class Tile : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private Vector3 center;

    [SerializeField]
    private Tile nextTile;
    [SerializeField]
    private Tile previousTile;
    [SerializeField]
    private Planet planet;

    #endregion

    #region Public Properties

    //Tile Attributes
    public Vector3 Center { get => center; set => center = value; }
    public Tile NextTile { get => nextTile; set => nextTile = value; }
    public Planet Planet { get => planet; set => planet = value; }
    public Tile PreviousTile { get => previousTile; set => previousTile = value; }
    public Color EditorPlanetColor;

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
        Gizmos.color = EditorPlanetColor;
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

    public void GetNextTile()
    {
        RaycastHit hit;

        Vector3 planetCenter = Planet.transform.position;
        center = CalculateMeshMiddle();
        Vector3 direction = (center - planetCenter).normalized;

        //SpawnCube(center, direction);

        Vector3 raycastStart = center + direction * 0.005f;

        Debug.DrawRay(raycastStart, direction, Color.red, .01f);

        if (Physics.Raycast(raycastStart, direction, out hit, Mathf.Infinity))
        {
            Tile hitTile = hit.collider.GetComponent<Tile>();
            NextTile = hitTile;
            hitTile.PreviousTile = this;
            hitTile.EditorPlanetColor = EditorPlanetColor;

            //Do some stuff to tile
        }
    }

    #endregion

    #region Private Methods

    private Vector3 CalculateMeshMiddle()
    {
        Vector3 middle = Vector3.zero;
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        foreach (var vertex in mesh.vertices)
        {
            middle += vertex;
        }

        middle = middle / mesh.vertices.Length;

        return middle * Planet.transform.localScale.x;
    }

    private void SpawnCube(Vector3 position, Vector3 direction)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = Vector3.one * 0.2f;
        cube.transform.position = position;
        cube.transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }

    #endregion
}