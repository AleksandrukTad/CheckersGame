using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

	public int x;
	public int y;
	public bool isQueen;
	public bool isWhite;

	public bool checkIfValidMove(Piece[,] board, int xS, int yS, int xE, int yE, out bool killed, out Piece p){
		p = null;
		//If we try to place piece, on top of another piece.
		if (board [xE, yE] != null) {
			Debug.Log ("Trying to place, piece on top of the other piece");
			killed = false;
			return false;
		}
		//Checks if we move piece diagonally
		int deltaX = Mathf.Abs(xE - xS);
		int Y = yE - yS;

		if (isWhite && !isQueen) {
            if (deltaX == 1)
            {
                if (Y == 1)
                {
					killed = false;
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
					if (board [midX, midY] != null || board [midX, midY].isWhite != isWhite) {
						//kill
						p = board [midX, midY];
						killed = true;
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
					killed = false;
					return true;
				}
			}
			else if (deltaX == 2)
			{   //possible jump and kill
				if(Y == -2)
				{	//count middle point between start and end Drag
					int midX = (xS + xE) / 2;
					int midY = (yS + yE) / 2;
					//if this point is not null and its not the same colour as selected piece
					if (board [midX, midY] != null || board [midX, midY].isWhite != isWhite) {
						//kill
						p = board [midX, midY];
						killed = true;
						return true;
					}
				}
			}
        }
		if (isQueen) {

			int y = this.y;

			//diagonaly going up and right
			for (int x = this.x; x <= xE && y <= yE; x++, y++) {
				//if "road" from current location to destiny 
				//is "clear"
				if (board [x, y] == null) {
					if (x == xE && y == yE) {
						killed = false;
						return true;
					}
				}
				//if piece come across, piece which is different colour
				if (board [x, y] != null && board[x,y].isWhite != isWhite) {
					//check if behind this piece is empty place.
					//and check if player set destination to that field.
					if (board [x + 1, y + 1] == null && xE == x + 1 && yE == y +1) {
						killed = true;
						p = board [x, y];
						return true;
					}
				}
			}
		}
		killed = false;
		return false;	
	}
}
