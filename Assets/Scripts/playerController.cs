using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackCooldown;
    public float moveCoolDown;
    private Rigidbody2D playerRB;
    public bool isCoolDown = false;
    Vector2 movement;
    Animator playerAnimator;
    RigidbodyConstraints2D originalContraints;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayers;
    public int playerDamage;
    public int maxHealth = 100;
    private int currentHealth;
    public float knockBackForce;
    private bool canMove = true;
    public float waitmove = 1f;

    private void Start() {
        playerRB = this.gameObject.GetComponent<Rigidbody2D>();
        playerAnimator = this.gameObject.GetComponent<Animator>();

        originalContraints = playerRB.constraints;
        currentHealth = maxHealth;
    }
    private void Update() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //avoid moving in cross directions
        if(movement.x == 1 || movement.x == -1){
            movement.y = 0;
        }else if(movement.y == 1 || movement.y == -1){
            movement.x = 0;
        }
        
        playerAnimator.SetFloat("horizontal", movement.x);
        playerAnimator.SetFloat("vertical", movement.y);
        playerAnimator.SetFloat("speed", movement.magnitude);

        //look animation
        if(movement.y > 0){
            playerAnimator.SetFloat("lookHorizontal", 0);            
            playerAnimator.SetFloat("lookVertical", 1);
        }else if(movement.y < 0){
            playerAnimator.SetFloat("lookHorizontal", 0);
            playerAnimator.SetFloat("lookVertical", -1);
        }else if(movement.x < 0){
            playerAnimator.SetFloat("lookVertical", 0);
            playerAnimator.SetFloat("lookHorizontal", -1);
        }else if(movement.x > 0){
            playerAnimator.SetFloat("lookVertical", 0);
            playerAnimator.SetFloat("lookHorizontal", 1);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        if(playerAnimator.GetFloat("lookHorizontal") < 1){
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        
        if(Input.GetKeyDown(KeyCode.T)){
            playerRB.AddForce(new Vector2(1,0) * knockBackForce * Time.deltaTime, ForceMode2D.Impulse);
        }

        Attack();

    }

    private void FixedUpdate() {
        if(canMove){
        playerRB.MovePosition(playerRB.position + movement * moveSpeed * Time.fixedDeltaTime); 
        }
        //freeze player while attacking.

    }
    private void Attack(){
        if(Input.GetKeyDown(KeyCode.Space) && !isCoolDown){
            StartCoroutine(MoveCoolDown(moveCoolDown));
            StartCoroutine(AttackCooldown(attackCooldown));
        //attack animation
        if(playerAnimator.GetFloat("lookVertical") == 0){
            playerAnimator.SetTrigger("attack");
        }else if (playerAnimator.GetFloat("lookVertical") ==1){
            playerAnimator.SetTrigger("attackUp");
        }else if(playerAnimator.GetFloat("lookVertical") == -1){
            playerAnimator.SetTrigger("attackDown");
        }            

        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        foreach(Collider2D enemy in hitEnemies){
            enemy.GetComponent<Enemy>().TakeDamage(playerDamage);
        }
        
        }
    }
    IEnumerator MoveCoolDown(float time){
        playerRB.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(time);
        playerRB.constraints = originalContraints | RigidbodyConstraints2D.FreezeRotation; 
    }
    IEnumerator AttackCooldown(float time){
        isCoolDown = true;
        yield return new WaitForSeconds(time);
        isCoolDown = false;

    }

    //Draw range of attack.
    private void OnDrawGizmos(){
        if(attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
    public void TakeDamage(int damage){
        currentHealth -= damage;
        playerAnimator.SetTrigger("getHit");
        canMove =false;
        Collider2D detectEnemies = Physics2D.OverlapCircle(playerRB.position, attackRange, enemyLayers); 
        Vector2 forceDirection = (playerRB.position - detectEnemies.GetComponent<Rigidbody2D>().position).normalized;
        playerRB.AddForce(forceDirection * 0.01f * knockBackForce * Time.deltaTime, ForceMode2D.Impulse);
        StartCoroutine(waitMove(waitmove));
        if(currentHealth <= 0){
            playerAnimator.SetBool("isDead", true);
            playerRB.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.enabled = false;
        }
        
    }
    IEnumerator waitMove(float time){
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
