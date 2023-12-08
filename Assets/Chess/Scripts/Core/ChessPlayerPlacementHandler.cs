using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.XR;

namespace Chess.Scripts.Core {
    public class ChessPlayerPlacementHandler : MonoBehaviour {
        [SerializeField] public int row, column;
        public ChessPieceMovement pieceMovement;
        private void Awake()
        {
            pieceMovement = GetComponent<ChessPieceMovement>();
        }
        private void Start() {
            transform.position = ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position;
            pieceMovement.SetRowCol(row,column);
        }

        public void CheckButtonClick()
        {
            Debug.Log("Button Pressed");
        }

        
        /// <summary>
        //King - Moves one square in any direction.
        //Queen - Moves any number of squares diagonally, horizontally, or vertically.
        //Rook - Moves any number of squares horizontally or vertically.
        //Bishop - Moves any number of squares diagonally.
        //Knight - Moves in an ‘L-shape,’ two squares in a straight direction, and then one square perpendicular to that.
        //Pawn - Moves one square forward, but on its first move, it can move two squares forward. It captures diagonally one square forward.
        /// </summary>

    }
    
}
   