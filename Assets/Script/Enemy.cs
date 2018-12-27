using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int HP = 1;

    [Space]
    public Transform player;
    public float forceJump;
    public float delayJump;
    
    public ParticleSystem particle;
    [Space]

    public bool isGround;

    float t = 1;

    bool playerIsAlive;
    
	void Start () {
        particle.Stop();
    }
	
	void Update () {
        EnemyControl(delayJump);
        Dead();
    }

    private void EnemyControl (float delay)
    {
        if (t == 0) //если таймер на 0, делаем прыжок
            Jump();

        t += Time.deltaTime; //запускаем таймер

        if (t > delay) //если время больше задержки в секундах
        {
            t = 0; //обнуляем, и как следствие, делаем прыжок
        }
    }

    private void Jump ()
    {
        if (isGround)
        {
            transform.LookAt(player);
            GetComponent<Rigidbody>().AddForce((transform.forward + Vector3.up) * forceJump, ForceMode.Impulse);
        }
    }

    // Смерть противника
    private void Dead ()
    {
        if (HP <= 0)
        {
            Debug.Log("Enemy Dead");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter (Collision coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            particle.Play();
        } else
        {
            particle.Stop();
        }
    }

    // Стоит ли враг на земле
    private void OnCollisionStay (Collision coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            isGround = true;
        }
    }

    private void OnCollisionExit (Collision coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            isGround = false;
        }
    }
}
