using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonController : MonoBehaviour {
    
    public Camera cam;
    public GameObject weapon;
    public GameObject bullet; // Пуля
    [Space]

    public Transform target; // Точка прицеливания
    public Transform weaponEnd; // Точка вылета пуль
    [Space]

    public Animator animator;
    [Space]

    public int storeCapacity = 30; // Емкость магазина

    public float speedBulet = 10; // Скорость пули
    
    public float delayShoting = 0.01f; // Задержка при стрельбе

    float t = 0f; // Таймер
    
    void Start ()
    {
        Instantiate(target, transform.position, Quaternion.identity);
        animator = GetComponent<Animator>();
    }
	
	void Update ()
    {
        DirectionShot();
        ReloadGun();
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
        }
        else
        {
            t = 0; //при прекращении огня обнуляем таймер, но выстрела не следует, тк не нажата кнопка стрельбы
        }
    }

    // Получения позиции попадания луча
    Vector3 CursorDirection()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition); // Пускаем луч из камеры

        bool isHit = Physics.Raycast(ray, out hit);

        if ((isHit) && !(hit.collider.tag == "Ground"))
        {
            Transform objectHit = hit.transform;
            target.position = hit.point;

            // Debug postition hit point
            // Debug.Log(objectHit.position);

            return target.transform.position;

        }
        return new Vector3(0, 0, 0);
    }


    void Shot ()
    {
        // Если магазин не пуст и если не проигрывается анимация перезарядки
        if ((storeCapacity != 0) && (!IsAnimationPlaying("ReloadAmmo")))
        {
            GameObject copyBullet = Instantiate(bullet, weaponEnd.position, Quaternion.identity); // Создание пули

            copyBullet.GetComponent<Rigidbody>().AddForce(weaponEnd.forward * speedBulet * Time.deltaTime, ForceMode.Impulse);

            storeCapacity--; // Минус один патрон за каждый выстрел

            Destroy(copyBullet, 1); // Удаление пуль через одну секунду

        }
        // Если кончились патроны
        else
        {
            t = 0; // обнуление таймера
            Debug.Log("Need Ammo");
        }
    }

    void DelayShoot(float delay)
    {
        if (t == 0) //если таймер на 0, делаем выстрел
            Shot();

        t += Time.deltaTime; //запускаем таймер

        if (t > delay) //если время больше задержки в секундах
        {
            t = 0; //обнуляем, и как следствие, делаем выстрел
        }
    }

    // Закончить анимацию перезарядки
    void AnimationOver (string nameAnimation)
    {
        animator.SetBool(nameAnimation, false);
        storeCapacity = 30;
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
}
