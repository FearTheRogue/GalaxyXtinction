using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// Handles the Player range on the planets. Using the distance between the player and the ship
/// If the player exceeds the range the radiation panel is set to active and a 5 second timer then starts
/// to count down, if it hits 0, the player dies. Else, it is set to deactive.
/// 
/// </summary>

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
            // Triggers the panel to true
            radiationPanelAnim.SetBool("hasRadiation", true);

            // Waits until the timer colour alpha to be full
            if (timer.color.a == 1)
            {
                // Play audio clip
                if (!AudioManager.instance.IsClipPlaying("Warning Sound"))
                {
                    AudioManager.instance.Play("Warning Sound");
                }

                // Decreases timeValue
                if (timeValue > 0)
                {
                    timeValue -= Time.deltaTime;
                }
                else
                {
                    // timeValue has reached 0
                    this.enabled = false;
                    radiationPanel.SetActive(false);

                    AudioManager.instance.Stop("Warning Sound");
                    health.PlayerDie();
                }

                DisplayTime(timeValue);
            }
        }
        else
        {
            // Triggers the panel to false
            radiationPanelAnim.SetBool("hasRadiation", false);
            AudioManager.instance.Stop("Warning Sound");
            
            // Resets timeValue
            timeValue = 5;
        }
    }

    private void LateUpdate()
    {
        distance = DistanceFromShip();

        // Distance is divided by 100 then the current distance is divided by the max distance
        // and multiplied by 100 to get the percentage
        float dist = distance / 100;

        // Update the UI
        detectorRadial.fillAmount = (dist / maxDistance) * 100;
        radiationIcon.fillAmount = detectorRadial.fillAmount;
        Progress.text = Mathf.RoundToInt(detectorRadial.fillAmount * 100).ToString() + "%";
    }

    // Returns the distance between the player and the ship
    private float DistanceFromShip()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    // Formats and displays the time
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
