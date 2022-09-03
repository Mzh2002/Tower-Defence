using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{

    public int movingSpeed = 10;
    private Transform[] positions;
    private int idx = 0;

    public float totalHp = 150;
    private float hp;
    public int onDieAward = 100;

    public GameObject explosionEffectPrefab;

    public Slider hpSlider;


    // Start is called before the first frame update
    void Start()
    {
        hp = totalHp;
        positions = wayPoints.positions;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        // when enemy reach the end of the road
        if (idx >= positions.Length)
        {
            ReachDestination();
            return;
        }
        // move toward the next waypoint
        transform.Translate((positions[idx].position - transform.position).normalized * Time.deltaTime * movingSpeed);
        if (Vector3.Distance(positions[idx].position, transform.position) < 0.4f)
        {
            idx++;
        }
    }

    void ReachDestination()
    {
        gameManager.Instance.fail();
        GameObject.Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        enemySpawner.enemyAlive--;
    }

    public void takeDamage(float damage)
    {
        hp -= damage;
        hpSlider.value = hp / totalHp;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
        Destroy(this.gameObject);
        Destroy(effect, 1.5f);
        buildManager.instance.updateMoney(onDieAward);
    }
}
