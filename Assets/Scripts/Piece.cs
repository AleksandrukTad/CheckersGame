using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

	private bool isQueen;
	public bool isWhite;

	public bool checkIfValidMove(Piece[,] pieces, int xS, int yS, int xE, int yE){
		//If we try to place piece, on top of another piece.
		if(pieces[xE, yE] != null)
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
            {   //jump and kill
                if(deltaY == 2)
                {
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
