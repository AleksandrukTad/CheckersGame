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
        }
        public override void ScanForAll() { }
        public override void ScanForOne() { }
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
                        board[x,y] = GeneratePiece(whitePiecePrefab, true , x, y);
                    }
                    else
                        board[x, y] =  GeneratePiece(whitePiecePrefab, true, x + 1, y);
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
                        //board[x, y] = new Piece { x = x, y = y, isWhite = true };
                        board[x + 1, y] =  GeneratePiece(blackPiecePrefab, false, x, y);
                    else
                        board[x + 1 , y] =  GeneratePiece(blackPiecePrefab, false, x + 1, y);
                }
            }
        }
        public Piece GeneratePiece(GameObject piecePrefab, bool isWhite, int x, int y)
        {
            GameObject go = Instantiate(piecePrefab) as GameObject;
            go.transform.SetParent(transform);
            //i dont know if its good practice.
            Piece p = go.GetComponent<Piece>();
            p.x = x; p.y = y; p.isWhite = isWhite; p.isQueen = false;
            gameObject.GetComponent<Board>().movePiece(p, p.x, p.y);
            return p;
        }
    }
}
