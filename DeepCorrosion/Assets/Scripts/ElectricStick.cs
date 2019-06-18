using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricStick : MonoBehaviour
{
    Animator anim;
    Player playerScript;
    StickCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerScript = transform.parent.GetComponent<Player>();
        collider = transform.GetChild(0).GetComponent<StickCollider>();

    }

    public void Attack()
    {
        anim.SetBool("attacking", true);
        collider.gameObject.SetActive(true);
        
    }
    private void EndAttack()
    {
        anim.SetBool("attacking", false);
        playerScript.EndAttack();
        collider.gameObject.SetActive(false);
    }
    private void doEffect()
    {
        Debug.Log("effect");

    }
}
