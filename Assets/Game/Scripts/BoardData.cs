using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardData : MonoBehaviour
{
    [System.NonSerialized]
    public Vector2[] boardPositions = new Vector2[40] 
    {
        new Vector2(2.05f, -2.05f), new Vector2(1.5f, -2.05f), new Vector2(1.15f, -2.05f), new Vector2(0.75f, -2.05f), new Vector2(0.38f, -2.05f),
        new Vector2(0, -2.05f), new Vector2(-0.38f, -2.05f), new Vector2(-0.75f, -2.05f), new Vector2(-1.15f, -2.05f), new Vector2(-1.5f, -2.05f),
        new Vector2(-2.05f, -2.05f), new Vector2(-2.05f, -1.5f), new Vector2(-2.05f, -1.15f), new Vector2(-2.05f, -0.75f), new Vector2(-2.05f, -0.38f),
        new Vector2(-2.05f, 0), new Vector2(-2.05f, 0.38f), new Vector2(-2.05f, 0.75f), new Vector2(-2.05f, 1.15f), new Vector2(-2.05f, 1.5f),
        new Vector2(-2.05f, 2.05f), new Vector2(-1.5f, 2.05f), new Vector2(-1.15f, 2.05f), new Vector2(-0.75f, 2.05f), new Vector2(-0.38f, 2.05f),
        new Vector2(0, 2.05f), new Vector2(0.38f, 2.05f), new Vector2(0.75f, 2.05f), new Vector2(1.15f, 2.05f), new Vector2(1.5f, 2.05f),
        new Vector2(2.05f, 2.05f), new Vector2(2.05f, 1.5f), new Vector2(2.05f, 1.15f), new Vector2(2.05f, 0.75f), new Vector2(2.05f, 0.38f),
        new Vector2(2.05f, 0), new Vector2(2.05f, -0.38f), new Vector2(2.05f, -0.75f), new Vector2(2.05f, -1.15f), new Vector2(2.05f, -1.5f)
    };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
