using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class PlayerController : MonoBehaviour
{

    public float healthPoints = 5f;
    public float moveSpeed = 0.8f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public Collider2D hitBox;                       // For getting hit
    public WeaponController weaponController;       // For changing weapon
    public StaminaController stamina;               // For stamina control

    private EventInstance playerFootsteps;
    private Vector2 previousPosition;

    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();


    // Scores
    public int numberOfSlimesKilled = 0;
    public int slimeScore = 2;
    public int numberOfRedMonsterKilled = 0;
    public int RedMonsterScore = 10;
    public int numberOfSprinterKilled = 0;
    public int sprinterScore = 5;

    public LevelFinishScreen levelFinishedPanel;

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

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;
    public GameObject heart5;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerFootsteps = AudioManager.instance.CreateInstance(FMODEvents.instance.playerFootsteps);
        previousPosition = rb.position;
    }

    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            print("right clicked");
            Dash();
        }
    }

    void FixedUpdate() {
        //UpdateSound();
        // print(rb.position);
        // print(previousPosition);
        
        if(movementInput != Vector2.zero){
            bool success = TryMove(movementInput);
            // If the movement is block, try moving sideways
            if(!success) {
                success = TryMove(new Vector2(movementInput.x, 0));
                if(!success) {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }
            animator.SetBool("isMovingDown", success);   // Need to add more moving directions animations
        } else {
            animator.SetBool("isMovingDown", false);
        }
        UpdateSound();
        previousPosition = rb.position;
    }

    // Check if there is an object in the moving direction and if not move toward that direction
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
            return false;
        };
    }

    // Get movement value from the player
    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }

    // Runs once when left mouse button pressed to fire bullet using weapon controller
    void OnFire() {
        weaponController.GetComponent<WeaponController>().SingleFire();
    }

    // void OnRightClick() {
    //     print("right clicked");
    // }

    public void Defeated(){
        print("Died");
        if (levelFinishedPanel != null) {
            levelFinishedPanel.FinishLevel("died");
        }
        //SceneManager.LoadSceneAsync(0);
    }

    public void TakeDamage(float damageValue) {
        HealthPoints -= damageValue;
        print(HealthPoints);
        print("damageTaken");
        // Here we can calculate other stats like defense and shield ect
        ResetHealthUI();
    }

    public void ResetHealthUI() {
        if (healthPoints == 1f) {
            heart1.SetActive(true);
            heart2.SetActive(false);
            heart3.SetActive(false);
            heart4.SetActive(false);
            heart5.SetActive(false);
        } else if (healthPoints == 2f) {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(false);
            heart4.SetActive(false);
            heart5.SetActive(false);
        } else if (healthPoints == 3f) {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
            heart4.SetActive(false);
            heart5.SetActive(false);
        } else if (healthPoints == 4f) {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
            heart4.SetActive(true);
            heart5.SetActive(false);
        } else if (healthPoints == 5f) {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
            heart4.SetActive(true);
            heart5.SetActive(true);
        }
    }
    
    public void Dash() {
        print(moveSpeed);
        if(moveSpeed < 1f) {
            // Check if the player have enough stamina
            if (stamina.transform.localScale.x > 0.3f) {
                moveSpeed = 3f;
                StartCoroutine(ResetDash());
                stamina.UseDash();
            }
        }
    }

    IEnumerator ResetDash() {
        yield return new WaitForSeconds(0.06f);
        moveSpeed = 0.8f;
    }


    private void UpdateSound() {
        if (previousPosition != rb.position) {
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED)) {
                playerFootsteps.start();
            }
        } else {
            playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
        // PLAYBACK_STATE playbackState;
        // playerFootsteps.getPlaybackState(out playbackState);
        // if (playbackState.Equals(PLAYBACK_STATE.STOPPED)) {
        //     playerFootsteps.start();
        // }
    }
}
