using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using ColourPalette;

public class UIConstraintDrawerButton : MonoBehaviour
{
    [SerializeField]
    CustomButton m_constraintButton;
    [SerializeField]
    CustomImage m_buttonImage;

    [SerializeField]
    ColourAsset m_fatalErrorColour;
    [SerializeField]
    ColourAsset m_warningColour;
    [SerializeField]
    ColourAsset m_errorColour;

    public void SetStateAndCallback(UnityAction a_callback, EConstraintType a_worstViolation)
    {
        m_constraintButton.onClick.RemoveAllListeners();
        m_constraintButton.onClick.AddListener(a_callback);
        switch(a_worstViolation)
        {
            case EConstraintType.Error:
                m_buttonImage.ColourAsset = m_errorColour;
                break;
            case EConstraintType.Warning:
                m_buttonImage.ColourAsset = m_warningColour;
                break;
            default:
                m_buttonImage.ColourAsset = m_fatalErrorColour;
                break;

        }
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        m_constraintButton.onClick.RemoveAllListeners();
    }
}

