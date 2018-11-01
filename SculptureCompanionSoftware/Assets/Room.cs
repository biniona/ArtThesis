using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	private GameObject wall1;
	private GameObject wall2;
	private GameObject room;
	public GameObject shelf; 
	public GameObject winch; 
	private int numShelvesRange = 15;
	private int numVertexRange = 30;
	private float timeToGo;
	private LineRenderer line;  
	private string[] potentialBrackets = {"b1","b2","b3"};
	private float UPDATETIMECONSTANT = .5f; 
	private float LINETHICKNESS = .15f;
	//private line = room.gameObject.AddComponent<LineRenderer>();


	// Use this for initialization
	void Start () {
		wall1 = GameObject.Find ("wall1"); 
		wall2 = GameObject.Find ("wall2");
		room = GameObject.Find ("Room");
		line = room.GetComponent (typeof(LineRenderer)) as LineRenderer;
		timeToGo = Time.fixedTime + UPDATETIMECONSTANT;

	}

	// Update is called once per frame
	void Update () {

		/*
		 * this is all a bit of a hack, but what its doing
		 * is incrementing time to go by a constant value so that it will 
		 * call this again when that amount of time goes by. also destroy has the same
		 * time delat as the whole code block, so ideally everything will get destroyed
		 * as soon as the functions are called again (so far it works well).
		 */
		if (Time.fixedTime >= timeToGo) {
			//create
			int numShelves = Random.Range (2, numShelvesRange);
			GameObject winchInstance = placeWinch ();
			GameObject[] shelvesInstances = placeShelves (numShelves);
			makeLines (shelvesInstances, winchInstance);
		
			//destroy
			Destroy (winchInstance, UPDATETIMECONSTANT); 
			destroyShelves (shelvesInstances);
			timeToGo = Time.fixedTime + UPDATETIMECONSTANT;
		}
 
	}

	void makeLines(GameObject[] shelvesInstances, GameObject winchInstance){

		line.SetWidth(LINETHICKNESS, LINETHICKNESS);
		int vertexCount = Random.Range (2, numVertexRange);
		line.SetVertexCount(vertexCount);
		createLines (shelvesInstances, vertexCount, winchInstance.transform.position);

	}

	GameObject correctShelf(GameObject shelf){
		Vector3 position = new Vector3 (0, 0, 0);
		Quaternion rotation = Quaternion.Euler (-90, 180, 0);
		shelf.transform.localPosition = position;
		shelf.transform.localRotation = rotation; 
		return shelf;
	}

	GameObject placeWinch(){
		Vector3 randomPos = new Vector3 (Random.Range (2f, 15f),-5.12f, Random.Range (2f, 15f));
		GameObject winchInstance = (GameObject)Instantiate (winch, room.transform); 
		Quaternion rotation = Quaternion.Euler (-90, 0, 0);
		winchInstance.transform.localPosition = randomPos;
		winchInstance.transform.localRotation = rotation; 
		return winchInstance; 
	}
		

	GameObject[] placeShelves(int numShelves){
		GameObject[] shelves = new GameObject[numShelves]; 
		for (int i = 0; i < numShelves; i++) {
			Vector3 randomPos = new Vector3 (Random.Range (-6.65f, 5.3f), Random.Range (-4.64f, 2.32f), 0f);
			while(hittingWall(shelves, i, randomPos)){
				randomPos = new Vector3 (Random.Range (-7.38f, 5.3f), Random.Range (-4.64f, 2.32f), 0f);
			}
			bool Boolean  = (Random.value > 0.5f);
			GameObject newShelf;
			if (Boolean) {
				newShelf = (GameObject) Instantiate(shelf, wall1.transform);
			} else {
				newShelf = (GameObject) Instantiate(shelf, wall2.transform);
			}
			correctShelf (newShelf);
			newShelf.transform.localPosition = randomPos;
			shelves [i] = newShelf;
		}
		return shelves;
	} 

	void destroyShelves(GameObject[] shelves){
		int length = shelves.Length;
		for (int i = 0; i < length; i++) {
			Destroy (shelves[i], UPDATETIMECONSTANT);
		}
	}

	void createLines(GameObject[] shelves, int numLines, Vector3 WinchPos){
		Vector3 winchAdj = new Vector3 (-.5f, 1.5f, -.6f);
		line.SetPosition(0, (WinchPos+winchAdj));
		int length = shelves.Length-1; 
		for (int i = 1; i < numLines; i++) {
			//TODO, UNDERSTAND WHAT THIS DOES
			int nextPoint = Random.Range (0, length);
			int bracket = Random.Range (0, 3);
			line.SetPosition(i, shelves[nextPoint].transform.Find(potentialBrackets[bracket]).position);
		}
	}

	bool hittingWall(GameObject[] shelves, int shelvesInstalled, Vector3 potentialPos){

		for (int i = 0; i < shelvesInstalled; i++) {
			if (shelves [i] != null) {
				Vector3 shelfPos = shelves [i].transform.localPosition;
				if (
					((shelfPos.x + 1.1) > (potentialPos.x)) &&
					((shelfPos.x - 1.1) < (potentialPos.x)) &&
					((shelfPos.y + 3) > (potentialPos.y)) &&
					((shelfPos.y - 3) < (potentialPos.y))) {	
					return true;
				}
			}
		}
		return false; 
	
	}

}
