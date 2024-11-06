
using System;
using UnityEngine;

[RequireComponent (typeof(Movement))]
public class Pacman : MonoBehaviour
{
    public Movement movement {  get; private set; }

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)) {
            this.movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            this.movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            this.movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            this.movement.SetDirection(Vector2.right);
        }

        float angle = Mathf.Atan2(this.movement.direction.y, this.movement.direction.x);
        this.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }


    public void ResetState()
    {
        
        gameObject.SetActive(true);
        movement.ResetState();

    }
}
