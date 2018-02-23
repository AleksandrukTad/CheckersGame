using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

	public int x;
	public int y;
	public bool isQueen;
	public bool isWhite;

	public bool checkIfValidMove(Piece[,] board, int xS, int yS, int xE, int yE, out Piece p){
		p = null;
		//If we try to place piece, on top of another piece.
		if (board [xE, yE] != null) {
			Debug.Log ("Trying to place, piece on top of the other piece");
			return false;
		}

		//INTERNATIONAL RULES
		//Checks if we move piece diagonally
		int deltaX = Mathf.Abs(xE - xS);
		int Y = yE - yS;

		if (isWhite && !isQueen) {
            if (deltaX == 1)
            {
                if (Y == 1)
                {
                    return true;
                }
            }
            else if (deltaX == 2)
            {   //possible jump and kill
                if(Y == 2)
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
				if(Y == -2)
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

        }
		else if (!isWhite && !isQueen)
        {
			if (deltaX == 1)
			{
				if (Y == -1){
					return true;
				}
			}
			else if (deltaX == 2)
			{   
				if(Y == 2)
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
				if(Y == -2)
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
        }
		//INTERNATIONAL RULES
		if (isQueen) {

			int y = this.y;
			//diagonaly going up and right
			for (int x = this.x; x <= xE && y <= yE; x++, y++) {
				//if "road" from current location to destiny 
				//is "clear"
				if (board [x, y] == null) {
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
						for (int i = x + 1; board[i,j] == null; i++, j++) {
								//check if scanned possition in "landing" position.
							if (i == xE && j == yE) {
								p = board [x, y];
								return true;
							}
						}
					}
				}
			}
			y = this.y;
			//diagonaly going up and left
			for (int x = this.x; x >= xE && y <= yE; x--, y++) {
				//if "road" from current location to destiny 
				//is "clear"
				if (board [x, y] == null) {
					if (x == xE && y == yE) {
						return true;
					}
				}
				//if piece come across, piece which is different colour
				if (board [x, y] != null && board[x,y].isWhite != isWhite) {
					//check if behind this piece is empty place.
					//and check if player set destination to that field.
					if (board [x - 1, y + 1] == null && xE == x - 1 && yE == y +1) {
						p = board [x, y];
						return true;
					}
					int j = y + 1;
					//look for the next, piece going top right.
					for (int i = x - 1; board[i,j] == null; i--, j++) {
						//check if scanned possition in "landing" position.
						if (i == xE && j == yE) {
							p = board [x, y];
							return true;
						}
					}
				}
			}
			y = this.y;
			//diagonaly going down and right
			for (int x = this.x; x <= xE && y >= yE; x++, y--) {
				//if "road" from current location to destiny 
				//is "clear"
				if (board [x, y] == null) {
					if (x == xE && y == yE) {
						return true;
					}
				}
				//if piece come across, piece which is different colour
				if (board [x, y] != null && board[x,y].isWhite != isWhite) {
					//check if behind this piece is empty place.
					//and check if player set destination to that field.
					if (board [x + 1, y - 1] == null && xE == x + 1 && yE == y - 1) {
						p = board [x, y];
						return true;
					}
					int j = y + 1;
					//look for the next, piece going top right.
					for (int i = x + 1; board[i,j] == null; i++, j--) {
						//check if scanned possition in "landing" position.
						if (i == xE && j == yE) {
							p = board [x, y];
							return true;
						}
					}
				}
			}
			y = this.y;
			//diagonaly going down and left
			for (int x = this.x; x >= xE && y >= yE; x--, y--) {
				//if "road" from current location to destiny 
				//is "clear"
				if (board [x, y] == null) {
					if (x == xE && y == yE) {
						return true;
					}
				}
				//if piece come across, piece which is different colour
				if (board [x, y] != null && board[x,y].isWhite != isWhite) {
					//check if behind this piece is empty place.
					//and check if player set destination to that field.
					if (board [x - 1, y - 1] == null && xE == x - 1 && yE == y - 1) {
						p = board [x, y];
						return true;
					}
					int j = y - 1;
					//look for the next, piece going down left.
					for (int i = x - 1; board[i,j] == null; i--, j--) {
						//check if scanned possition in "landing" position.
						if (i == xE && j == yE) {
							p = board [x, y];
							return true;
						}
					}
				}
			}
		}
		return false;	
	}
}
