using UnityEngine;
using System.Collections;


public class CameraView : MonoBehaviour {

	private string cam = "CameraViews/camera";
	private float timeToGo;
	private float UPDATETIMECONSTANT = 9.9f;
	private static int numCamera = 6; 
	private GameObject[] cameraPositions = new GameObject[numCamera]; 
	private int currCamera = 0; 

	// Use this for initialization
	void Start () {
		//load in all cameras 
		for (int i = 0; i < numCamera; i++) {
			string newCam = cam + i.ToString();
			cameraPositions[i] = (GameObject)Resources.Load(newCam, typeof(GameObject));

		}

		timeToGo = Time.fixedTime + UPDATETIMECONSTANT;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.fixedTime >= timeToGo) {
			if (currCamera >= numCamera)
				currCamera = 0; 
			gameObject.transform.position = cameraPositions[currCamera].transform.position;
			gameObject.transform.rotation = cameraPositions[currCamera].transform.rotation;
			timeToGo = Time.fixedTime + UPDATETIMECONSTANT;
			currCamera++; 
		}
	}
}
