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
    public ParticleSystem particleExplosion;
    [Space]

    public Light lightExplosion;
    [Space]

    public GameObject deadEnemy;

    public bool isGround;

    public bool isDead = false;

    float t = 1;

    bool playerIsAlive;

    bool once = false;

    void Start () {
        particle.Stop();
    }
	
	void Update () {
        // Отключение взрыва
        particleExplosion.Stop();
        lightExplosion.gameObject.SetActive(false);
        // Отключение взрыва

        EnemyControl(delayJump);
        CheckHP();
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
    // Проверка жизней противника
    private void CheckHP ()
    {
        if (HP <= 0)
        {
            isDead = true;
        }
    }

    // Смерть противника
    void Dead ()
    {
        if (isDead)
        {
            if (!once)
            {
                once = true;

                Debug.Log("Enemy Dead");

                particleExplosion.Play();
                lightExplosion.gameObject.SetActive(true);

                // Доделать
                GameObject enemyD = Instantiate(deadEnemy, transform.position, Quaternion.identity);

                foreach (Transform child in enemyD.GetComponentInChildren<Transform>())
                {
                    float direction = Random.Range(-2.5f, 2.5f);
                    child.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(direction, direction, direction), ForceMode.Impulse);
                }

                // Доделать

                Destroy(enemyD, 1.5f);
            }
            Destroy(gameObject);
        }
        else
        {
            lightExplosion.gameObject.SetActive(false);
            particleExplosion.Stop();
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
