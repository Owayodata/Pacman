using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;

    public bool eaten { get; private set; }
    private float duration; // Store duration for later use

    // Override enable function
    public override void Enable(float duration)
    {
        base.Enable(duration);
        this.duration = duration;

        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        Invoke(nameof(Flash), duration / 2.0f);
    }

    // Override disable function
    public override void Disable()
    {
        base.Disable();
        body.enabled = true;   // Restore the body when disabling frightened mode
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    // Flash from blue to white
    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;
            white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    // If ghost is eaten, reset to home position and change sprites
    private void Eaten()
    {
        eaten = true;

        // Move ghost back to home position
        Vector3 homePosition = ghost.home.inside.position;
        homePosition.z = ghost.transform.position.z; // Maintain original z-axis

        ghost.transform.position = homePosition; // Fix: Move ghost to home position

        // Enable ghost's home behavior to return it
        ghost.home.Enable(duration);

        // Show the eyes and hide everything else when eaten
        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    // Slow down the ghost when frightened
    private void OnEnable()
    {
        ghost.movement.speedMultiplier = 0.5f;
        eaten = false;
    }

    // Restore the ghost's speed when not frightened
    private void OnDisable()
    {
        ghost.movement.speedMultiplier = 1f;
        eaten = false;
    }

    // Handle collision with Pacman
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (enabled)
            {
                Eaten();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // Do nothing while the ghost is frightened
        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            // Find the available direction that moves closet to pacman
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // If the distance in this direction is less than the current
                // min distance then this direction becomes the new closest
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }
}
