using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	//Board generation
	public Piece[,] pieces = new Piece[8, 8];
	public GameObject whitePiecePrefab;
	public GameObject blackPiecePrefab;
	public Vector3 boardOffset = new Vector3(-4.0f, 0, -4.0f);
	public Vector3 pieceOffset = new Vector3 (0.5f, 0, 0.5f);

	//Mouse
	private Vector2 mouseOver;

	//Piece mechanics
	private Piece selectedPiece;
	private Vector2 startDrag;
	private Vector2 endDrag;

	private void Start()
	{
		GenerateBoard ();
	}
	private void Update(){
		
		MouseOver ();
		//Here will we have tu check if its my turn
		{
			//saving x and y in variables.
			int x = (int)mouseOver.x;
			int y = (int)mouseOver.y;
			//if we click left button
			if(Input.GetMouseButtonDown(0)){
				SelectPiece (x, y);
			}
			//after we select piece, we click where do we want to put it
			if(Input.GetMouseButton(0)){
				AttemptToMove ((int)startDrag.x, (int)startDrag.y, x, y);
			}
		}

	}
	private void MouseOver(){
		RaycastHit hit;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 30.0f, LayerMask.GetMask ("Board"))) {

			mouseOver.x = (int)(hit.point.x - boardOffset.x);
			mouseOver.y = (int)(hit.point.z - boardOffset.z);
		} else {
			mouseOver.x = -1;
			mouseOver.y = -1;
		}
	}

	private void SelectPiece(int x, int y){
		//if x and y is not out of bound
		if (x >= 0 && x <= 7 && y >= 0 && y <= 7) {
			Piece p = pieces [x, y];

			if (p != null) {
				selectedPiece = p;
				startDrag = mouseOver;
				Debug.Log (selectedPiece.name);
			}

		} else
			return;
	}

	private void AttemptToMove(int xS, int yS, int xE, int yE){
		//for multiplayer, we need to redefine those values.
		startDrag = new Vector2 (xS, yS);
		endDrag = new Vector2 (xE, yE);
		selectedPiece = pieces [xS, yS];

		movePiece (selectedPiece, xE, yE);
	}

	private void movePiece(Piece p, int x, int y){
		p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset +pieceOffset;
	}
	/**************************************************************************/
	//Generating the board and pieces
	private void GenerateBoard(){
		//generate White team.
		for (int y = 0; y < 3; y++) {
			bool oddRow = (y % 2 == 0);
			for (int x = 0; x < 8; x += 2) {
				//generate piece
				//ternery operator
				GeneratePiece((oddRow)?x:x+1, y);
			}
		}

		//generate Black team.
		for (int y = 7; y > 4; y--) {
			bool oddRow = (y % 2 == 0);
			for (int x = 0; x < 8; x += 2) {
				//generate piece
				//ternery operator
				GeneratePiece((oddRow)?x:x+1, y);
			}
		}
	}

	private void GeneratePiece(int x, int y){
		bool isWhite = (y > 3) ? false : true;
		GameObject go = Instantiate ((isWhite)?whitePiecePrefab : blackPiecePrefab) as GameObject;
		go.transform.SetParent (transform);
		Piece p = go.GetComponent<Piece> ();
		pieces [x, y] = p;
		movePiece (p, x, y);
	}
}
