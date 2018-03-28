using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { set; get; }
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;
    //Board generation


	//Mouse
	private Vector2 mouseOver;

	//Piece mechanics
	private Piece selectedPiece;
	private Vector2 startDrag;
	private Vector2 endDrag;
	private List<Piece> forcedToMove;

 

	//did piece, killed other piece?
	private Piece killedPiece;

	//Turns
	public bool isWhite = true;
	private bool multipleMove = false;

	//online
	private Client client;
	private bool isWhiteTurn;

    [SerializeField]
	private GameObject endScreen;


    private Rules rules;
    private Board board;
    private void Start()
	{
		Instance = this;
		client = FindObjectOfType<Client> ();
		isWhite = client.isHost;
		isWhiteTurn = true;
        board = gameObject.GetComponent<InternationalBoard>();
        rules = new InternationalRules();
        forcedToMove = new List<Piece> ();
        board.GenerateBoard(whitePiecePrefab, blackPiecePrefab);
        forcedToMove = board.ScanForAll(isWhite);

    }
	private void Update(){
		
		MouseOver ();
		//Here will we have tu check if its my turn
		if((isWhite)?isWhiteTurn:!isWhiteTurn)
		{
			if (selectedPiece != null) {
				PieceDragging (selectedPiece);
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
	private void MouseOver(){
		RaycastHit hit;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 30.0f, LayerMask.GetMask ("Board"))) {

			mouseOver.x = (int)(hit.point.x - board.boardOffset.x);
			mouseOver.y = (int)(hit.point.z - board.boardOffset.z);
		} else {
			mouseOver.x = -1;
			mouseOver.y = -1;
		}
	}
	private void SelectPiece(int x, int y){
		//if x and y is not out of bound
		if (x >= 0 && x <= 7 && y >= 0 && y <= 7) 
		{
			//Piece p = board [x, y];
            Piece p = board.board[x, y];
            //When forcedToMove is not empty, we have to check if piece which is going to be selected,
            //is inside forcedToMove.
            if (forcedToMove.Count != 0) {
				if (p != null && p.isWhite == isWhite && forcedToMove.Any (piece => piece == p)) {
					selectedPiece = p;
					startDrag = new Vector2 (selectedPiece.x, selectedPiece.y);
					Debug.Log ("piece selected");
				}
			}
			//if forcedToMove is empty then we can pick whatever we want!
			else {
				if (p != null && p.isWhite == isWhite) {
					selectedPiece = p;
					startDrag = new Vector2 (selectedPiece.x, selectedPiece.y);
					Debug.Log ("piece selected");
				}
			}
		} 
	}
	public void AttemptToMove(int xS, int yS, int xE, int yE){
		//for multiplayer, we need to redefine those values.
		startDrag = new Vector2 (xS, yS);
		endDrag = new Vector2 (xE, yE);
		if (xS > 0 && yS > 0) {
            //selectedPiece = board[xS, yS];
            selectedPiece = board.board [xS, yS];
		}


		//if its out of bond
		if(xE < 0 || xE > 7 || yE < 0 || yE > 7)
		{
			if (selectedPiece != null) {
                board.MovePiece (selectedPiece, (int)startDrag.x, (int)startDrag.y);
			}
			startDrag.x = -1;
			startDrag.y = -1;
			selectedPiece = null;
			Debug.Log ("Put back, because of button up was out of bound");
			return;
		}
		if (selectedPiece != null) {
            //If the move is valid according to checkIfValidMove function
            if(rules.CheckIfValidMove(board.board, selectedPiece, xE, yE, multipleMove, out killedPiece))
            {
                //changing x, y for piece object
                selectedPiece.x = xE;
                selectedPiece.y = yE;
                //changing x,y for board 
                board.board[xE, yE] = selectedPiece;
                board.board[xS, yS] = null;
                board.MovePiece(selectedPiece, (int)endDrag.x, (int)endDrag.y);
                if (killedPiece != null)
                {
                    board.board[killedPiece.x, killedPiece.y] = null;
					DestroyImmediate(killedPiece.gameObject);

                    //piece, killed. Can it become queen?
                    if (selectedPiece.CheckIfCanBeQueen())
                    {
                        selectedPiece.TurnIntoQueen(board.board);
                        //piece cannot move, right after it became queen.
                        selectedPiece = null;

						//sending move and queen info to server

						//
                        EndTurn();
                        return;
                    }
                    killedPiece = null;
                    //to prevent the dragging animation, selectedPiece need to be set as null after kill.
                    Piece placeholderPiece = selectedPiece;
                    selectedPiece = null;
                    //check if there is anything else to kill.

					//sending move to server
					SendData(startDrag, endDrag);
					//
                    forcedToMove = board.ScanForOne(placeholderPiece, isWhite);
                    if (forcedToMove.Count == 0)
                    {
                        multipleMove = false;
                        EndTurn();
                        forcedToMove = board.ScanForAll(isWhite);
                    }
                    else
                    {
                        multipleMove = true;
                    }
                    return;
                }
                //piece did not kill.
                if(selectedPiece.CheckIfCanBeQueen())
					//sending move and queen info to server
					//
                    selectedPiece.TurnIntoQueen(board.board);
				SendData(startDrag, endDrag);
                EndTurn();
                forcedToMove = board.ScanForAll(isWhite);
                return;
            }
            //if the move is not valid, put back piece
            //This also handles, putting up and putting back in the same place
            else
            {
                if (selectedPiece != null)
                {
                    board.MovePiece(selectedPiece, (int)startDrag.x, (int)startDrag.y);
                }
                startDrag.x = -1;
                startDrag.y = -1;
                selectedPiece = null;
                Debug.Log("Put back piece, because of invalid move or picked up and dropped");
                return;
            }
		}
	}
/**************************************************************************/
//DRAGING
	//function to create an "animation" of draging
	private void PieceDragging(Piece p){
		
		RaycastHit hit;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 30.0f, LayerMask.GetMask ("Board"))) 
		{
			//this makes piece to move with mouse and elevates it.
			p.transform.position = hit.point + Vector3.up;
		}
	}
