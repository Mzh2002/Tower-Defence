using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public int damage = 50;
    public float speed = 50;

    private Transform target;

    public GameObject explosionEffectPrefab;

    public float explosionDistance = 1.2f;

    public void setTarget(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            explode();
            return;
        }

        // make bullet track enemy
        transform.LookAt(target.position);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // explode when hit enemy
        Vector3 dir = transform.position - target.position;
        if(dir.magnitude <= explosionDistance)
        {
            target.GetComponent<enemy>().takeDamage(damage);
            explode();
        }
    }

    private void explode()
    {
        GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
        Destroy(this.gameObject);
        Destroy(effect, 1);
    }
}
