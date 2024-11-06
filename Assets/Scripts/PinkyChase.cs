using UnityEngine;

public class PinkyChase : GhostChase
{
    private void OnDisable()
    {
        // Switch back to scatter mode when disabling chase mode
        ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // Do nothing while the ghost is frightened
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            // Get Pac-Man's current direction
            Vector2 pacManDirection = ghost.target.GetComponent<Movement>().nextDirection;

            // Calculate Pinky's target position, four tiles ahead of Pac-Man's current direction
            Vector2 targetPosition = CalculatePinkyTargetPosition(pacManDirection);

            // Find the available direction that moves closest to Pinky's target position
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (targetPosition - (Vector2)newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            // Set Pinky's direction towards the calculated target
            ghost.movement.SetDirection(direction);
        }
    }

    // Calculate Pinky's target position based on Pac-Man's direction
    private Vector2 CalculatePinkyTargetPosition(Vector2 pacManDirection)
    {
        // Get Pac-Man's current position
        Vector2 pacManPosition = ghost.target.position;

        // Pinky's target is four tiles ahead of Pac-Man in the direction he's moving
        Vector2 targetPosition = pacManPosition + (pacManDirection * 4);

        // Special case for when Pac-Man is moving up (bug in the original game)
        if (pacManDirection == Vector2.up)
        {
            // Target four tiles up and two tiles left of Pac-Man's position
            targetPosition += new Vector2(-2, 0);
        }

        return targetPosition;
    }
}
