using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : BaseWeapon
{
    // Интарвал нанесения урона
    public float damageInterval = 0.1f;
    
    // Наносимый лазером урон
    public float damage = 0.1f;
    
    // Шаблон для создания луча лазера
    public GameObject laserPrefab;
    
    // Содержит true, если в данный момент ведется огонь.
    private bool isFiring = false;

    // Список активных лазеров
    private List<LaserBeam> currentLaserBeams = new List<LaserBeam>();

    // Вызывается, чтобы начать огонь
    public override void StartFiring()
    {
        // Запустить сопрограмму ведения огня
        StartCoroutine(Firing());
    }

    // Вызывается, когда прекращается огонь
    public override void StopFiring()
    {
        // Присвоить false, чтобы завершить цикл в Firing
        isFiring = false;
    }

    IEnumerator Firing()
    {
        // Установить признак ведения огня
        isFiring = true;

        Fire();
        
        // Продолжать итерации, пока isFiring равна true
        while (isFiring)
        {
            foreach (var laserBeam in currentLaserBeams)
            {
                if (laserBeam.hitting)
                {
                    // Нанести повреждение объекту, в который попал лазер,
                    // если возможно.
                    var theirDamage =
                        laserBeam.hitedObject.GetComponentInParent<DamageTaking>();
                    if (theirDamage)
                    {
                        theirDamage.TakeDamage(damage);
                    }
                }
            }

            // Ждать damageInterval секунд перед
            // следующим нанесением урона
            yield return new WaitForSeconds(damageInterval);
        }
        
        // Уничтожаем лазеры при остановке стрельбы
        foreach (var laserBeam in currentLaserBeams)
        {
            Destroy(laserBeam.gameObject);
        }
        currentLaserBeams.Clear();
    }

    // Создаёт лазерные лучи
    void Fire()
    {
        foreach (var firePoint in firePoints)
        {
            var laserBeamObject = Instantiate(laserPrefab,
                                                firePoint.position,
                                                firePoint.rotation);

            var laserBeam = laserBeamObject.GetComponent<LaserBeam>();

            if (laserBeam != null)
            {
                currentLaserBeams.Add(laserBeam);
            }
            
            // Если пушка имеет компонент источника звука,
            // воспроизвести звуковой эффект
            var audio = firePoint.GetComponent<AudioSource>();
            if (audio)
            {
                audio.Play();
            }
        }

        StartCoroutine(Moving());
    }
    
    IEnumerator Moving()
    {
        // Продолжать итерации, пока isFiring равна true
        while (isFiring)
        {
            for (int i = 0; i < currentLaserBeams.Count; ++i)
            {
                currentLaserBeams[i].transform.position = firePoints[i].position;
                currentLaserBeams[i].transform.rotation = firePoints[i].rotation;
            }

            // Ждать следующего одновления физики (FixedUpdate)
            yield return new WaitForFixedUpdate();
        }
    }
}
