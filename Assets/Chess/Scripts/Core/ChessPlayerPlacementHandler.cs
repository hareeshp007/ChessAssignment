using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.XR;

namespace Chess.Scripts.Core {
    public class ChessPlayerPlacementHandler : MonoBehaviour {
        [SerializeField] public int row, column;
        public ChessSide CurrentSide;
        [SerializeField]
        private int CurrentRow,CurrentColumn;
        private int MaxRow=8, MaxColumn=8;

        private void Start() {
            CurrentRow = row;
            CurrentColumn = column;
            transform.position = ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position;
        }

        public void CheckButtonClick()
        {
            Debug.Log("Button Pressed");
        }

        public void GetPossibleMovement()
        {
            ChessBoardPlacementHandler.Instance.ClearHighlights();
            string ChessPieceTag = this.gameObject.tag;
            switch (ChessPieceTag)
            {
                case "King":
                    KingMovement();
                    break;
                case "Queen":
                    QueenMovement();
                    break;
                case "Bishop":
                    BishopMovement();
                    break;
                case "Rook":
                    RookMovement();
                    break;
                case "Knight":
                    KnightMovemetn();
                    break;
                case "Pawn":
                    Pawn();
                    break;
            }
        }
        /// <summary>
        //King - Moves one square in any direction.
        //Queen - Moves any number of squares diagonally, horizontally, or vertically.
        //Rook - Moves any number of squares horizontally or vertically.
        //Bishop - Moves any number of squares diagonally.
        //Knight - Moves in an ‘L-shape,’ two squares in a straight direction, and then one square perpendicular to that.
        //Pawn - Moves one square forward, but on its first move, it can move two squares forward. It captures diagonally one square forward.
        /// </summary>
        private void KingMovement()
        {
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = column - 1; j <= column + 1; j++)
                {
                   
                    if (i == row && j == column)
                        continue;
                    if(IsWithinBounds(i, j))
                    {
                        if (CheckPositionIsFree(i, j))
                        {
                            ChessBoardPlacementHandler.Instance.Highlight(i, j);
                        }
                    }
                }
            }
        }
        private void QueenMovement()
        {
            RookMovement();
            BishopMovement();
        }
        private void BishopMovement()
        {
            for (int i = -1; i <= 1; i += 2)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    int newRow = CurrentRow + i;
                    int newColumn = CurrentColumn + j;
                    
                    while (IsWithinBounds(newRow, newColumn))
                    {
                        bool isFree = CheckPositionIsFree(newRow, newColumn);
                        if (isFree) 
                        {
                            Debug.Log("Bishop Pos : row " + newRow + "  COl: " + newColumn);
                            ChessBoardPlacementHandler.Instance.Highlight(newRow, newColumn);
                            
                        }
                        else
                        {
                            Debug.Log("Bishop Pos : row " + newRow + "  COl: " + newColumn  +"  Break ");
                            break;
                        }
                        newRow += i;
                        newColumn += j;
                    }
                }
            }
        }
        private void RookMovement()
        {

            for (int i = CurrentRow + 1; i < MaxRow; i++)
            {
                if (CheckPositionIsFree(i, column))  ChessBoardPlacementHandler.Instance.Highlight(i, column);
                else break;
                    
            }
            for (int i = CurrentRow - 1; i >= 0; i++)
            {
                if (CheckPositionIsFree(i, column)) ChessBoardPlacementHandler.Instance.Highlight(i, column);
                else break;
            }

            for (int j = CurrentColumn + 1; j < MaxColumn; j++)
            {
                if (CheckPositionIsFree(row, j))  ChessBoardPlacementHandler.Instance.Highlight(row, j);
                else break;
            }
            for (int j = CurrentColumn - 1; j >= 0 ; j++)
            {
                if (CheckPositionIsFree(row, j))  ChessBoardPlacementHandler.Instance.Highlight(row, j);
                else break;
            }
        }
        private void KnightMovemetn()
        {
            int[] knightRowMoves = { -2, -1, 1, 2, 2, 1, -1, -2 };
            int[] knightColMoves = { 1, 2, 2, 1, -1, -2, -2, -1 };

            for (int k = 0; k < 8; k++)
            {
                int newRow = row + knightRowMoves[k];
                int newCol = column + knightColMoves[k];

                if (IsWithinBounds(newRow, newCol) && CheckPositionIsFree(newRow,newCol))
                {
                    Debug.Log(newRow + " , "+ newCol);
                    ChessBoardPlacementHandler.Instance.Highlight(newRow, newCol);
                }
            }
        }
        private void Pawn()
        {
            int forwardRow = row + 1;
            if (IsWithinBounds(forwardRow, column) && CheckPositionIsFree(forwardRow, column))
            {
                ChessBoardPlacementHandler.Instance.Highlight(forwardRow, column);
                if (row == 1 && CheckPositionIsFree(forwardRow + 1, column))
                {
                    ChessBoardPlacementHandler.Instance.Highlight(forwardRow + 1, column);
                }
            }
        }
        private bool IsWithinBounds(int r, int c)
        {
            return r >= 0 && r < MaxRow && c >= 0 && c < MaxColumn;
        }
        private bool CheckPositionIsFree(int r,int c)
        {
            Vector2 position = ChessBoardPlacementHandler.Instance.GetTile(r, c).transform.position;
            Collider2D collider = Physics2D.OverlapPoint(position);
            Debug.Log(collider);
            if(collider != null)
            {
                ChessPlayerPlacementHandler piece = collider.gameObject.GetComponent<ChessPlayerPlacementHandler>();
                if (piece != null)
                {
                    CheckPositionIsEnemy(piece,r,c);
                    return false;
                }
            }
            return true;
        }

        private void CheckPositionIsEnemy(ChessPlayerPlacementHandler piece,int r,int c)
        {
           if( piece.CurrentSide!=this.CurrentSide)
            {
                ChessBoardPlacementHandler.Instance.HighlightEnemy(r,c);
                Debug.Log("Row : " + r + "  Col : " + c + "IsEnemy :" + piece.name +piece.CurrentSide.ToString());
            }
              
        }
    }
    public enum ChessSide
    {
        White,
        Black
    }
}
   