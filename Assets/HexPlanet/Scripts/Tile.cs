using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//Hex Sphere Tile
public class Tile : MonoBehaviour {

	private static int ID = 0;
	private static float tileRadius = .8f;
	
	//Tile Attributes
	public Vector3 location;
	
	private Vector3 center;
	private int color;
	private int id;
	private GameObject[] Neighbors;
	private List<Tile> neighborTiles;
	
	private float maxTileRadius;
	private const float tileAlpha = .75f;
	private static Color[] colors = new Color[]{new Color(1f, 1f, 1f, 0f), new Color(1f, 0.235f, 0f, tileAlpha), new Color(0.51f, 0.137f, 0.725f, tileAlpha), new Color(0.294f, 0.725f, 0f, tileAlpha), new Color(1f, .5f, 0f, tileAlpha)};
	private Color defaultColor = Color.white;
	private Color startColor;
	private Color hilightColor;


	public void Initialize(Vector3 coordinates){
		center = coordinates;
		id = ID;
		ID++;
	}

	void OnMouseEnter(){
		Pointer.instance.setPointer (PointerStatus.TILE, center);
		
	}
	void OnMouseExit(){
		Pointer.instance.unsetPointer ();
	}
	
	void OnMouseDown(){

	}

	//NEW FIND NEIGHBORS
	public void FindNeighbors(){
		//Extend a sphere around this tile to find all adjacent tiles within the spheres radius
		Collider[] neighbors = Physics.OverlapSphere (center, maxTileRadius);
		//OverlapSphere detects the current tile so we must omit this tile from the list
		Neighbors = new GameObject[neighbors.Length - 1];
		neighborTiles = new List<Tile> ();
		int j = 0;
		for(int i = 0; i < neighbors.Length; i++){
			if(neighbors[i] != this.GetComponent<Collider>()){
				Neighbors[j] = neighbors [i].gameObject;
				neighborTiles.Add(neighbors[i].gameObject.GetComponent<Tile>());
				j++;
			}
		}
	}

	public void setColor(int col){
		color = col;
		if (color == 0) {
			GetComponent<Renderer>().material.color = colors[0];
		}
		else{
			GetComponent<Renderer>().material.color = colors[color];
		}
	}

	public void setCenter(Vector3 c){
		this.center = c;
	}
	public void setTileRadius(float r){
		this.maxTileRadius = r;
	}

	public int getID(){
		return id;
	}
	public int getColor(){
		return color;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
