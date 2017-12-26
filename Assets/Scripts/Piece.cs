using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

	public int x;
	public int y;
	private bool isQueen;
	public bool isWhite;

	public bool checkIfValidMove(Piece[,] board, int xS, int yS, int xE, int yE, out bool killed){
		//If we try to place piece, on top of another piece.
		if (board [xE, yE] != null) {
			Debug.Log ("Trying to place, piece on top of the other piece");
			killed = false;
			return false;
		}

		//Checks if we move piece diagonally
		int deltaX = Mathf.Abs(xE - xS);
		int Y = yE - yS;

        if (isWhite) {
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
						killed = true;
						return true;
					}
                }
            }
        }
        else if (!isWhite)
        {
			if (deltaX == 1)
			{
				if (Y == -1)
				{
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
						killed = true;
						return true;
					}
				}
			}
        }
		killed = false;
		return false;	
	}
}
