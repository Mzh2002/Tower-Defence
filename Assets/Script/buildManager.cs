using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class buildManager : MonoBehaviour
{

    public turretData laserData;
    public turretData missleData;
    public turretData standardData;

    private upgradeController controller; 

    private turretData selectedTurret;

    private mapCube selectedMapCube;

    public Text moneyText;
    private int money = 500;

    public Animator moneyAnimator;

    public static buildManager instance;

    private void Start()
    {
        instance = this;
        controller = GetComponent<upgradeController>();
    }

    public void updateMoney(int change = 0)
    {
        money += change;
        moneyText.text = "$" + money;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                // build turret
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube"));
                if (isCollider)
                {
                    // if mouse hit a mapCube
                    mapCube MapCube = hit.collider.GetComponent<mapCube>();
                    if (selectedTurret != null && MapCube.turretGo == null)
                    {
                        if (money >= selectedTurret.cost)
                        {
                            updateMoney(-selectedTurret.cost);
                            MapCube.buildTurret(selectedTurret);
                        }
                        else
                        {
                            // if lack money, trigger money animation
                            moneyAnimator.SetTrigger("flick");
                        }
                    }
                    else if (MapCube.turretGo != null)
                    {
                        if (MapCube == selectedMapCube && controller.upgradeCanvas.activeInHierarchy)
                        {
                            StartCoroutine(controller.HideUpgradeUI());
                        }
                        else
                        {
                            controller.ShowUpgradeUI(MapCube, MapCube.isUpgraded);
                            selectedMapCube = MapCube;
                        }
                    }
                }
            }
        }
    }

    public void OnLaserSelected(bool IsOn)
    {
        if (IsOn)
        {
            selectedTurret = laserData;
        }
    }

    public void OnMissleSelected(bool IsOn)
    {
        if (IsOn)
        {
            selectedTurret = missleData;
        }
    }

    public void OnStandardSelected(bool IsOn)
    {
        if (IsOn)
        {
            selectedTurret = standardData;
        }
    }

    public void OnUpgradeButtonDown()
    {
        int upgradeCost = selectedMapCube.TurretData.upgradeCost;
        if (money < upgradeCost)
        {
            moneyAnimator.SetTrigger("flick");
            controller.ShowUpgradeUI(selectedMapCube);
        }
        else
        {
            updateMoney(-upgradeCost);
            selectedMapCube.upgradeTurret();
            StartCoroutine(controller.HideUpgradeUI());
        }
    }

    public void OnDestroyButtonDown()
    {
        selectedMapCube.destroyTurret();
        StartCoroutine(controller.HideUpgradeUI());
    }
}
