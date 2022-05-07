using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// Handles the warning detection when the player flys to close to a planet.
/// 
/// </summary>

public class WarningManager : MonoBehaviour
{
    [SerializeField] private GameObject warningPanel;

    [SerializeField] private Text timer;
    [SerializeField] private float timeValue;

    [SerializeField] private Animator warningPanelAnim;

    [SerializeField] private bool isWarning = false;
    [SerializeField] private bool checkBool;

    private HealthSystem health;

    private void Start()
    {
        health = gameObject.GetComponentInParent<HealthSystem>();
    }

    private void Update()
    {
        // If the alpha of the timer is full
        if (timer.color.a == 1)
        {
            if (timeValue > 0)
            {
                // Decrease timeValue
                timeValue -= Time.deltaTime;
            }
            else
            {
                // Set warningPanel to false
                warningPanel.SetActive(false);

                // Stops the warning audio
                if (AudioManager.instance.IsClipPlaying("Warning Sound"))
                    AudioManager.instance.Stop("Warning Sound");

                // Player dies
                health.PlayerDead();
            }

            DisplayTime(timeValue);
        }
    }

    private void LateUpdate()
    { 
        // Sets the animation state to the isWarning bool
        warningPanelAnim.SetBool("hasWarning", isWarning);
    }

    // Formats the time string
    private void DisplayTime(float time)
    {
        if (time < 0)
        {
            time = 0;
        }

        float seconds = Mathf.FloorToInt(time % 60);
        float milliseconds = time % 1 * 1000;

        timer.text = string.Format("00:{0:00}:{1:000}", seconds, milliseconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player triggers the Death Barrier, the player instantly dies
        if(other.CompareTag("Death Barrier"))
        {
            health.PlayerDead();
        }

        // If the player triggers any other gameobject that isnt Planet, return 
        if (!other.CompareTag("Planet"))
        {
            return;
        }

        isWarning = true;

        // Play the audio clip, if it isnt playing already
        if (!AudioManager.instance.IsClipPlaying("Warning Sound"))
        {
            AudioManager.instance.Play("Warning Sound");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Planet"))
        {
            return;
        }

        isWarning = false;
        
        // Resets timeValue
        timeValue = 5f;

        // Stop playing audio
        if (AudioManager.instance.IsClipPlaying("Warning Sound"))
            AudioManager.instance.Stop("Warning Sound");
    }
}
