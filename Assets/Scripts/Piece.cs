using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

	private bool isQueen;
	public bool isWhite;

	public bool checkIfValidMove(Piece[,] board, int xS, int yS, int xE, int yE){
		//If we try to place piece, on top of another piece.
		if(board[xE, yE] != null)
			return false;

		//Checks if we move piece diagonally
		int deltaX = Mathf.Abs(xE - xS);
		int deltaY = yE - yS;

        if (isWhite) {
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
					if(board[midX, midY] != null || board[midX, midY].isWhite != isWhite)
						//kill
                    	return true;
                }
            }
        }
        else if (!isWhite)
        {
            if (deltaX == 1)
            {
                if (deltaY == -1)
                {
                    return true;
                }
            }
            else if(deltaX == 2)
            {
                //jump and kill
                if(deltaY == -2)
                {
                    return true;
                }
            }
        }
		return false;	
	}
}
