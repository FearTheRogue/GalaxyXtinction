using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (timer.color.a == 1)
        {


            if (timeValue > 0)
            {
                timeValue -= Time.deltaTime;
            }
            else
            {
                warningPanel.SetActive(false);

                if (AudioManager.instance.IsClipPlaying("Warning Sound"))
                    AudioManager.instance.Stop("Warning Sound");

                health.PlayerDead();
            }

            DisplayTime(timeValue);
        }
    }

    private void LateUpdate()
    { 
        warningPanelAnim.SetBool("hasWarning", isWarning);
    }

    private void DisplayWarningPanel()
    {

    }

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
        if(other.CompareTag("Death Barrier"))
        {
            health.PlayerDead();
        }

        if (!other.CompareTag("Planet"))
        {
            return;
        }

        isWarning = true;

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
        timeValue = 5f;

        if (AudioManager.instance.IsClipPlaying("Warning Sound"))
            AudioManager.instance.Stop("Warning Sound");
    }
}
