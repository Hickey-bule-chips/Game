using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float maxHP = 10;  //Initial HP value
    public float speed = 4.0f;  //Movement speed
    [HideInInspector]
    public float hp;  //Actual HP value
    [SerializeField]
    float jumpForce = 10.0f;  //Jump Force

    private Animator animator;
    private Rigidbody2D body2d;
    private GroundSensor groundSensor;

    public float inputX;  //Move direction control. 1 forward, 0 no movement, -1 backward
    private bool grounded = false;  //Whether on the ground or not
    private bool combatIdle = false;  //Whether to switch the idle state animation to a combatIdle animation
    //private bool isDead = false;  //Dead or not
    private bool isHurt = false;  //Is it under attack
    private float hurtTimer = 0;  //Impact Timer

    // Use this for initialization
    void Start () {
        //Initialization
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();

        hp = maxHP;
    }
	
	// Update is called once per frame
	void Update () {
        //Check if character just landed on the ground
        if (!grounded && groundSensor.State()) {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }

        //Check if character just started falling
        if(grounded && !groundSensor.State()) {
           grounded = false;
           animator.SetBool("Grounded", false);
        }

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        //Suspension of movement when struck
        if (isHurt == false)
            body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);

        //Set AirSpeed in animator
        animator.SetFloat("AirSpeed", body2d.velocity.y);

        // -- Handle Animations --
        //Run
        if (Mathf.Abs(inputX) > Mathf.Epsilon)
            animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (combatIdle)
            animator.SetInteger("AnimState", 1);

        //Idle
        else
            animator.SetInteger("AnimState", 0);

        //After 0.5 seconds after being attacked, the attacked state ends
        if (isHurt)
        {
            hurtTimer += Time.deltaTime;
            if(hurtTimer > 0.5f)
            {
                isHurt = false;
            }
        }
    }

    //Jump
    public void Jump()
    {
        if (grounded)
        {
            animator.SetTrigger("Jump");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }
    }

    public bool GetGrounded()
    {
        return grounded;
    }

    //Switching the animation of the Idle state to CombatIdle animation
    public void CombatIdle()
    {
        combatIdle = true;
    }

    //Attack
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    //Invoked when the player character is hit
    public void Hurt(GameObject player)
    {
        //Trigger hit animation
        animator.SetTrigger("Hurt");
        //Applying the impacted force
        if (player.transform.position.x > transform.position.x)
        {
            body2d.AddForce(new Vector2(-400, 100));
        }
        else
        {
            body2d.AddForce(new Vector2(400, 100));
        }
        hurtTimer = 0;
        isHurt = true;
        //Blood deduction and determination of death
        hp -= 10;
        if(hp <= 10)
        {
            animator.SetTrigger("Death");
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Prop")
        {
            hp += 10;
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Die")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
            animator.SetTrigger("Death");
        }
    }
}
