using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldofView : MonoBehaviour
{
    public Transform viewObj;
    public float viewRange;
    public float moveSpeed;
    public LayerMask playerLayer;
    Rigidbody2D playerRB;
    Rigidbody2D rb;


    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(DetectPlayer()){
            Vector2 direction = (playerRB.position - this.rb.position).normalized;
            rb.AddForce(direction * moveSpeed * Time.fixedDeltaTime);
        }
    }
    private void OnDrawGizmos() {
        if(viewObj == null){
            Gizmos.color = Color.red;
        }else{
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireSphere(viewObj.position, viewRange);                
    }
    
    public bool DetectPlayer(){
        Collider2D collider = Physics2D.OverlapCircle(viewObj.position, viewRange, playerLayer);
        if(collider){
            return true;
        }
        return false;
    }
}
