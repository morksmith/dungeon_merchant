using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialSequence : MonoBehaviour
{
    public List<TutorialStep> TutorialSteps;
    public int CurrentStep;
    public TextMeshProUGUI TutorialText;
    public RectTransform TextBox;

    private void Start()
    {
        for(var i = 0; i < TutorialSteps.Count; i++)
        {
            if(i != CurrentStep)
            {
                TutorialSteps[i].NextButton.SetActive(false);
            }
        }
    }

    public void NextStep()
    {
        CurrentStep++;
        if(CurrentStep < TutorialSteps.Count)
        {
            for(var i = 0; i < TutorialSteps[CurrentStep].StepEvents.Count; i++)
            {
                TutorialSteps[CurrentStep].StepEvents[i].Invoke();
            }
            TutorialText.text = TutorialSteps[CurrentStep].StepText;
            TutorialSteps[CurrentStep].NextButton.SetActive(true);
            TextBox.anchoredPosition = new Vector3(0, TutorialSteps[CurrentStep].TextboxPosition, 0);
        }
        for(var i = 0; i < TutorialSteps.Count; i++)
        {
            if (i != CurrentStep)
            {
                TutorialSteps[i].NextButton.SetActive(false);
            }
        }

    }
}
