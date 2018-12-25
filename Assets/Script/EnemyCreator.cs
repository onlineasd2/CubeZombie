using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour {

    public int countEnemy;
    public GameObject [] kindsEnemy;
    
	void Start () {
    }

    void Update ()
    {
        if (!IsInvoking("CreateEnemy"))
        {
            Invoke("CreateEnemy", 1); //выполняем sec каждые 5 секунд
        }
    }

    void CreateEnemy()
    {
        for (int i = 0; i < countEnemy; i++)
        {
            float jumpDelay = Random.Range(1f, 3f);
            float jumpForce = Random.Range(5f, 10f);
            float randDirectionZ = Random.Range(-13f, -14f);
            float randDirectionX = Random.Range(transform.position.x, transform.position.x + 3f);

            GameObject copyEnemy = Instantiate(kindsEnemy[0], new Vector3(randDirectionX, 1.19f, randDirectionZ), Quaternion.identity); // Создание врагов

            copyEnemy.GetComponent<Enemy>().delayJump = jumpDelay;
            copyEnemy.GetComponent<Enemy>().forceJump = jumpForce;
        }
    }
}
