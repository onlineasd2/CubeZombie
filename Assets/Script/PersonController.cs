using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonController : MonoBehaviour {
    
    public Camera cam;
    public CameraShake cameraShake;
    public GameObject weapon;
    public GameObject bullet; // Пуля
    [Space]

    public Transform target; // Точка прицеливания
    public Transform weaponEnd; // Точка вылета пуль
    [Space]

    public Animator animator;
    [Space]

    public ParticleSystem sleeve;
    [Space]

    public AudioClip shotSound;
    public AudioSource audioSource;
    [Space]

    public bool isShot;
    [Space]

    public Light LightFormAmmo;

    public int storeCapacity = 30; // Емкость магазина

    public float speedBulet = 10; // Скорость пули
    
    public float delayShoting = 0.01f; // Задержка при стрельбе

    float t = 0f; // Таймер

    public GameObject ScreenDead;

    public bool isDead = false;
    
    void Start ()
    {
        Instantiate(target, transform.position, Quaternion.identity);
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        ParticleSleeve(false);

        ScreenDead.SetActive(false);

        isDead = false;
    }
	
	void Update ()
    {
        DirectionShot();
        ReloadGun();
        CheckIsShot();
        Dead(isDead);
    }

    void ReloadGun() // Перезарядка
    {
        // Если магазин меньше 30 и нажата кнопка R и если Анимация перезарядки не проигрывается можно перезарядится
        if ((storeCapacity < 30) && (Input.GetKeyDown(KeyCode.R)) && (!IsAnimationPlaying("ReloadAmmo")))
        {
            animator.SetBool("ReloadAmmo", true);
            Debug.Log("Reload Ammo");
        }
    }


    // Control
    void DirectionShot()
    {
        if (Input.GetMouseButton(0))
        {
            CursorDirection(); // Get position and transform target to position

            transform.LookAt(target); // PersonController look at target

            transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, Time.deltaTime);

            DelayShoot(delayShoting);

            Debug.Log("isShot = " + isShot);
        }
        else
        {

            ParticleSleeve(false);
            AnimationShot(false);
            t = 0; //при прекращении огня обнуляем таймер, но выстрела не следует, тк не нажата кнопка стрельбы
        }
    }

    // Получения позиции попадания луча
    Vector3 CursorDirection()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition); // Пускаем луч из камеры

        bool isHit = Physics.Raycast(ray, out hit);

        if ((isHit) && (hit.collider.tag == "Enemy"))
        {
            Transform objectHit = hit.transform;
            target.position = hit.point;

            // Debug postition hit point
            // Debug.Log(objectHit.position);

            return target.transform.position;

        }
        return new Vector3(0, 0, 0);
    }


    bool Shot ()
    {
        // Если магазин не пуст и если не проигрывается анимация перезарядки
        if ((storeCapacity != 0) && (!IsAnimationPlaying("ReloadAmmo")))
        {
            GameObject copyBullet = Instantiate(bullet, weaponEnd.position, Quaternion.identity); // Создание пули

            copyBullet.GetComponent<Rigidbody>().AddForce(weaponEnd.forward * speedBulet * Time.deltaTime, ForceMode.Impulse);
            
            AnimationShot(true);

            StartCoroutine(cameraShake.Shake(.15f, .4f));

            if (copyBullet != null)
            {
                isShot = true;
            }
            else
            {
                isShot = false;
            }

            storeCapacity--; // Минус один патрон за каждый выстрел

            Destroy(copyBullet, 1); // Удаление пуль через одну секунд

            return true;
        }
        // Если кончились патроны
        else
        {
            AnimationShot(false); // отключение анимации выстрела
            ParticleSleeve(false); // отключение вылета гильз

            t = 0; // обнуление таймера
            Debug.Log("Need Ammo");
            return false;
        }
    }


    void DelayShoot(float delay)
    {
        //если таймер на 0, делаем выстрел
        if (t == 0)
        {
            Shot();
        }

        t += Time.deltaTime; //запускаем таймер

        if (t > delay) //если время больше задержки в секундах
        {
            t = 0; //обнуляем, и как следствие, делаем выстрел
        }

    }

    void Dead(bool isD)
    {
        if (isD)
        {
            ScreenDead.SetActive(true);
            gameObject.GetComponent<PersonController>().enabled = false;
        } else
        {
            ScreenDead.SetActive(false);
            gameObject.GetComponent<PersonController>().enabled = true;
        }
    }

    public void ReloadSceen()
    {
        Application.LoadLevel(0);
    }

    // Закончить анимацию перезарядки
    void AnimationOver(string nameAnimation)
    {
        animator.SetBool(nameAnimation, false);
        storeCapacity = 30; // Присвоить 30 патронов
    }

    void AnimationShot(bool turn)
    {
        if (isShot)
            animator.SetBool("Shot", turn);
        else
            animator.SetBool("Shot", turn);
    }

    void ParticleSleeve (bool turn)
    {
        if (turn)
            sleeve.Play();
        else
            sleeve.Stop();
    }

    // Проверка проигрывания анимации
    public bool IsAnimationPlaying (string animationName)
    {
        // берем информацию о состоянии
        var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // смотрим, есть ли в нем имя какой-то анимации, то возвращаем true
        if (animatorStateInfo.IsName(animationName))
            return true;

        return false;
    }

    // Проверка каждый кадр был ли выстрел
    void CheckIsShot()
    {
        if (isShot)
        {
            LightShot(true);
            ParticleSleeve(true);
            SoundShot();
            isShot = false;
        } else
        {
            LightShot(false);
        }
    }

    // Звук выстрела
    void SoundShot ()
    {
        audioSource.clip = shotSound;
        audioSource.Play();
    }

    // Вспышка от выстрела
    void LightShot (bool shot)
    {
        LightFormAmmo.gameObject.SetActive(shot);
    }

    // Вспышка от выстрела
    void LightFromShoot (bool turn)
    {
        LightFormAmmo.gameObject.SetActive(turn);
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            isDead = true;
        }
    }
}
