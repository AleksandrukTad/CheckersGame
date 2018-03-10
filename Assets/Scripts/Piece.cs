using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

	public int x;
	public int y;
    public bool isQueen;
	public bool isWhite;

    public bool CheckIfCanBeQueen()
    {
        if (((this.y == 0 && !this.isWhite) || (this.y == 7 && this.isWhite)) && !this.isQueen)
        {
            return true;
        }
        return false;
    }
    public void TurnIntoQueen(Piece[,] board)
    {
        board[this.x, this.y].isQueen = true;
        this.transform.Rotate(Vector3.right * 180);
    }
    //legacy code
    /*
	public bool checkIfValidMove(Piece[,] board, int xS, int yS, int xE, int yE, bool multipleMove, out Piece p){
		p = null;
		//If we try to place piece, on top of another piece.
		if (board [xE, yE] != null) {
			Debug.Log ("Trying to place, piece on top of the other piece");
			return false;
		}

		//INTERNATIONAL RULES
		//Checks if we move piece diagonally
		int deltaX = Mathf.Abs(xE - xS);
		int deltaY = yE - yS;

		if (multipleMove == false) 
		{
			if (isWhite && !isQueen) 
				return checkWhitePiece (board, xS, yS, xE, yE, out p, deltaX, deltaY);
			else if (!isWhite && !isQueen) 
				return checkBlackPiece (board, xS, yS, xE, yE, out p, deltaX, deltaY);
			if (isQueen) 
				return checkQueen (board, xS, yS, xE, yE, out p, multipleMove);

		} 
		else if (multipleMove == true) 
		{
			if (!isQueen)
				return checkMultipleMovePiece (board, xS, yS, xE, yE, out p, deltaX, deltaY);
			else
				return checkQueen (board, xS, yS, xE, yE, out p, multipleMove);
		}
		return false;	
	}

	private bool checkWhitePiece(Piece[,] board, int xS, int yS, int xE, int yE, out Piece p, int deltaX, int deltaY)
	{
		p = null;
		if (deltaX == 1)
		{
			if (deltaY == 1)
			{
				return true;
			}
		}
		else if (deltaX == 2)
		{   //possible jump and kill
			if(deltaY == 2)
			{	//count middle point between start and end Drag
				int midX = (xS + xE) / 2;
				int midY = (yS + yE) / 2;
				//if this point is not null and its not the same colour as selected piece
				if (board [midX, midY] != null && board [midX, midY].isWhite != isWhite) {
					//kill
					p = board [midX, midY];
					return true;
				}
			}
			if(deltaY == -2)
			{	//count middle point between start and end Drag
				int midX = (xS + xE) / 2;
				int midY = (yS + yE) / 2;
				//if this point is not null and its not the same colour as selected piece
				if (board [midX, midY] != null && board [midX, midY].isWhite != isWhite) {
					//kill
					p = board [midX, midY];
					return true;
				}
			}
		}
		return false;
	}
	private bool checkBlackPiece(Piece[,] board, int xS, int yS, int xE, int yE, out Piece p, int deltaX, int deltaY)
	{
		p = null;
		if (deltaX == 1)
		{
			if (deltaY == -1){
				return true;
			}
		}
		else if (deltaX == 2)
		{   
			if(deltaY == 2)
			{	//count middle point between start and end Drag
				int midX = (xS + xE) / 2;
				int midY = (yS + yE) / 2;
				//if this point is not null and its not the same colour as selected piece
				if (board [midX, midY] != null && board [midX, midY].isWhite != isWhite) {
					//kill
					p = board [midX, midY];
					return true;
				}
			}
			//possible jump and kill
			if(deltaY == -2)
			{	//count middle point between start and end Drag
				int midX = (xS + xE) / 2;
				int midY = (yS + yE) / 2;
				//if this point is not null and its not the same colour as selected piece
				if (board [midX, midY] != null && board [midX, midY].isWhite != isWhite) {
					//kill
					p = board [midX, midY];
					return true;
				}
			}
		}
		return false;
	}
	private bool checkQueen(Piece[,] board, int xS, int yS, int xE, int yE, out Piece p, bool multipleMove)
	{
		p = null;
		int y = this.y;
		//diagonaly going up and right
		for (int x = this.x; x <= xE && y <= yE; x++, y++) {
			//if "road" from current location to destiny 
			//is "clear"
			if (board [x, y] == null && !multipleMove) {
				if (x == xE && y == yE) {
					return true;
				}
			}
			//if piece come across, piece which is different colour
			if (board [x, y] != null && board[x,y].isWhite != isWhite) {
				//check if behind this piece is empty place.
				if (board [x + 1, y + 1] == null) {
					//check if player set destination to the field after that piece
					if (xE == x + 1 && yE == y + 1) {
						p = board [x, y];
						return true;
					}
					//if not look if the destination picked by player is valid
					int j = y + 1;
                    //look for the next, piece going top right.
                    for (int i = x + 1; i < 8; i++, j++)
                    {
                        if (board[i, j] != null)
                        {
                            return false;
                        }
                        //check if scanned possition in "landing" position.
                        if (i == xE && j == yE)
                        {
                            p = board[x, y];
                            return true;
                        }

                    }
				} else if (board [x + 1, y + 1] != null) {
					return false;
				}
			}
		}
		y = this.y;
		//diagonaly going up and left
		for (int x = this.x; x >= xE && y <= yE; x--, y++) {
			//if "road" from current location to destiny 
			//is "clear"
			if (board [x, y] == null && !multipleMove) {
				if (x == xE && y == yE) {
					return true;
				}
			}
			//if piece come across, piece which is different colour
			if (board [x, y] != null && board[x,y].isWhite != isWhite) {
				//check if behind this piece is empty place.
				//and check if player set destination to that field.
				if (board [x - 1, y + 1] == null) {
					if (xE == x - 1 && yE == y + 1) {
						p = board [x, y];
						return true;
					}
					int j = y + 1;
					//look for the next, piece going top right.
					for (int i = x - 1; board [i, j] == null; i--, j++) {
                        if (board[i, j] != null)
                        {
                            return false;
                        }
                        //check if scanned possition in "landing" position.
                        if (i == xE && j == yE) {
							p = board [x, y];
							return true;
						}
					}
					//To prevent jumping over two pieces
				}else if (board [x + 1, y + 1] != null) {
					return false;
				}
			}
		}
		y = this.y;
		//diagonaly going down and right
		for (int x = this.x; x <= xE && y >= yE; x++, y--) {
			//if "road" from current location to destiny 
			//is "clear"
			if (board [x, y] == null && !multipleMove) {
				if (x == xE && y == yE) {
					return true;
				}
			}
			//if piece come across, piece which is different colour
			if (board [x, y] != null && board [x, y].isWhite != isWhite) {
				//check if behind this piece is empty place.
				//and check if player set destination to that field.
				if (board [x + 1, y - 1] == null) {
					if (xE == x + 1 && yE == y - 1) {
						p = board [x, y];
						return true;
					}
					int j = y + 1;
					//look for the next, piece going top right.
					for (int i = x + 1; board [i, j] == null; i++, j--) {
                        if (board[i, j] != null)
                        {
                            return false;
                        }
                        //check if scanned possition in "landing" position.
                        if (i == xE && j == yE) {
							p = board [x, y];
							return true;
						}
					}
				} else if (board [x + 1, y + 1] != null) {
					return false;
				}
			}
		}
		y = this.y;
		//diagonaly going down and left
		for (int x = this.x; x >= xE && y >= yE; x--, y--) {
			//if "road" from current location to destiny 
			//is "clear"
			if (board [x, y] == null && !multipleMove) {
				if (x == xE && y == yE) {
					return true;
				}
			}
			//if piece come across, piece which is different colour
			if (board [x, y] != null && board [x, y].isWhite != isWhite) {
				//check if behind this piece is empty place.
				//and check if player set destination to that field.
				if (board [x - 1, y - 1] == null) {
					if (xE == x - 1 && yE == y - 1) {
						p = board [x, y];
						return true;
					}
					int j = y - 1;
					//look for the next, piece going down left.
					for (int i = x - 1; board [i, j] == null; i--, j--) {
                        if (board[i, j] != null)
                        {
                            return false;
                        }
                        //check if scanned possition in "landing" position.
                        if (i == xE && j == yE) {
							p = board [x, y];
							return true;
						}
					}
				} else if (board [x + 1, y + 1] != null) {
					return false;
				}
			}
		}
		return false;
	}
	private bool checkMultipleMovePiece(Piece[,] board, int xS, int yS, int xE, int yE, out Piece p, int deltaX, int deltaY){
		p = null;
		if (deltaX == 2)
		{   //possible jump and kill
			if(deltaY == 2)
			{	//count middle point between start and end Drag
				int midX = (xS + xE) / 2;
				int midY = (yS + yE) / 2;
				//if this point is not null and its not the same colour as selected piece
				if (board [midX, midY] != null && board [midX, midY].isWhite != isWhite) {
					//kill
					p = board [midX, midY];
					return true;
				}
			}
			if(deltaY == -2)
			{	//count middle point between start and end Drag
				int midX = (xS + xE) / 2;
				int midY = (yS + yE) / 2;
				//if this point is not null and its not the same colour as selected piece
				if (board [midX, midY] != null && board [midX, midY].isWhite != isWhite) {
					//kill
					p = board [midX, midY];
					return true;
				}
			}
		}
		return false;
	}
    */
}
