using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour {

    public int countEnemy;
    public GameObject [] kindsEnemy;
    public int Waves;
    
	void Start () {
    }

    void Update ()
    {
        if (Waves != 0)
        {
            Invoke("CreateEnemy", 1); //выполняем sec каждые 1 секунд
            Waves--;
        }
    }

    void CreateEnemy()
    {
        for (int i = 0; i < countEnemy; i++)
        {
            float jumpDelay = Random.Range(1f, 1.5f);
            float jumpForce = Random.Range(1.5f, 2f);
            float randDirectionZ = Random.Range(transform.position.z, transform.position.x - 17f);
            float randDirectionX = Random.Range(transform.position.x, transform.position.x + 3f);

            GameObject copyEnemy = Instantiate(kindsEnemy[0], new Vector3(randDirectionX, 1.19f, randDirectionZ), Quaternion.identity); // Создание врагов

            copyEnemy.GetComponent<Enemy>().delayJump = jumpDelay;
            copyEnemy.GetComponent<Enemy>().forceJump = jumpForce;
        }
    }
}
