using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_animator : MonoBehaviour
{
    public Animator animator;
    
    
    private Rigidbody2D body;
    private float x, y;
    private bool isMoving;
    [SerializeField] private float spd;
    private Vector3 moveDir;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        body.velocity = moveDir * spd;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if(x == 0 && y == 0) {
            isMoving = false;
            animator.SetBool("isMoving", isMoving);
            body.velocity = Vector3.zero;
            moveDir = new Vector3(x,y).normalized;
            return;
        }

        animator.SetFloat("x", x);
        animator.SetFloat("y", y);
        if(!isMoving) {
            isMoving = true;
            animator.SetBool("isMoving", isMoving);
        }

        moveDir = new Vector3(x,y).normalized;
    }
}
