using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class InternationalRules : Rules
    {
        private List<Piece> pieceList;
        public InternationalRules()
        {
            pieceList = new List<Piece>();
        }
        //Scanning whole board, to look for potential kills.
        public override List<Piece> ScanForAll(Piece[,] board, bool isWhiteTurn)
        {
            pieceList.Clear();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //check if scan will not go out of bounds
                    if (board[j, i] != null && !board[j, i].isQueen)
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
                                    if (board[j + 2, i + 2] == null && isWhiteTurn == board[j, i].isWhite)
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
                                    if (board[j - 2, i + 2] == null && isWhiteTurn == board[j, i].isWhite)
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
                                    if (board[j + 2, i - 2] == null && isWhiteTurn == board[j, i].isWhite)
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
                                    if (board[j - 2, i - 2] == null && isWhiteTurn == board[j, i].isWhite)
                                    {
                                        pieceList.Add(board[j, i]);
                                    }
                                }
                            }
                        }
                    }
                    else if (board[j, i] != null && board[j, i].isQueen)
                    {
                        QueenScanningHelper(board[j, i], board, pieceList, isWhiteTurn);
                    }
                }
            }
            return pieceList;
        }
        //Scanning only for one piece, to look for potential kills.
        public override List<Piece> ScanForOne(Piece[,] board, Piece p, bool isWhiteTurn)
        {
            pieceList.Clear();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //check if scan will not go out of bounds
                    if (board[j, i] != null && !board[j, i].isQueen)
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
                                    if (board[j + 2, i + 2] == null && isWhiteTurn == board[j, i].isWhite)
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
                                    if (board[j - 2, i + 2] == null && isWhiteTurn == board[j, i].isWhite)
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
                                    if (board[j + 2, i - 2] == null && isWhiteTurn == board[j, i].isWhite)
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
                                    if (board[j - 2, i - 2] == null && isWhiteTurn == board[j, i].isWhite)
                                    {
                                        pieceList.Add(board[j, i]);
                                    }
                                }
                            }
                        }
                    }
                    else if (board[j, i] != null && board[j, i].isQueen)
                    {
                        QueenScanningHelper(board[j, i], board, pieceList, isWhiteTurn);
                    }
                }
            }
            return pieceList;
        }
        public override bool checkIfValidMove(Piece[,] board, Piece p, int destX, int destY, bool multipleMove, out Piece killedP)
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
                    return checkWhitePiece(board, p, destX, destY, out killedP, deltaX, deltaY);
                else if (!p.isWhite && !p.isQueen)
                    return checkBlackPiece(board, p, destX, destY, out killedP, deltaX, deltaY);
                if (p.isQueen)
                    return checkQueen(board, p, destX, destY, out killedP, multipleMove);

            }
            else if (multipleMove == true)
            {
                if (!p.isQueen)
                    return checkMultipleMovePiece(board, p, destX, destY, out killedP, deltaX, deltaY);
                else
                    return checkQueen(board, p, destX, destY, out killedP, multipleMove);
            }
            return false;
        }
        private bool checkWhitePiece(Piece[,] board, Piece p, int destX, int destY, out Piece killedP, int deltaX, int deltaY)
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
        private bool checkBlackPiece(Piece[,] board, Piece p, int destX, int destY, out Piece killedP, int deltaX, int deltaY)
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
        private bool checkQueen(Piece[,] board, Piece p, int destX, int destY, out Piece killedP, bool multipleMove)
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
                            p = board[x, y];
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
                            if (i == destX && j == destY)
                            {
                                p = board[x, y];
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
                            p = board[x, y];
                            return true;
                        }
                        int j = y + 1;
                        //look for the next, piece going top right.
                        for (int i = x - 1; board[i, j] == null; i--, j++)
                        {
                            if (board[i, j] != null)
                            {
                                return false;
                            }
                            //check if scanned possition in "landing" position.
                            if (i == destX && j == destY)
                            {
                                p = board[x, y];
                                return true;
                            }
                        }
                        //To prevent jumping over two pieces
                    }
                    else if (board[x + 1, y + 1] != null)
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
                            p = board[x, y];
                            return true;
                        }
                        int j = y + 1;
                        //look for the next, piece going top right.
                        for (int i = x + 1; board[i, j] == null; i++, j--)
                        {
                            if (board[i, j] != null)
                            {
                                return false;
                            }
                            //check if scanned possition in "landing" position.
                            if (i == destX && j == destY)
                            {
                                p = board[x, y];
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
                            p = board[x, y];
                            return true;
                        }
                        int j = y - 1;
                        //look for the next, piece going down left.
                        for (int i = x - 1; board[i, j] == null; i--, j--)
                        {
                            if (board[i, j] != null)
                            {
                                return false;
                            }
                            //check if scanned possition in "landing" position.
                            if (i == destX && j == destY)
                            {
                                p = board[x, y];
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
            return false;
        }
        private bool checkMultipleMovePiece(Piece[,] board, Piece p, int destX, int destY, out Piece killedP, int deltaX, int deltaY)
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
        private void QueenScanningHelper(Piece p, Piece[,] board, List<Piece> forcedToMove, bool isWhiteTurn)
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
                            forcedToMove.Add(p);
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
                            forcedToMove.Add(p);
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
                            forcedToMove.Add(p);
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
                            forcedToMove.Add(p);
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
