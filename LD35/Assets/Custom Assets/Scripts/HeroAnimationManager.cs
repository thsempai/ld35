using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Animator))]

public class HeroAnimationManager : MonoBehaviour {

    private Animator animator;
    public Rigidbody hero_rigidbody ;


    // Use this for initialization
    void Start () {
    animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update () {
        animator.SetFloat("speed", Mathf.Abs(hero_rigidbody.velocity.x));
        if(Input.GetKeyDown("space")||Input.GetMouseButtonDown(0)){
             animator.SetBool("attack", true);
        }
        if(Input.GetKeyUp("space") || Input.GetMouseButtonUp(0)){
             animator.SetBool("attack", false);
        }
    }
}
