using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    public Movement movement {  get; private set; }

    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }

    public GhostBehavior initialBehavior;

    public Transform target;


    public int points = 200;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState();
    }


    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if(home != initialBehavior)
        {
            home.Disable();
        }

        if(initialBehavior != null)
        {
            initialBehavior.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            } else
            {
                FindObjectOfType<GameManager>().PacmanEaten();
            }
        }
    }
}
