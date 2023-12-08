
using Chess.Scripts.Core;
using UnityEngine;


namespace Chess.Scripts.MouseInput
{
    public class InputClickDetection : MonoBehaviour
    {
        void Update()
        {
            CheckMouseClick();
        }
        private void CheckMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("clicked!");
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Ray ray = new Ray(mousePosition, Vector3.forward);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit.collider != null)
                {
                    Debug.Log("Sprite clicked!");
                    ChessPieceMovement ChessPiece = hit.collider.gameObject.GetComponent<ChessPieceMovement>();

                    if (ChessPiece != null)
                    {
                        Debug.Log("ChessPiece clicked!" + ChessPiece);
                        ChessPiece.GetPossibleMovement();
                    }
                }
            }
        }
    }
}
