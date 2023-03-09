using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public int enemyDamage;
    public LayerMask playerLayer;
    private Animator enemyAnimator;
    private Rigidbody2D enemyRB;
    private Rigidbody2D playerRB;
    public Transform attackPoint;
    public float attackRange;
    public float knockBackForce = 5f;
    private bool isCoolDown = false;
    public float attackCooldown;
    public float attackDelay;
    public float staggerTime;
    public HealthBar healthBar;
    public bool isEnemyDead = false;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);

        enemyAnimator = this.gameObject.GetComponent<Animator>();
        enemyRB = this.gameObject.GetComponent<Rigidbody2D>();
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }
    private void Update() {

        if(enemyRB.velocity.x >=0.1f || enemyRB.velocity.x <=-0.1f){
            enemyAnimator.SetBool("Walking", true);
        }else{
            enemyAnimator.SetBool("Walking", false);
        }
        //flip enemy if player is on right.
        if(enemyRB.velocity.x >= 0.1f){
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }else if(enemyRB.velocity.x <= -0.1f){
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        attackPlayer();
        if(Input.GetKeyDown(KeyCode.O)){
            enemyRB.AddForce(new Vector2(1,0) * knockBackForce * Time.deltaTime, ForceMode2D.Impulse);
        }

    }
    public void TakeDamage(int damage){
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        enemyAnimator.SetTrigger("Hurt");
        Vector2 forceDirection = (enemyRB.position - playerRB.position).normalized;
        enemyRB.AddForce(forceDirection * knockBackForce * Time.deltaTime, ForceMode2D.Impulse);
        StartCoroutine(Stagger(staggerTime));        

        if(currentHealth <= 0){
            isEnemyDead = true;
            enemyAnimator.SetBool("isDead", true);
            this.enabled = false;
        }
        
    }
    private void Die(){
        Debug.Log(this.name + " Dead");
        Destroy(this.gameObject);
    }

    private void attackPlayer(){
        Collider2D collider = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if(collider && isCoolDown == false){
            StartCoroutine(AttackDelay(attackDelay));
            StartCoroutine(AttackCooldown(attackCooldown));

        }
    }
    IEnumerator AttackCooldown(float time){
        isCoolDown = true;
        yield return new WaitForSeconds(time);
        isCoolDown = false;
    }
    IEnumerator AttackDelay(float time){
        yield return new WaitForSeconds(time);
        //check again if the player is colliding with enemy.
        Collider2D collider = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if(collider){
            GameObject.Find("Player").GetComponent<playerController>().TakeDamage(enemyDamage);
            enemyAnimator.SetTrigger("Attack");
        }
        
    }
    IEnumerator Stagger(float time){
        this.enabled = false;
        yield return new WaitForSeconds(time);
        this.enabled = true;
    }
    
    private void OnDrawGizmos() {
        if(attackPoint == null){
            Gizmos.color = Color.white;
        }else{
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);                
    }
}
