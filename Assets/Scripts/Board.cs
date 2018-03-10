using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    abstract class Board : MonoBehaviour
    {
        public Piece[,] board;
        protected List<Piece> pieceList;
        public Vector3 boardOffset = new Vector3(-4.0f, 0, -4.0f);
        public Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

        public abstract List<Piece> ScanForAll(bool isWhiteTurn);
        public abstract List<Piece> ScanForOne(Piece p, bool isWhiteTurn);
        public abstract void GenerateBoard(GameObject whitePiecePrefab, GameObject blackPiecePrefab);
        public void MovePiece(Piece p, int x, int y)
        {
            p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
        }
    }
}
