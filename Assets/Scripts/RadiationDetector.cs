using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadiationDetector : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float distance;
    [SerializeField] private float maxDistance;

    [SerializeField] private GameObject radiationPanel;

    [Header("Radiation Detector")]
    [SerializeField] private Image detectorRadial;
    [SerializeField] private Image radiationIcon;
    [SerializeField] private Text Progress;

    [SerializeField] private Text timer;
    [SerializeField] private float timeValue;

    [SerializeField] private Animator radiationPanelAnim;

    [SerializeField] private FPSHealth health;

    private void Awake()
    {
        timer.text = "00:00";
    }

    private void Update()
    {
        if (distance > maxDistance)
        {
            radiationPanelAnim.SetBool("hasRadiation", true);

            if (timer.color.a == 1)
            {
                if (timeValue > 0)
                {
                    timeValue -= Time.deltaTime;
                }
                else
                {
                    this.enabled = false;
                    radiationPanel.SetActive(false);
                    health.PlayerDie();
                }

                DisplayTime(timeValue);
            }
        }
        else
        {
            radiationPanelAnim.SetBool("hasRadiation", false);
            timeValue = 5;
        }
    }

    private void LateUpdate()
    {
        distance = DistanceFromShip();

        float dist = distance / 100;

        detectorRadial.fillAmount = (dist / maxDistance) * 100;
        radiationIcon.fillAmount = detectorRadial.fillAmount;
        Progress.text = Mathf.RoundToInt(detectorRadial.fillAmount * 100).ToString() + "%";
    }

    private float DistanceFromShip()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    private void DisplayTime(float time)
    {
        if(time < 0)
        {
            time = 0;
        }

        float seconds = Mathf.FloorToInt(time % 60);
        float milliseconds = time % 1 * 1000;

        timer.text = string.Format("00:{0:00}:{1:000}", seconds, milliseconds);
    }
}
