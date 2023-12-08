using UnityEngine;

namespace Chess.Scripts.Core
{
    public class ChessPieceMovement : MonoBehaviour
    {
        public ChessSide CurrentSide;
        [SerializeField]
        private int CurrentRow, CurrentColumn;
        private int MaxRow = 8, MaxColumn = 8;

        public void SetRowCol(int row, int column)
        {
            CurrentColumn = column;
            CurrentRow = row;
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
        private void KingMovement()
        {
            for (int i = CurrentRow - 1; i <= CurrentRow + 1; i++)
            {
                for (int j = CurrentColumn - 1; j <= CurrentColumn + 1; j++)
                {

                    if (i == CurrentRow && j == CurrentColumn)
                        continue;
                    if (IsWithinBounds(i, j))
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
                            Debug.Log("Bishop Pos : row " + newRow + "  COl: " + newColumn + "  Break ");
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
                if (CheckPositionIsFree(i, CurrentColumn)) ChessBoardPlacementHandler.Instance.Highlight(i, CurrentColumn);
                else break;

            }
            for (int i = CurrentRow - 1; i >= 0; i++)
            {
                if (CheckPositionIsFree(i, CurrentColumn)) ChessBoardPlacementHandler.Instance.Highlight(i, CurrentColumn);
                else break;
            }

            for (int j = CurrentColumn + 1; j < MaxColumn; j++)
            {
                if (CheckPositionIsFree(CurrentRow, j)) ChessBoardPlacementHandler.Instance.Highlight(CurrentRow, j);
                else break;
            }
            for (int j = CurrentColumn - 1; j >= 0; j++)
            {
                if (CheckPositionIsFree(CurrentRow, j)) ChessBoardPlacementHandler.Instance.Highlight(CurrentRow, j);
                else break;
            }
        }
        private void KnightMovemetn()
        {
            int[] knightRowMoves = { -2, -1, 1, 2, 2, 1, -1, -2 };
            int[] knightColMoves = { 1, 2, 2, 1, -1, -2, -2, -1 };

            for (int k = 0; k < 8; k++)
            {
                int newRow = CurrentRow + knightRowMoves[k];
                int newCol = CurrentColumn + knightColMoves[k];

                if (IsWithinBounds(newRow, newCol) && CheckPositionIsFree(newRow, newCol))
                {
                    Debug.Log(newRow + " , " + newCol);
                    ChessBoardPlacementHandler.Instance.Highlight(newRow, newCol);
                }
            }
        }
        private void Pawn()
        {
            int direction = (CurrentSide == ChessSide.Black) ? 1 : -1;
            int forwardRow = CurrentRow + direction;
            if(IsWithinBounds(forwardRow, CurrentColumn + 1))CheckPositionIsFree(forwardRow, CurrentColumn + 1);
            if(IsWithinBounds(forwardRow, CurrentColumn - 1))CheckPositionIsFree(forwardRow, CurrentColumn - 1);
            if (IsWithinBounds(forwardRow, CurrentColumn) && checkPawnFreePos(forwardRow, CurrentColumn))
            {
                ChessBoardPlacementHandler.Instance.Highlight(forwardRow, CurrentColumn);
                if ((CurrentSide == ChessSide.Black && CurrentRow == 1) ||  (CurrentSide == ChessSide.White && CurrentRow == 6))
                {
                    int doubleForwardRow = forwardRow + direction;
                    if (IsWithinBounds(doubleForwardRow, CurrentColumn) && checkPawnFreePos(doubleForwardRow, CurrentColumn))
                    {
                        ChessBoardPlacementHandler.Instance.Highlight(doubleForwardRow, CurrentColumn);
                    }
                }
            }
        }
        private bool IsWithinBounds(int r, int c)
        {
            return r >= 0 && r < MaxRow && c >= 0 && c < MaxColumn;
        }
        private bool CheckPositionIsFree(int r, int c)
        {
            Vector2 position = ChessBoardPlacementHandler.Instance.GetTile(r, c).transform.position;
            Collider2D collider = Physics2D.OverlapPoint(position);
            if (collider != null)
            {
                ChessPieceMovement piece = collider.gameObject.GetComponent<ChessPieceMovement>();
                if (piece != null)
                {
                   CheckPositionIsEnemy(piece, r, c);
                    return false;
                }
            }
            return true;
        }
        private bool checkPawnFreePos(int r,int c)
        {
            Vector2 position = ChessBoardPlacementHandler.Instance.GetTile(r, c).transform.position;
            Collider2D collider = Physics2D.OverlapPoint(position);
            if (collider != null)
            {
                ChessPieceMovement piece = collider.gameObject.GetComponent<ChessPieceMovement>();
                if (piece != null)
                {
                    return false;
                }
            }
            return true;
        }

        private void CheckPositionIsEnemy(ChessPieceMovement piece, int r, int c)
        {
            if (piece.CurrentSide != this.CurrentSide)
            {
                ChessBoardPlacementHandler.Instance.HighlightEnemy(r, c);
                Debug.Log("Row : " + r + "  Col : " + c + "  IsEnemy : " + piece.name +"  "+ piece.CurrentSide.ToString());
            }

        }

        public enum ChessSide
        {
            White,
            Black
        }
    }
}
