using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public List<Vector2> availableDirections {  get; private set; }

    private void Start()
    {
        availableDirections = new List<Vector2> ();

        checkAvailableDirection(Vector2.up);
        checkAvailableDirection(Vector2.down);
        checkAvailableDirection(Vector2.left);
        checkAvailableDirection(Vector2.right);
    }

    private void checkAvailableDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1.0f, obstacleLayer);

        if(hit.collider == null)
        {
            this.availableDirections.Add (direction);
        }
    }
}
