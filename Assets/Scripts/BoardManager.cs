using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	private List<Piece> forcedToMove; 

	//did piece, killed other piece?
	private bool killed = false;

	//Turns
	public bool isWhiteTurn = true;

	private void Start()
	{
		GenerateBoard ();
		Scan ();
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
			//When forcedToMove is not empty, we have to check if piece which is going to be selected,
			//is inside forcedToMove.
			if (forcedToMove.Count != 0) {
				if (p != null && p.isWhite == isWhiteTurn && forcedToMove.Any (piece => piece == p)) {
					selectedPiece = p;
					startDrag = new Vector2 (selectedPiece.x, selectedPiece.y);
					Debug.Log ("piece selected");
				}
			}
			//if forcedToMove is empty then we can pick whatever we want!
			else {
				if (p != null && p.isWhite == isWhiteTurn) {
					selectedPiece = p;
					startDrag = new Vector2 (selectedPiece.x, selectedPiece.y);
					Debug.Log ("piece selected");
				}
			}
		} 
	}
/**************************************************************************/
//UTILS
	//Scan whole board to look for piece, that can kill
	private void Scan(){
		forcedToMove = new List<Piece> ();
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				//check if scan will not go out of bounds
				if (board [j, i] != null) {
					//Debug.Log ("Board " + j + ":" + i);
					//if its white turn
					if (isWhiteTurn && board[j,i].isWhite) {
						//if there is a piece
						//check if the top right exists and its different colour
						if (j + 1 <= 7 && i + 1 <= 7) {
							if (board [j + 1, i + 1] != null && board [j, i].isWhite != board [j + 1, i + 1].isWhite) {
								//check if next piece, in top right exists if not
								//we are able to kill
								if (j + 2 <= 7 && i + 2 <= 7) {
									if (board [j + 2, i + 2] == null) {
										forcedToMove.Add (board [j, i]);
									}
								}
							}
						}
						//check if the top left exists and its different colour
						if (j - 1 >= 0 && i + 1 <= 7) {  
							if (board [j - 1, i + 1] != null && board [j, i].isWhite != board [j - 1, i + 1].isWhite) {
								//check if next piece, in top right exists if not
								//we are able to kill
								if (j - 2 >= 0 && i + 2 <= 7) {
									if (board [j - 2, i + 2] == null) {
										forcedToMove.Add (board [j, i]);
									}
								}
							}
						}
					} else if (!isWhiteTurn && !board[j,i].isWhite) {
						//if there is a piece
						//check if the bottom right exists and its different colour
						if (j + 1 <= 7 && i - 1 >= 0) {
							if (board [j + 1, i - 1] != null && board [j, i].isWhite != board [j + 1, i - 1].isWhite) {
								//check if next piece, in top right exists if not
								//we are able to kill
								if (j + 2 <= 7 && i - 2 >= 0) {
									if (board [j + 2, i - 2] == null) {
										forcedToMove.Add (board [j, i]);
									}
								}
							}
						}
						//check if the bottom left exists and its different colour
						if (j - 1 >= 0 && i - 1 >= 0) {  
							if (board [j - 1, i - 1] != null && board [j, i].isWhite != board [j - 1, i - 1].isWhite) {
								//check if next piece, in top right exists if not
								//we are able to kill
								if (j - 2 >= 0 && i - 2 >= 0) {
									if (board [j - 2, i - 2] == null) {
										forcedToMove.Add (board [j, i]);
									}
								}
							}
						}
					}
				}
			}
		}
	}
	//Check if piece, which just killed can kill again
	private void Scan(Piece p){
		forcedToMove = new List<Piece> ();

		if (isWhiteTurn && p.isWhite) {
			//if top right is not out of bound
			if(p.x + 1 <= 7 && p.y + 1 <= 7){
				//if top right is not null AND top right from investigated has different colour from investigated 
				if(board[p.x + 1, p.y + 1] != null && board[p.x + 1, p.y + 1].isWhite != p.isWhite){
					if (p.x + 2 <= 7 && p.y + 2 <= 7) {
						if (board [p.x + 1, p.y + 1] == null) {
							forcedToMove.Add (p);
						}
					}
				}
			}
			//if top left is not out of bound
			if(p.x - 1 >= 0 && p.y + 1 <= 7){
				//if top left is not null AND top right from investigated has different colour from investigated 
				if(board[p.x - 1, p.y + 1] != null && board[p.x - 1, p.y + 1].isWhite != p.isWhite){
					if (p.x - 2 >= 0 && p.y + 2 <= 7) {
						if (board [p.x - 2, p.y + 2] == null) {
							forcedToMove.Add (p);
						}
					}
				}
			}
		}
		else if (!isWhiteTurn && !p.isWhite) {
			//if bottom right is not out of bound
			if(p.x + 1 <= 7 && p.y - 1 >= 0){
				//if bottom right is not null AND top right from investigated has different colour from investigated 
				if(board[p.x + 1, p.y - 1] != null && board[p.x + 1, p.y - 1].isWhite != p.isWhite){
					if (p.x + 2 <= 7 && p.y - 2 >= 0) {
						if (board [p.x + 1, p.y - 1] == null) {
							forcedToMove.Add (p);
						}
					}
				}
			}
			//if bottom left is not out of bound
			if(p.x - 1 >= 0 && p.y - 1 >= 0){
				//if top left is not null AND top right from investigated has different colour from investigated 
				if(board[p.x - 1, p.y - 1] != null && board[p.x - 1, p.y - 1].isWhite != p.isWhite){
					if (p.x - 2 >= 0 && p.y - 2 >= 0) {
						if (board [p.x - 2, p.y - 2] == null) {
							forcedToMove.Add (p);
						}
					}
				}
			}
		}
	}
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
			//changing x, y for piece object
			selectedPiece.x = xE;
			selectedPiece.y = yE;
			//changing x,y for board 
			board [xE, yE] = selectedPiece;
			board [xS, yS] = null;

			movePiece (selectedPiece, (int)endDrag.x, (int)endDrag.y);
			if (killed == true) {
				int midX = (xS + xE) / 2;
				int midY = (yS + yE) / 2;
				Piece killedPiece = board [midX, midY];
				board [midX, midY] = null;
				Destroy (killedPiece.gameObject);
				startDrag = Vector2.zero;
				Scan (selectedPiece);
				selectedPiece = null;
				if (forcedToMove.Count == 0) {
					EndTurn();
				}
				Debug.Log (forcedToMove.Count);
				return;
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
		p.x = x;
		p.y = y;
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

		if (isWhiteTurn == true) {
			isWhiteTurn = false;
		} else {
			isWhiteTurn = true;
		}
		Scan ();
		Debug.Log (forcedToMove.Count);
	}
}
