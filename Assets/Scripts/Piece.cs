﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

	public bool isWhite;
	public bool isKing;
	public bool isForcedToMove(Piece[,] board, int x, int y){
		if (isWhite || isKing) {
			//Top left 
			if (x >= 2 && y <= 5) {
				//Piece that is top left from selected
				Piece p = board [x - 1, y + 1];
				//If there is a piece, and its not the same colour as ours
				if (p != null && p.isWhite != isWhite) {
					//Check of its possible to land after the jump
					if (board [x - 2, x + 2] == null) {
						return true;
					}
				}
			}
			//Top right
			if (x <= 5 && y <= 5) {
				//Piece that is top right from selected
				Piece p = board [x + 1, y + 1];
				//If there is a piece, and its not the same colour as ours
				if (p != null && p.isWhite != isWhite) {
					//Check of its possible to land after the jump
					if (board [x + 2, x + 2] == null) {
						return true;
					}
				}
			}
		}
		else if (!isWhite || isKing) {
			//Bottom left 
			if (x >= 2 && y >= 2) {
				//Piece that is top left from selected
				Piece p = board [x - 1, y - 1];
				//If there is a piece, and its not the same colour as ours
				if (p != null && p.isWhite != isWhite) {
					//Check of its possible to land after the jump
					if (board [x - 2, x - 2] == null) {
						return true;
					}
				}
			}
			//Top right
			if (x <= 5 && y <= 5) {
				//Piece that is top right from selected
				Piece p = board [x + 1, y - 1];
				//If there is a piece, and its not the same colour as ours
				if (p != null && p.isWhite != isWhite) {
					//Check of its possible to land after the jump
					if (board [x + 2, x - 2] == null) {
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool ValidMove(Piece[,] board, int x1, int y1, int x2, int y2){
		//if you are moving on top of another piece
		if (board [x2, y2] != null)
			return false;
		
		int deltaMove = Mathf.Abs (x1 - x2);
		int deltaMoveY = y2 - y1;
		if (isWhite || isKing) {
			if (deltaMove == 1) {
				//normal move
				if (deltaMoveY == 1) {
					return true;
				}
			}else if(deltaMove == 2){
				//kill move
				if(deltaMoveY == 2){
					//reference, to the piece we are killing(if there is one).
					Piece p = board[(x1 + x2)/2, (y1 + y2)/2];
					//if there is piece to kill and
					//if the piece we are jumping over is different colour then ours
					if(p != null && p.isWhite != isWhite){
						return true;
					}
				}
			}
		}
		if (!isWhite || isKing) {
			if (deltaMove == 1) {
				//normal move
				if (deltaMoveY == -1) {
					return true;
				}
			}else if(deltaMove == 2){
				//kill move
				if(deltaMoveY == -2){
					//reference, to the piece we are killing(if there is one).
					Piece p = board[(x1 + x2)/2, (y1 + y2)/2];
					//if there is piece to kill and
					//if the piece we are jumping over is different colour then ours
					if(p != null && p.isWhite != isWhite){
						return true;
					}
				}
			}
		}
		return false;
	}
}
