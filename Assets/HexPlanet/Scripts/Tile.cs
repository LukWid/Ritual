using System.Collections;
using DG.Tweening;
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

    #region Private Fields

    private Quaternion tileRotation;

    private Mesh originalMesh;
    [SerializeField]
    private Mesh tileMesh;
    private Vector3[] vertices;
    private int[] triangles;

    #endregion

    public Color EditorPlanetColor;
    private MeshCollider meshCollider;

    #region Public Properties

    public Vector3 Center { get => center; set => center = value; }
    public Tile NextTile { get => nextTile; set => nextTile = value; }
    public Planet Planet { get => planet; set => planet = value; }
    public Tile PreviousTile { get => previousTile; set => previousTile = value; }
    public Quaternion TileRotation { get => tileRotation; set => tileRotation = value; }
    public Mesh TileMesh => tileMesh;

    #endregion

    #region Unity Callbacks

    private void OnMouseEnter()
    {
//        Pointer.instance.setPointer(PointerStatus.TILE, Center);
    }

    private void OnMouseExit()
    {
  //      Pointer.instance.unsetPointer();
    }

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = GetComponent<MeshFilter>().mesh;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = EditorPlanetColor;
        Gizmos.DrawMesh(GetComponent<MeshFilter>().sharedMesh, Planet.transform.position, Planet.transform.rotation, Planet.transform.localScale);
    }

    #endregion

    #region Public Methods

    private void Start()
    {
        CloneMesh();
    }

    public void Initialize()
    {
        Center = CalculateMeshMiddle();
        TileRotation = GetTileRotation();
    }

    public void SetMaterial(Material material)
    {
        GetComponent<Renderer>().sharedMaterial = material;
    }

    public void GetNextTile()
    {
        RaycastHit hit;

        Vector3 planetCenter = Planet.transform.position;
        Vector3 direction = ( center - planetCenter ).normalized;

        //SpawnCube(center, direction);

        Vector3 rayCastStart = center + direction * 0.005f;

        Debug.DrawRay(rayCastStart, direction, Color.red, .01f);

        if (Physics.Raycast(rayCastStart, direction, out hit, Mathf.Infinity))
        {
            Tile hitTile = hit.collider.GetComponent<Tile>();
            NextTile = hitTile;
            hitTile.PreviousTile = this;
            hitTile.EditorPlanetColor = EditorPlanetColor;
        }
    }

    #endregion

    #region Private Methods

    private void CloneMesh()
    {
        var meshFilter = GetComponent<MeshFilter>();
        originalMesh = meshFilter.sharedMesh;
        tileMesh = new Mesh
        {
            name = "Cloned",
            vertices = originalMesh.vertices,
            triangles = originalMesh.triangles,
            normals = originalMesh.normals,
            uv = originalMesh.uv
        };

        meshFilter.mesh = TileMesh;

        vertices = TileMesh.vertices;
        triangles = TileMesh.triangles;
    }

    private Quaternion GetTileRotation()
    {
        Vector3 planetCenter = Planet.transform.position;
        center = CalculateMeshMiddle();
        Vector3 direction = ( center - planetCenter ).normalized;

        return Quaternion.LookRotation(direction, transform.up);
    }

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

    public void MorphToNextTile()
    {
        var thisVertices = tileMesh.vertices;
        var targetVertices = NextTile.tileMesh.vertices;
        
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        StartCoroutine(SmoothTransition(thisVertices, targetVertices, mesh));
    }

    private void SpawnCube(Vector3 position, Vector3 direction)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = Vector3.one * 0.2f;
        cube.transform.position = position;
        cube.transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }

    private IEnumerator SmoothTransition(Vector3[] thisVertices, Vector3[] targetVertices, Mesh mesh)
    {
        for (int t = 0; t < 100; t++)
        {
            for (int i = 0; i < thisVertices.Length; i++)
            {
                Vector3 from = transform.TransformPoint(thisVertices[i]);
                Vector3 to = nextTile.transform.TransformPoint(targetVertices[i]);

                Vector3 between = Vector3.Lerp(from, to, t / 100.0f);

                thisVertices[i] = transform.InverseTransformPoint(between);
            }

            mesh.vertices = thisVertices;
            mesh.RecalculateNormals();
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = mesh;

            yield return null;
        }
    }

    #endregion
}