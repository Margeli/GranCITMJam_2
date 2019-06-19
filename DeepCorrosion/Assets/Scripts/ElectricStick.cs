using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricStick : MonoBehaviour
{
    Animator anim;
    Player playerScript;
    StickCollider collider;
    ParticleSystem particle_system;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerScript = transform.parent.GetComponent<Player>();
        collider = transform.GetChild(0).GetComponent<StickCollider>();
        particle_system = transform.Find("Emitter Parent").GetComponent<ParticleSystem>();

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
        StartCoroutine(Countdown());

    }

    IEnumerator Countdown()
    {

        while (true)
        {
            if (!particle_system.gameObject.activeSelf)
            {
                particle_system.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.3f);
            }
            else
            {
                particle_system.gameObject.SetActive(false);
                StopAllCoroutines();
                yield return null;
            }
        }


    }
}
