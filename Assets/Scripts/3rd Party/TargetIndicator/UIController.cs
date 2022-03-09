using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] public List<TargetIndicator> targetIndicators = new List<TargetIndicator>();
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject targetIndicatorPrefab;

    private void Update()
    {
        if (targetIndicators.Count > 0)
        {
            for (int i = 0; i < targetIndicators.Count; i++)
            {
                targetIndicators[i].UpdateTargetIndicator();
            }
        }
    }

    public void AddTargetIndicator(GameObject target)
    {
        TargetIndicator indicator = GameObject.Instantiate(targetIndicatorPrefab, canvas.transform).GetComponent<TargetIndicator>();
        indicator.InitialiseTargetIndicator(target, playerCamera, canvas);
        targetIndicators.Add(indicator);
    }
}
