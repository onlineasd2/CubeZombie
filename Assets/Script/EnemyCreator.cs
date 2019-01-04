using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCreator : MonoBehaviour {

    public int countEnemy;
    public GameObject [] kindsEnemy;
    public int waves;
    public bool creatEnemy = true;
    public float deleyCreatEnemy = 1f;
    [Space]

    public Slider sliderDeley;
    public InputField inputCountEnemy;
    public InputField inputWave;
    public Toggle toggleSpawnEnemy;
    [Space]

    public Toggle ui;
    public GameObject controllUI;
    
	void Start () {
    }

    void Update ()
    {
        if (ui.isOn)
            controllUI.SetActive(true);
        else
            controllUI.SetActive(false);

        if ((waves != 0) && (creatEnemy))
        {
            Invoke("CreateEnemy", (float)deleyCreatEnemy); //выполняем sec каждые 1 секунд
            waves--;
        }
    }

    public void ApplySettings()
    {
        waves = int.Parse(inputCountEnemy.text);
        creatEnemy = sliderDeley;
        deleyCreatEnemy = sliderDeley.value;
        countEnemy = int.Parse(inputWave.text);
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
