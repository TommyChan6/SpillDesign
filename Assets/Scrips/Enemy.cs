using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public float healthPoints = 2;
    Animator animator;

    public float moveSpeed = 1f;
    public GameObject drop1;
    public GameObject drop2;
    public GameObject drop3;
    private List<GameObject> dropList = new List<GameObject>();
    public float dropRate;
    public float collisionOffset = 0.02f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    GameObject player;
    private bool isDying = false;
    public Collider2D hitBox;
    private SpriteRenderer spriteRenderer;
    
    
    

    // Start is called before the first frame update
    void Start()
    {
        //print("dsfds");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
        if (drop1 != null) {
            dropList.Add(drop1);
        }
        if (drop2 != null) {
            dropList.Add(drop2);
        }
        if (drop3 != null) {
            dropList.Add(drop3);
        }
    }

    public float HealthPoints {
        set {
            healthPoints = value;
            if(healthPoints <= 0) {
                Defeated();
            }
        }
        get {
            return healthPoints;
        }
    }

    private Vector2 currentMoveDirection;
    public bool isDashable = false;
    private bool isDashing = false;
    private bool isStartDashing = false;
    private float dashTimer = 0f;
    public float dashSpeed = 1.5f;
    public float dashTiggerRadius = 2f;
    // Update is called once per frame
    private void Update() {
        // Find player and move towards the player
        if (isDying != false) {
            animator.SetBool("isMoving", false);
        } else {
            // set a public variable isChargingAttack, if true, stops the movement, set animator isMoving to false                         < -----------------------------
            movementInput = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            movementInput.Normalize();
            if (movementInput != Vector2.zero){
                // Dash with a random chance
                if (isDashable) {
                    if (
                        Math.Sqrt((player.transform.position.x - transform.position.x)*(player.transform.position.x - transform.position.x)
                        +((player.transform.position.y - transform.position.y)*(player.transform.position.y - transform.position.y))) < dashTiggerRadius
                    ) {
                        // randomValue = Random.Range(0f, 1f);
                        // if (randomValue < 0.01)
                        if (!isDashing) {
                            StartChargingToDash();
                            moveSpeed = 0f;
                        }
                        
                    }
                }
                
                if(isDashing) {
                    dashTimer += Time.deltaTime;
                    if(isStartDashing) {
                        print("start actual");
                        currentMoveDirection = movementInput;
                        isStartDashing = false;
                        moveSpeed = 4f;
                    } else {
                        // currentMoveDirection remains the same
                        if(dashTimer >= 1f) {
                            // End of dash
                            isDashing = false;
                            moveSpeed = 0.6f;
                            dashTimer = 0f;
                        } else {
                            //rb.MovePosition(rb.position + direction * dashSpeed * Time.fixedDeltaTime);
                        }
                    }
                } else {
                    currentMoveDirection = movementInput;
                }
                bool success = TryMove(currentMoveDirection);
                if(!success) {
                    success = TryMove(new Vector2(currentMoveDirection.x, 0));
                    if(!success) {
                        success = TryMove(new Vector2(0, currentMoveDirection.y));
                    }
                }
                animator.SetBool("isMoving", success);   // Need to add more moving directions
                if (currentMoveDirection.x < 0f) {
                    spriteRenderer.flipX = true;
                } else {
                    spriteRenderer.flipX = false;
                }
            } else {
                animator.SetBool("isMoving", false);
            }
        }
    }

    private bool TryMove(Vector2 direction) {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset
        );
        if(count == 0){
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            
            return true;
        } else {
            Attack();                      // Attack() in FixedUpdate() is not good      <---------------------------
            return false;
        };
    }

    void OnMove(Vector2 movementValue) {
        movementInput = movementValue;
    }

    public void TakeDamage(float damageValue) {
        HealthPoints -= damageValue;
        // Here we can calculate other stats like defense and shield etc
        
    }

    public void Defeated() {
        animator.SetTrigger("Defeated");
        isDying = true;
        if (hitBox != null) {
            hitBox.enabled = false;
        }
    }

    public void RemoveEnemy() {
        print("remove enemy");
        if (dropRate > UnityEngine.Random.Range(0f, 1f)) {
            if (dropList.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, dropList.Count); // Get a random index
                GameObject selectedObject = dropList[randomIndex]; // Select the GameObject
                Instantiate(selectedObject, transform.position, Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }

    public GameObject attack;
    //private bool notAttacked = true;
    private float timer = 0f;
    private float attackCooldown = 1f;
    // Generate attack collider for 1 update and cooldown 0.5 sec
    public void Attack() {

        timer += Time.deltaTime;
        if (timer >= attackCooldown) {
            Instantiate(attack, transform.position, Quaternion.identity);
            timer = 0f;
            // if(notAttacked) {
            //         attack.GetComponent<Collider2D>().enabled = true;
            //         print("attack");
            //         notAttacked = false;
            // }
            // attack.GetComponent<Collider2D>().enabled = false;
        }
    }


    
    // public void DashForward(Vector2 direction) {
    //     dashTimer += Time.deltaTime;
    //     if(dashTimer >= 1f) {
    //         // End dash
    //         isDashing = false;
    //     } else {
    //         rb.MovePosition(rb.position + direction * dashSpeed * Time.fixedDeltaTime);
    //     }
    // }
    public void StartChargingToDash() {
        animator.SetBool("isCharging", true);
    }

    public void EndChargingToDash() {
        animator.SetBool("isCharging", false);
    }

    public void StartDashing() {
        if(isDashable) {
            //print("start dashing");
            if (!isDashing) {
                print("start dashing");
                isDashing = true;
                isStartDashing = true;
            }
        } else {
            print("Not able to dash, change the isDashable variable to make this enemy dashable");
            isDashing = false;
            isStartDashing = false;
        }
    }
}
