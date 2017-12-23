using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public Piece[,] pieces = new Piece[8, 8];
	public GameObject whitePiecePrefab;
	public GameObject blackPiecePrefab;

	public Vector3 boardOffset = new Vector3(-4.0f, 0, -4.0f);
	public Vector3 pieceOffset = new Vector3 (0.5f, 0, 0.5f);

	private bool hasKilled;
	private List<Piece> forcedPieces;
	private bool whiteTurn;
	public bool isWhite;
	private Piece selectedPiece;
	private Vector2 startDrag;
	private Vector2 endDrag;

	private Vector2 mouseOver;

	private void Start()
	{
		forcedPieces = new List<Piece> ();
		whiteTurn = true;
		generateBoard ();
	}

	private void Update()
	{
		updateMouseOver ();

		//If it is my turn
		{
			int x = (int)mouseOver.x;
			int y = (int)mouseOver.y;

			if (selectedPiece != null)
				updatePieceDrag (selectedPiece);

			if (Input.GetMouseButtonDown (0))
				selectPiece (x, y);
			if (Input.GetMouseButtonUp (0))
				TryMove ((int)startDrag.x, (int)startDrag.y, x, y);
		}

	}
	private List<Piece> ScanForPossibleMoves(Piece p, int x, int y){
		if (pieces [x, y].isForcedToMove(pieces, x, y))
			forcedPieces.Add (pieces [x, y]);
		return forcedPieces;
			
	}
	private List<Piece> ScanForPossibleMoves(){
		forcedPieces = new List<Piece>();

		//Checker all the picese
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				if (pieces [i, j] != null && pieces [i, j].isWhite == whiteTurn) {
					if(pieces[i,j].isForcedToMove(pieces, i, j)){
						forcedPieces.Add(pieces[i,j]);
					}
				}
			}
		}
		return forcedPieces;
	}
	private void TryMove(int x1, int y1, int x2, int y2){
		forcedPieces = ScanForPossibleMoves ();
		//Mulitplayer support
		startDrag = new Vector2 (x1, y1);
		endDrag = new Vector2 (x2, y2);
		selectedPiece = pieces [x1, y1];

		//out of bounds
		if (x2 < 0 || x2 >= 8 || y2 < 0 || y2 >= 8) {
			if (selectedPiece != null) {
				movePiece (selectedPiece, x1, y1);
			}
			startDrag = Vector2.zero;
			selectedPiece = null;
			return;
		}
	
		if (selectedPiece != null) {
			//if we did not move
			if (endDrag == startDrag) {
				movePiece (selectedPiece, x1, y1);
				startDrag = Vector2.zero;
				selectedPiece = null;
				return;
			}
			//check if its valid move
			if (selectedPiece.ValidMove(pieces, x1, y1, x2, y2)) {
				if (Mathf.Abs (x1 - x2) == 2) {
					Piece p = pieces[(x1 + x2)/2, (y1 + y2)/2];
					if (p != null) {
						pieces [(x1 + x2) / 2, (y1 + y2) / 2] = null;
						Destroy (p.gameObject);
						hasKilled = true;
					}
				}
				//Are we supposed to kill anything?
				if (forcedPieces.Count != 0 && !hasKilled) {
					movePiece (selectedPiece, x1, y1);
					startDrag = Vector2.zero;
					selectedPiece = null;
					return;
				}

				//making the move in the array.
				pieces [x2, y2] = selectedPiece;
				pieces [x1, y1] = null;

				movePiece (selectedPiece, x2, y2);

				endTurn ();
			}
			else{
				movePiece (selectedPiece, x1, y1);
				startDrag = Vector2.zero;
				selectedPiece = null;
				return;
			}
		}
		//movePiece (selectedPiece, x2, y2);
	}
	private void updatePieceDrag(Piece p){
		RaycastHit hit;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 25.0f, LayerMask.GetMask ("Board"))) {
			p.transform.position = hit.point + Vector3.up;
		}
	}
	private void updateMouseOver(){
		RaycastHit hit;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 25.0f, LayerMask.GetMask ("Board"))) {
			mouseOver.x = (int)(hit.point.x - boardOffset.x);
			mouseOver.y = (int)(hit.point.z - boardOffset.z);
		} else {
			mouseOver.x = -1;
			mouseOver.y = -1;
		}
	}

	private void selectPiece(int x, int y){
		//Out of bounds
		if (x < 0 || y < 0 || x > 8 || y > 8)
			return;
		Piece p = pieces [x, y];
		if (p != null && p.isWhite == isWhite) {

			if (forcedPieces.Count == 0) {
				selectedPiece = p;
				startDrag = mouseOver;
			} else {
				//Look for the piece under our forced pieces list
				if (forcedPieces.Find (fp => fp == p) == null)
					return;
				
				selectedPiece = p;
				startDrag = mouseOver;
			}
		}
	}
	private void generateBoard(){
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

	private void movePiece(Piece p, int x, int y){
		p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset +pieceOffset;
	}

	private void checkVictory(){
	}
	private void endTurn(){
		int x = (int)endDrag.x;
		int y = (int)endDrag.y;

		if (selectedPiece != null) {
			if (selectedPiece.isWhite && !selectedPiece && y == 7) {
				selectedPiece.isKing = true;
				selectedPiece.transform.Rotate (Vector3.right * 180);
			}
			if (!selectedPiece.isWhite && !selectedPiece && y == 0) {
				selectedPiece.isKing = true;
				selectedPiece.transform.Rotate (Vector3.right * 180);
			}
		}
		selectedPiece = null;
		startDrag = Vector2.zero;

		if (ScanForPossibleMoves (selectedPiece, x, y).Count != 0 && hasKilled)
			return;
		whiteTurn = !whiteTurn;
		hasKilled = false;
		checkVictory ();
	}
}
