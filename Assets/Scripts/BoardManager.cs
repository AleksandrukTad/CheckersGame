using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	//Board generation
	public Piece[,] board = new Piece[8, 8];
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

	//did piece, killed other piece?
	private bool killed = false;

	private void Start()
	{
		GenerateBoard ();
	}
	private void Update(){
		
		MouseOver ();
		//Here will we have tu check if its my turn
		{
			if (selectedPiece != null) {
				PieceDraging (selectedPiece);
			}
			//saving x and y in variables.
			int x = (int)mouseOver.x;
			int y = (int)mouseOver.y;
			//if we click left button
			if(Input.GetMouseButtonDown(0)){
				SelectPiece (x, y);
			}
			//after we select piece, we click where do we want to put it
			if(Input.GetMouseButtonUp(0)){
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
		if (x >= 0 && x <= 7 && y >= 0 && y <= 7) 
		{
			Piece p = board [x, y];

			if (p != null) 
			{
				selectedPiece = p;
				startDrag = mouseOver;
				Debug.Log ("piece selected");
			}
		} 
	}
/**************************************************************************/
//UTILS


/**************************************************************************/
//MOVEMENT
	private void AttemptToMove(int xS, int yS, int xE, int yE){
		//for multiplayer, we need to redefine those values.
		startDrag = new Vector2 (xS, yS);
		endDrag = new Vector2 (xE, yE);
		selectedPiece = board [xS, yS];


		//if its out of bond
		if(xE < 0 || xE > 7 || yE < 0 || yE > 7)
		{
			if (selectedPiece != null) {
				movePiece (selectedPiece, (int)startDrag.x, (int)startDrag.y);
			}
			startDrag = Vector2.zero;
			selectedPiece = null;
			Debug.Log ("Put back, because of button up was out of bound");
			return;
		}
		//If the move is valid according to checkIfValidMove function
		if (selectedPiece.checkIfValidMove (board, xS, yS, xE, yE, out killed)) 
		{
			board [xE, yE] = board [xS, yS];
			board [xS, yS] = null;
			movePiece (selectedPiece, (int)endDrag.x, (int)endDrag.y);
			if (killed == true) {
				int midX = (xS + xE) / 2;
				int midY = (yS + yE) / 2;
				Piece killedPiece = board [midX, midY];
				board [midX, midY] = null;
				Destroy (killedPiece.gameObject);
			}
			EndTurn ();
			return;
		}
		//if the move is not valid, put back piece
		//This also handles, putting up and putting back in the same place
		else 
		{
			if (selectedPiece != null) {
				movePiece (selectedPiece, (int)startDrag.x, (int)startDrag.y);
			}
			startDrag = Vector2.zero;
			selectedPiece = null;
			Debug.Log ("Put back piece, because of invalid move or picked up and dropped");
			return;
		}
	}

	private void movePiece(Piece p, int x, int y){
		p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset +pieceOffset;
	}

/**************************************************************************/
//DRAGING
	//function to create an "animation" of draging
	private void PieceDraging(Piece p){
		
		RaycastHit hit;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 30.0f, LayerMask.GetMask ("Board"))) 
		{
			//this makes piece to move with mouse and elevates it.
			p.transform.position = hit.point + Vector3.up;
		}
	}
/**************************************************************************/
//GENERATE BOARD
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
		board [x, y] = p;
		movePiece (p, x, y);
	}

/**************************************************************************/
//END TURN
	private void EndTurn()
	{
		endDrag = Vector2.zero;
		startDrag = Vector2.zero;
		selectedPiece = null;
		killed = false;
	}
}
