using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricStick : MonoBehaviour
{
    Animator anim;
    Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerScript = transform.parent.GetComponent<Player>();

    }

    public void Attack()
    {
        anim.SetBool("attacking", true);
        
    }
    private void EndAttack()
    {
        anim.SetBool("attacking", false);
        playerScript.EndAttack();
    }
    private void doEffect()
    {
        Debug.Log("effect");

    }
}
