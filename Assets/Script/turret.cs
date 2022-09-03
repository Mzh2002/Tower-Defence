using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    private List<GameObject> enemies = new List<GameObject>();

    public float attackRate = 1;
    private float timer = 0;

    public GameObject bulletPrefab;
    public Transform firePosition;

    public Transform head;

    // special for Laser
    public bool useLaser = false;
    public float damageRate = 10;
    public LineRenderer laser;
    public GameObject laserEffect;

    private void Start()
    {
        timer = attackRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        if (!useLaser)
        {
            timer += Time.deltaTime;
            if (enemies.Count > 0 && timer >= attackRate)
            {
                timer = 0;
                Attack();
            }
        }
        else
        {
            // special for laser turret
            if (enemies.Count > 0)
            {
                if (!laser.enabled)
                {
                    laser.enabled = true;
                    laserEffect.SetActive(true);
                }
                LaserAttack();
            }
            else
            {
                laser.enabled = false;
                laserEffect.SetActive(false);
            }
        }

        if (enemies.Count > 0 && enemies[0] != null)
        {
            Vector3 targetPosition = enemies[0].transform.position;
            targetPosition.y = transform.position.y;
            head.LookAt(targetPosition);
        }
    }

    private void Attack()
    {
        if (enemies[0] == null)
        {
            updateEnemies();
        }
        if (enemies.Count > 0)
        {
            GameObject Bullet = GameObject.Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            Bullet.GetComponent<bullet>().setTarget(enemies[0].transform);
        }
    }

    private void LaserAttack()
    {
        if (enemies[0] == null)
        {
            updateEnemies();
        }
        if (enemies.Count > 0)
        {
            laser.SetPositions(new Vector3[] {firePosition.position, enemies[0].transform.position});
            enemies[0].GetComponent<enemy>().takeDamage(damageRate * Time.deltaTime);
            laserEffect.transform.position = enemies[0].transform.position;
            Vector3 pos = transform.position;
            pos.y = enemies[0].transform.position.y;
            laserEffect.transform.LookAt(pos);
        }
    }

    private void updateEnemies()
    {
        List<int> emptyIndex = new List<int>();
        // enemies.RemoveAll(null);
        for (int i=0; i<enemies.Count; i++)
        { 
            if (enemies[i] == null)
            {
                emptyIndex.Add(i);
            }
        }

        for (int i=0; i<emptyIndex.Count; i++)
        {
            enemies.RemoveAt(emptyIndex[i] - i);
        }
    }
}
