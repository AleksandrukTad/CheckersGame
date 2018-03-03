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
        private Vector3 boardOffset = new Vector3(-4.0f, 0, -4.0f);
        private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

        public abstract void ScanForAll();
        public abstract void ScanForOne();
        public abstract void GenerateBoard(GameObject whitePiecePrefab, GameObject blackPiecePrefab);
        public void movePiece(Piece p, int x, int y)
        {
            p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
        }
    }
}
