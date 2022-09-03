using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mapCube : MonoBehaviour
{

    // (whether) there is a turret on the cube
    [HideInInspector]
    public GameObject turretGo;
    [HideInInspector]
    public turretData TurretData;

    public GameObject buildEffect;

    private new Renderer renderer;

    [HideInInspector]
    public bool isUpgraded = false;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void buildTurret(turretData data)
    {
        turretGo = GameObject.Instantiate(data.turretPrefab, transform.position, Quaternion.identity);
        TurretData = data;
        useBuildEffect();
    }

    public void upgradeTurret()
    {
        if (isUpgraded) return;
        Destroy(turretGo);
        turretGo = GameObject.Instantiate(TurretData.upgradedPrefab, transform.position, Quaternion.identity);
        isUpgraded = true;
        useBuildEffect();
    }

    public void destroyTurret()
    {
        Destroy(turretGo);
        isUpgraded = false;
        TurretData = null;
        turretGo = null;
        useBuildEffect();
    }

    private void OnMouseEnter()
    {
        if (turretGo == null && EventSystem.current.IsPointerOverGameObject() == false)
        {
            renderer.material.color = Color.red;
        }
    }

    private void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }

    private void useBuildEffect()
    {
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
}
