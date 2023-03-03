using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int maxHealth = 100;
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
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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
    }
    public void TakeDamage(int damage){
        currentHealth -= damage;
        enemyAnimator.SetTrigger("Hurt");

        Vector2 forceDirection = (enemyRB.position - playerRB.position).normalized;
        enemyRB.AddForce(forceDirection * knockBackForce * Time.deltaTime, ForceMode2D.Impulse);
        Debug.Log(forceDirection);

        if(currentHealth <= 0){
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
    
    private void OnDrawGizmos() {
        if(attackPoint == null){
            Gizmos.color = Color.white;
        }else{
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);                
    }
}
