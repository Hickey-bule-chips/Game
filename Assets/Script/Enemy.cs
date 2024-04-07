using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2.0f; //Movement speed
    public EnemyFlipSensor groundFlipSensor;  //Determining whether an enemy has reached the edge of the terrain
    public EnemyFlipSensor wallFlipSensor;  //Determining whether an enemy is touching a wall

    private Animator animator;
    private Rigidbody2D body2d;
    private GroundSensor groundSensor;

    AttackSensor attackSensor;  //Determine if there is an enemy in range of the attack
    private bool grounded = false;  //Whether the enemy character is on the ground
    private bool combatIdle = false;  //Idle state animation switch
    private bool isDead = false;  //Whether the enemy character is dead or not
    private bool isHurt = false;  //Whether the enemy character is being hit or not
    private float hurtTimer;  //Duration of the attack
    private float attackTimer;  //Attack timer

    // Start is called before the first frame update
    void Start()
    {
        //Initialisation
        groundFlipSensor = GetComponentInChildren<EnemyFlipSensor>();
        attackSensor = GetComponentInChildren<AttackSensor>();
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();
        groundFlipSensor.isGround = true;
        attackSensor.type = "Player";  //Set the attack target tag to Player


    }

    // Update is called once per frame
    void Update()
    {
        //No logic is processed if the command has not been started or if the attacker is dead
        if (GameManager.isStartGame == false) return;
        if (isDead) return;


        int direction = (int)transform.localScale.x;  //Determine the Enemy's current orientation based on localScale.x
        //Turn when the Enemy moves to the edge of the terrain or hits a wall
        if (groundFlipSensor.isGround == false || wallFlipSensor.isWall)
        {
            direction = -direction;
            transform.localScale = new Vector3(direction, 1, 1);
            groundFlipSensor.isGround = true;
            wallFlipSensor.isWall = false;
        }

        //Check if character just landed on the ground
        if (!grounded && groundSensor.State())
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }

        string animString;
        Transform player = GameObject.Find("Player").transform;
        //If the player character is less than 1, the movement is suspended in preparation for an attack
        if (Vector3.Distance(player.position, transform.position) < 1f)
        {
            //Determine the position of the player character so that the enemy is facing the player character
            if (player.position.x < transform.position.x){
                transform.localScale = new Vector3(1, 1, 1);
            }
            else{
                transform.localScale = new Vector3(-1, 1, 1);
            }
            //Set direction to 0 to pause movement
            direction = 0;
            attackTimer += Time.deltaTime;
            //Start timing, 0.4 seconds to start the attack animation
            if (attackTimer > 0.4f)
            {
                animString = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                //Determine which animation is currently running to prevent repeated triggering of Attack animations
                if (!animString.Contains("Attack"))
                {
                    animator.SetTrigger("Attack");
                }
                //Damage to player character starts after 0.74 seconds
                if (attackTimer > 0.74f)
                {
                    for (int i = 0; i < attackSensor.enemyList.Count; i++)
                    {
                        //Debug.Log(attackSensor.enemyList[i].gameObject.name);
                        //Attack enemies picked up by attackSensor
                        attackSensor.enemyList[i].GetComponent<Player>().Hurt(gameObject);
                    }
                    attackTimer = 0;
                }
            }
        }
        else
        {
            attackTimer = 0;
        }
        animString = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name; //Get the name of the animation currently playing
        // Move
        if (isHurt == false && !animString.Contains( "Attack"))  //Does not move when the character is being hit or is in an attack state
            body2d.velocity = new Vector2(-direction * speed, body2d.velocity.y);

        //Set AirSpeed in animator
        animator.SetFloat("AirSpeed", body2d.velocity.y);

        // -- Handle Animations --
        //Run
        if (Mathf.Abs(direction) > Mathf.Epsilon && isHurt == false)
            animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (combatIdle)
            animator.SetInteger("AnimState", 1);

        //Idle
        else
            animator.SetInteger("AnimState", 0);

        if (isHurt)
        {
            hurtTimer += Time.deltaTime;
            //After 0.5 seconds of being attacked, the attack state ends and the component is removed
            if (hurtTimer > 0.5f)
            {
                isHurt = false;
                isDead = true;
                hurtTimer = 0;
                animator.SetTrigger("Death");
                Destroy(GetComponent<Rigidbody2D>());
                Destroy(GetComponent<CapsuleCollider2D>());
                gameObject.tag = "Untagged";
            }
        }
    }
    //Invoked when the enemy character is attacked
    public void Hurt(GameObject player)
    {
        if (isDead == true) return;
        //Triggers the attack animation
        animator.SetTrigger("Hurt");
        //Applies the force of the attack
        if (player.transform.position.x > transform.position.x)
        {
            body2d.AddForce(new Vector2(-12000, 5000));
        }
        else
        {
            body2d.AddForce(new Vector2(12000, 5000));
        }
        hurtTimer = 0;
        isHurt = true;
    }


}
