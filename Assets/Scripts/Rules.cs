using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    abstract class Rules
    {
        //abstract public List<Piece> ScanForAll(Piece[,] board, bool isWhiteTurn);
        //abstract public List<Piece> ScanForOne(Piece[,] board, Piece p, bool isWhiteTurn);
        abstract public bool CheckIfValidMove(Piece[,] board, Piece p, int destX, int destY, bool multipleMove, out Piece killedP);
    }
}
