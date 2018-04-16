using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class InternationalRules : Rules
    {
        
        public override bool CheckIfValidMove(Piece[,] board, Piece p, int destX, int destY, bool multipleMove, out Piece killedP)
        {
            killedP = null;
            //If we try to place piece, on top of another piece.
            if (board[destX, destY] != null)
            {
                return false;
            }

            //Checks if we move piece diagonally
            int deltaX = Mathf.Abs(destX - p.x);
            int deltaY = destY - p.y;

            if (multipleMove == false)
            {
                if (p.isWhite && !p.isQueen)
                    return CheckWhitePiece(board, p, destX, destY, out killedP, deltaX, deltaY);
                else if (!p.isWhite && !p.isQueen)
                    return CheckBlackPiece(board, p, destX, destY, out killedP, deltaX, deltaY);
                if (p.isQueen)
                    return CheckQueen(board, p, destX, destY, out killedP, multipleMove);

            }
            else if (multipleMove == true)
            {
                if (!p.isQueen)
                    return CheckMultipleMovePiece(board, p, destX, destY, out killedP, deltaX, deltaY);
                else
                    return CheckQueen(board, p, destX, destY, out killedP, multipleMove);
            }
            return false;
        }
        private bool CheckWhitePiece(Piece[,] board, Piece p, int destX, int destY, out Piece killedP, int deltaX, int deltaY)
        {
            killedP = null;
            if (deltaX == 1)
            {
                if (deltaY == 1)
                {
                    return true;
                }
            }
            else if (deltaX == 2)
            {   //possible jump and kill
                if (deltaY == 2)
                {   //count middle point between start and end Drag
                    int midX = (p.x + destX) / 2;
                    int midY = (p.y + destY) / 2;
                    //if this point is not null and its not the same colour as selected piece
                    if (board[midX, midY] != null && board[midX, midY].isWhite != p.isWhite)
                    {
                        //kill
                        killedP = board[midX, midY];
                        return true;
                    }
                }
                if (deltaY == -2)
                {   //count middle point between start and end Drag
                    int midX = (p.x + destX) / 2;
                    int midY = (p.y + destY) / 2;
                    //if this point is not null and its not the same colour as selected piece
                    if (board[midX, midY] != null && board[midX, midY].isWhite != p.isWhite)
                    {
                        //kill
                        killedP = board[midX, midY];
                        return true;
                    }
                }
            }
            return false;
        }
        private bool CheckBlackPiece(Piece[,] board, Piece p, int destX, int destY, out Piece killedP, int deltaX, int deltaY)
        {
            killedP = null;
            if (deltaX == 1)
            {
                if (deltaY == -1)
                {
                    return true;
                }
            }
            else if (deltaX == 2)
            {
                if (deltaY == 2)
                {   //count middle point between start and end Drag
                    int midX = (p.x + destX) / 2;
                    int midY = (p.y + destY) / 2;
                    //if this point is not null and its not the same colour as selected piece
                    if (board[midX, midY] != null && board[midX, midY].isWhite != p.isWhite)
                    {
                        //kill
                        killedP = board[midX, midY];
                        return true;
                    }
                }
                //possible jump and kill
                if (deltaY == -2)
                {   //count middle point between start and end Drag
                    int midX = (p.x + destX) / 2;
                    int midY = (p.y + destY) / 2;
                    //if this point is not null and its not the same colour as selected piece
                    if (board[midX, midY] != null && board[midX, midY].isWhite != p.isWhite)
                    {
                        //kill
                        killedP = board[midX, midY];
                        return true;
                    }
                }
            }
            return false;
        }
        private bool CheckQueen(Piece[,] board, Piece p, int destX, int destY, out Piece killedP, bool multipleMove)
        {
            killedP = null;
            int y = p.y;
            //diagonaly going up and right
            for (int x = p.x; x <= destX && y <= destY; x++, y++)
            {
                //if "road" from current location to destiny 
                //is "clear"
                if (board[x, y] == null && !multipleMove)
                {
                    if (x == destX && y == destY)
                    {
                        return true;
                    }
                }
                //if piece come across, piece which is different colour
                if (board[x, y] != null && board[x, y].isWhite != p.isWhite)
                {
                    //check if behind this piece is empty place.
                    if (board[x + 1, y + 1] == null)
                    {
                        //check if player set destination to the field after that piece
                        if (destX == x + 1 && destY == y + 1)
                        {
                            killedP = board[x, y];
                            return true;
                        }
                        //if not look if the destination picked by player is valid
                        int j = y + 1;
                        //look for the next, piece going top right.
                        for (int i = x + 1; i < 8 || j < 8; i++, j++)
                        {
                            if (board[i, j] != null)
                            {
                                return false;
                            }
                            //check if scanned possition in "landing" position.
                            if (i == destX && j == destY)
                            {
                                killedP = board[x, y];
                                return true;
                            }

                        }
                    }
                    else if (board[x + 1, y + 1] != null)
                    {
                        return false;
                    }
                }
            }
            y = p.y;
            //diagonaly going up and left
            for (int x = p.x; x >= destX && y <= destY; x--, y++)
            {
                //if "road" from current location to destiny 
                //is "clear"
                if (board[x, y] == null && !multipleMove)
                {
                    if (x == destX && y == destY)
                    {
                        return true;
                    }
                }
                //if piece come across, piece which is different colour
                if (board[x, y] != null && board[x, y].isWhite != p.isWhite)
                {
                    //check if behind this piece is empty place.
                    //and check if player set destination to that field.
                    if (board[x - 1, y + 1] == null)
                    {
                        if (destX == x - 1 && destY == y + 1)
                        {
                            killedP = board[x, y];
                            return true;
                        }
                        int j = y + 1;
                        //look for the next, piece going top left.
                        for (int i = x - 1; i >= 0 || j < 8; i--, j++)
                        {
                            if (board[i, j] != null)
                            {
                                return false;
                            }
                            //check if scanned possition in "landing" position.
                            if (i == destX && j == destY)
                            {
                                killedP = board[x, y];
                                return true;
                            }
                        }
                        //To prevent jumping over two pieces
                    }
                    else if (board[x - 1, y + 1] != null)
                    {
                        return false;
                    }
                }
            }
            y = p.y;
            //diagonaly going down and right
            for (int x = p.x; x <= destX && y >= destY; x++, y--)
            {
                //if "road" from current location to destiny 
                //is "clear"
                if (board[x, y] == null && !multipleMove)
                {
                    if (x == destX && y == destY)
                    {
                        return true;
                    }
                }
                //if piece come across, piece which is different colour
                if (board[x, y] != null && board[x, y].isWhite != p.isWhite)
                {
                    //check if behind this piece is empty place.
                    //and check if player set destination to that field.
                    if (board[x + 1, y - 1] == null)
                    {
                        if (destX == x + 1 && destY == y - 1)
                        {
                            killedP = board[x, y];
                            return true;
                        }
                        int j = y - 1;
                        //look for the next, piece going down right.
                        for (int i = x + 1; i < 8 || j >= 0; i++, j--)
                        {
                            if (board[i, j] != null)
                            {
                                return false;
                            }
                            //check if scanned possition in "landing" position.
                            if (i == destX && j == destY)
                            {
                                killedP = board[x, y];
                                return true;
                            }
                        }
                    }
                    else if (board[x + 1, y - 1] != null)
                    {
                        return false;
                    }
                }
            }
            y = p.y;
            //diagonaly going down and left
            for (int x = p.x; x >= destX && y >= destY; x--, y--)
            {
                //if "road" from current location to destiny 
                //is "clear"
                if (board[x, y] == null && !multipleMove)
                {
                    if (x == destX && y == destY)
                    {
                        return true;
                    }
                }
                //if piece come across, piece which is different colour
                if (board[x, y] != null && board[x, y].isWhite != p.isWhite)
                {
                    //check if behind this piece is empty place.
                    //and check if player set destination to that field.
                    if (board[x - 1, y - 1] == null)
                    {
                        if (destX == x - 1 && destY == y - 1)
                        {
                            killedP = board[x, y];
                            return true;
                        }
                        int j = y - 1;
                        //look for the next, piece going down left.
                        for (int i = x - 1; i >= 0 || j >= 0; i--, j--)
                        {
                            if (board[i, j] != null)
                            {
                                return false;
                            }
                            //check if scanned possition in "landing" position.
                            if (i == destX && j == destY)
                            {
                                killedP = board[x, y];
                                return true;
                            }
                        }
                    }
                    else if (board[x - 1, y - 1] != null)
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        private bool CheckMultipleMovePiece(Piece[,] board, Piece p, int destX, int destY, out Piece killedP, int deltaX, int deltaY)
        {
            killedP = null;
            if (deltaX == 2)
            {   //possible jump and kill
                if (deltaY == 2)
                {   //count middle point between start and end Drag
                    int midX = (p.x + destX) / 2;
                    int midY = (p.y + destY) / 2;
                    //if this point is not null and its not the same colour as selected piece
                    if (board[midX, midY] != null && board[midX, midY].isWhite != p.isWhite)
                    {
                        //kill
                        killedP = board[midX, midY];
                        return true;
                    }
                }
                if (deltaY == -2)
                {   //count middle point between start and end Drag
                    int midX = (p.x + destX) / 2;
                    int midY = (p.y + destY) / 2;
                    //if this point is not null and its not the same colour as selected piece
                    if (board[midX, midY] != null && board[midX, midY].isWhite != p.isWhite)
                    {
                        //kill
                        killedP = board[midX, midY];
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