/************************************************************************/
//END TURN
	private void EndTurn()
	{	
		forcedToMove.Clear ();
		multipleMove = false;
		startDrag.x = -1;
		startDrag.y = -1;
		selectedPiece = null;

		isWhiteTurn = !isWhiteTurn;
		//CheckVictory ();
	}
//SEND PIECE
	private void SendData(Vector2 startDrag, Vector2 endDrag)
	{
		string msg = "CMOVE|";
		msg += startDrag.x.ToString() + "|";
		msg += startDrag.y.ToString() + "|";
		msg += endDrag.x.ToString() + "|";
		msg += endDrag.y.ToString ();
		client.Send (msg);
	}
//CHECK VICTORY
	private void CheckVictory(){
        List<Piece> pieces = new List<Piece> ();
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
                //if (board [j, i] != null)
                //	pieces.Add (board [j, i]);
                if (board.board[j, i] != null)
                    pieces.Add(board.board[j, i]);
					
			}
		}
		bool hasWhite = false;
		bool hasBlack = false;

		for (int i = 0; i < pieces.Count; i++) {
			if (pieces [i].isWhite)
				hasWhite = true;
			else if (!pieces [i].isWhite)
				hasBlack = true;
		}
		Debug.Log (pieces.Count);
		if (!hasWhite)
			Victory (false);
		if (!hasBlack)
			Victory (true);
	}
	private void Victory(bool isWhite){
		if (isWhite)
			Debug.Log ("White won!");
		else
			Debug.Log ("Black won!");
		endScreen.SetActive (true);
	}
	public void RedirectToSurvey()
	{
        Debug.Log("Redirect");
		var testing = GameObject.Find ("Surveys").GetComponent<Testing> ();
		testing.redirectToSurvey ();
	}
}
