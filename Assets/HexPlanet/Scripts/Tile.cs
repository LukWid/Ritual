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

    public void GetNextTile()
    {
        RaycastHit hit;

        center = CalculateMeshMiddle();
        Vector3 direction = ( center - transform.position ).normalized;

        //SpawnCube(planet.transform.TransformPoint(meshMiddle), direction);

        center = Planet.transform.TransformPoint(center);
        Vector3 raycastStart = center + direction * .005f;

        Debug.DrawRay(raycastStart, direction, Color.red, Mathf.Infinity);

        if (Physics.Raycast(raycastStart, direction, out hit, Mathf.Infinity))
        {
            Tile hitTile = hit.collider.GetComponent<Tile>();
            NextTile = hitTile;
            
            //Do some stuff to tile
        }
    }

    #endregion

    #region Private Methods

    private Vector3 CalculateMeshMiddle()
    {
        Vector3 middle = Vector3.zero;
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        foreach (var vertex in mesh.vertices)
        {
            middle += vertex;
        }

        middle = middle / mesh.vertices.Length;

        return middle;
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