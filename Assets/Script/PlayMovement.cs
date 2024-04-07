using UnityEngine;
//Code is not used in the project
public class PlayMovement : MonoBehaviour
{
    Rigidbody2D rb;

    Animator anim;
    public float playSpeed = 5f;

    public float moveX;

    private bool facingRight = true;// Facing right
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate() //Physical
    {
        Move();
        playerAnim();


    }
    private void Move()
    {
        if (facingRight == false&&moveX>0)
        {
            Flip();
        }
        else if (facingRight==true&&moveX<0)
        {
            Flip();
        }
    }
    private void Flip() //Flip
    {
        facingRight = !facingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }
    void playerAnim()
    {
        if (Mathf.Abs(moveX) > 0)
        {
            anim.SetBool("running", true);
        }
        else
        {
            anim.SetBool("running", false);
        }
    }
}
