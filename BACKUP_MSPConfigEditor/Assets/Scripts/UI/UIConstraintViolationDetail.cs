using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ColourPalette;
using TMPro;

public class UIConstraintViolationDetail : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_detailText;
    [SerializeField]
    CustomImage m_violationTypeImage;

    public void SetDetail(Constraint a_constraint)
    {
        m_detailText.text = a_constraint.GetViolationText();
        switch (a_constraint.ConstaintType)
        {
            case EConstraintType.Error:
                m_violationTypeImage.ColourAsset = DrawerManager.Instance.DrawerOptions.ErrorColour;
                break;
            case EConstraintType.Warning:
                m_violationTypeImage.ColourAsset = DrawerManager.Instance.DrawerOptions.WarningColour;
                break;
            default:
                m_violationTypeImage.ColourAsset = DrawerManager.Instance.DrawerOptions.FatalErrorColour;
                break;
        }
    }

    public void UpdateText(Constraint a_constraint)
    {
        m_detailText.text = a_constraint.GetViolationText();
    }
}

