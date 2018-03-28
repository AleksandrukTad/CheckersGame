using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class InternationalBoard : Board
    {
        public InternationalBoard()
        {
            board = new Piece[8, 8];
            pieceList = new List<Piece>();
        }
        public override List<Piece> ScanForAll(bool isWhite)
        {
            pieceList.Clear();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //check if scan will not go out of bounds
                    if (board[j, i] != null && !board[j, i].isQueen && board[j, i].isWhite == isWhite)
                    {
                        //if its white turn
                        //if (isWhiteTurn && board[j,i].isWhite) {
                        //if there is a piece
                        //check if the top right exists and its different colour
                        if (j + 1 <= 7 && i + 1 <= 7)
                        {
                            if (board[j + 1, i + 1] != null && board[j, i].isWhite != board[j + 1, i + 1].isWhite)
                            {
                                //check if next piece, in top right exists if not
                                //we are able to kill
                                if (j + 2 <= 7 && i + 2 <= 7)
                                {
                                    if (board[j + 2, i + 2] == null && isWhite == board[j, i].isWhite)
                                    {
                                        pieceList.Add(board[j, i]);
                                    }
                                }
                            }
                        }
                        //check if the top left exists and its different colour
                        if (j - 1 >= 0 && i + 1 <= 7)
                        {
                            if (board[j - 1, i + 1] != null && board[j, i].isWhite != board[j - 1, i + 1].isWhite)
                            {
                                //check if next piece, in top right exists if not
                                //we are able to kill
                                if (j - 2 >= 0 && i + 2 <= 7)
                                {
                                    if (board[j - 2, i + 2] == null && isWhite == board[j, i].isWhite)
                                    {
                                        pieceList.Add(board[j, i]);
                                    }
                                }
                            }
                        }
                        //if there is a piece
                        //check if the bottom right exists and its different colour
                        if (j + 1 <= 7 && i - 1 >= 0)
                        {
                            if (board[j + 1, i - 1] != null && board[j, i].isWhite != board[j + 1, i - 1].isWhite)
                            {
                                //check if next piece, in top right exists if not
                                //we are able to kill
                                if (j + 2 <= 7 && i - 2 >= 0)
                                {
                                    if (board[j + 2, i - 2] == null && isWhite == board[j, i].isWhite)
                                    {
                                        pieceList.Add(board[j, i]);
                                    }
                                }
                            }
                        }
                        //check if the bottom left exists and its different colour
                        if (j - 1 >= 0 && i - 1 >= 0)
                        {
                            if (board[j - 1, i - 1] != null && board[j, i].isWhite != board[j - 1, i - 1].isWhite)
                            {
                                //check if next piece, in top right exists if not
                                //we are able to kill
                                if (j - 2 >= 0 && i - 2 >= 0)
                                {
                                    if (board[j - 2, i - 2] == null && isWhite == board[j, i].isWhite)
                                    {
                                        pieceList.Add(board[j, i]);
                                    }
                                }
                            }
                        }
                    }
                    else if (board[j, i] != null && board[j, i].isQueen)
                    {
                        QueenScanningHelper(board[j, i],isWhite);
                    }
                }
            }
            return pieceList;
        }
        public override List<Piece> ScanForOne(Piece p, bool isWhiteTurn)
        {
            pieceList.Clear();
            if (!p.isQueen)
            {
                //if top right is not out of bound
                if (p.x + 1 <= 7 && p.y + 1 <= 7)
                {
                    //if top right is not null AND top right from investigated has different colour from investigated 
                    if (board[p.x + 1, p.y + 1] != null && board[p.x + 1, p.y + 1].isWhite != p.isWhite)
                    {
                        //checking the boundaries
                        if (p.x + 2 <= 7 && p.y + 2 <= 7)
                        {
                            if (board[p.x + 2, p.y + 2] == null)
                            {
                                pieceList.Add(p);
                            }
                        }
                    }
                }
                //if top left is not out of bound
                if (p.x - 1 >= 0 && p.y + 1 <= 7)
                {
                    //if top left is not null AND top left from investigated has different colour from investigated 
                    if (board[p.x - 1, p.y + 1] != null && board[p.x - 1, p.y + 1].isWhite != p.isWhite)
                    {
                        if (p.x - 2 >= 0 && p.y + 2 <= 7)
                        {
                            if (board[p.x - 2, p.y + 2] == null)
                            {
                                pieceList.Add(p);
                            }
                        }
                    }
                }
                //}
                //else if (!isWhiteTurn && !p.isWhite) {
                //if bottom right is not out of bound
                if (p.x + 1 <= 7 && p.y - 1 >= 0)
                {
                    //if bottom right is not null AND bottom right from investigated has different colour from investigated 
                    if (board[p.x + 1, p.y - 1] != null && board[p.x + 1, p.y - 1].isWhite != p.isWhite)
                    {
                        if (p.x + 2 <= 7 && p.y - 2 >= 0)
                        {
                            if (board[p.x + 2, p.y - 2] == null)
                            {
                                pieceList.Add(p);
                            }
                        }
                    }
                }
                //if bottom left is not out of bound
                if (p.x - 1 >= 0 && p.y - 1 >= 0)
                {
                    //if bottom left is not null AND bottom left from investigated has different colour from investigated 
                    if (board[p.x - 1, p.y - 1] != null && board[p.x - 1, p.y - 1].isWhite != p.isWhite)
                    {
                        if (p.x - 2 >= 0 && p.y - 2 >= 0)
                        {
                            if (board[p.x - 2, p.y - 2] == null)
                            {
                                pieceList.Add(p);
                            }
                        }
                    }
                }
            }
            else if (p.isQueen)
            {
                QueenScanningHelper(p, isWhiteTurn);
            }
            return pieceList;
        }
        public override void GenerateBoard(GameObject whitePiecePrefab, GameObject blackPiecePrefab)
        {
            //generate White team.
            for (int y = 0; y < 3; y++)
            {
                bool oddRow = (y % 2 == 0);
                for (int x = 0; x < 8; x += 2)
                {
                    //generate piece
                    //ternery operator
                    //GeneratePiece((oddRow) ? x : x + 1, y);
                    if (oddRow)
                    {
                        GeneratePiece(whitePiecePrefab, true , x, y);
                    }
                    else
                        GeneratePiece(whitePiecePrefab, true, x + 1, y);
                }
            }

            //generate Black team.
            for (int y = 7; y > 4; y--)
            {
                bool oddRow = (y % 2 == 0);
                for (int x = 0; x < 8; x += 2)
                {
                    //generate piece
                    //ternery operator
                    if (oddRow)
                        GeneratePiece(blackPiecePrefab, false, x, y);
                    else
                        GeneratePiece(blackPiecePrefab, false, x + 1, y);
                }
            }
        }
        public void GeneratePiece(GameObject piecePrefab, bool isWhite, int x, int y)
        {
            GameObject go = Instantiate(piecePrefab) as GameObject;
            go.transform.SetParent(transform);
            //i dont know if its good practice.
            Piece p = go.GetComponent<Piece>();
            p.x = x;
            p.y = y;
            p.isWhite = isWhite;
            p.isQueen = false;
            board[x, y] = p;
            gameObject.GetComponent<Board>().MovePiece(p, p.x, p.y);
        }
        private void QueenScanningHelper(Piece p, bool isWhiteTurn)
        {
            if (p.isWhite == isWhiteTurn)
            {
                //right top
                int y = p.y;
                int j = p.x;
                for (int x = j; x < 7 && y < 7; x++, y++)
                {
                    //if piece come across, piece which is different colour
                    if (board[x, y] != null && p.isWhite != board[x, y].isWhite)
                    {
                        //check if behind this piece is empty place.
                        if (board[x + 1, y + 1] == null && isWhiteTurn == p.isWhite)
                        {
                            pieceList.Add(p);
                        }
                        else
                        {
                            //To prevent situation, that queen is able to kill the piece behind the piece.
                            //explained: https://github.com/AleksandrukTad/CheckersGame/issues/5
                            return;
                        }
                    }
                }
                y = p.y;
                j = p.x;
                //left top
                for (int x = j; x > 0 && y < 7; x--, y++)
                {
                    //if piece come across, piece which is different colour
                    if (board[x, y] != null && p.isWhite != board[x, y].isWhite)
                    {
                        //check if behind this piece is empty place.
                        if (board[x - 1, y + 1] == null)
                        {
                            pieceList.Add(p);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                y = p.y;
                j = p.x;
                //right bottom
                for (int x = j; x < 7 && y > 0; x++, y--)
                {
                    //if piece come across, piece which is different colour
                    if (board[x, y] != null && p.isWhite != board[x, y].isWhite)
                    {
                        //check if behind this piece is empty place.
                        if (board[x + 1, y - 1] == null)
                        {
                            pieceList.Add(p);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                y = p.y;
                j = p.x;
                //left bottom
                for (int x = j; x > 0 && y > 0; x--, y--)
                {
                    //if piece come across, piece which is different colour
                    if (board[x, y] != null && p.isWhite != board[x, y].isWhite)
                    {
                        //check if behind this piece is empty place.
                        if (board[x - 1, y - 1] == null)
                        {
                            pieceList.Add(p);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}
