using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using ColourPalette;

public class UIConstraintViolation : MonoBehaviour
{
    [SerializeField]
    GameObject m_violationDetailPrefab;
    [SerializeField]
    CustomButton m_showDrawerButton;
    [SerializeField]
    CustomButton m_expandButton;
    [SerializeField]
    Transform m_detailsContainer;
    [SerializeField]
    TextMeshProUGUI m_titleText;
    [SerializeField]
    Image m_violationTypeImage;
    [SerializeField]
    Sprite m_fatalErrorSprite;
    [SerializeField]
    Sprite m_warningSprite;
    [SerializeField]
    Sprite m_errorSprite;
    [SerializeField]

    FieldData m_fieldData;
    EConstraintType m_worstType;
    List<UIConstraintViolationDetail> m_details;

    private void Start()
    {
        m_expandButton.onClick.AddListener(ToggleExpand);
        m_showDrawerButton.onClick.AddListener(OpenDrawerPath);
    }

    public void SetToFieldData(FieldData a_fieldData)
    {
        m_fieldData = a_fieldData;
        m_detailsContainer.DestroyAllChildren();
        m_details = new List<UIConstraintViolationDetail>();
        m_titleText.text = a_fieldData.Name;
        m_worstType = EConstraintType.Warning;
        for (int i = 0; i < a_fieldData.Constraints.Count; i++)
        {
            if (!a_fieldData.ConstraintViolations[i])
                continue;
            if (a_fieldData.Constraints[i].ConstaintType > m_worstType)
                m_worstType = a_fieldData.Constraints[i].ConstaintType;
            CreateViolationDetail(a_fieldData.Constraints[i]);
        }
        switch (m_worstType)
        {
            case EConstraintType.FatalError:
                m_violationTypeImage.sprite = m_fatalErrorSprite;
                m_violationTypeImage.color = DrawerManager.Instance.DrawerOptions.FatalErrorColour.GetColour();
                break;
            case EConstraintType.Error:
                m_violationTypeImage.sprite = m_errorSprite;
                m_violationTypeImage.color = DrawerManager.Instance.DrawerOptions.ErrorColour.GetColour();
                break;
            default:
                m_violationTypeImage.sprite = m_warningSprite;
                m_violationTypeImage.color = DrawerManager.Instance.DrawerOptions.WarningColour.GetColour();
                break;
        }
        a_fieldData.WorstConstraintType = m_worstType;
    }

    void Highlight()
    {
        DrawerManager.Instance.HighlightRim.HighlightObject(transform);
    }

    public void UpdateDetailText(FieldData a_fieldData)
    {
        int j = 0;
        for (int i = 0; i < a_fieldData.Constraints.Count; i++)
        {
            if (!a_fieldData.ConstraintViolations[i])
                continue;
            m_details[j].UpdateText(a_fieldData.Constraints[i]);
            j++;
        }
    }

    void ToggleExpand()
    {
        SetExpanded(!m_detailsContainer.gameObject.activeSelf);
    }

    void SetExpanded(bool a_expanded)
    {
        m_detailsContainer.gameObject.SetActive(a_expanded);
    }

    void OpenDrawerPath()
    {
        m_fieldData?.OpenPathToFieldData();
    }

    void CreateViolationDetail(Constraint a_constraint)
    {
        UIConstraintViolationDetail detail = Instantiate(m_violationDetailPrefab, m_detailsContainer).GetComponent<UIConstraintViolationDetail>();
        detail.SetDetail(a_constraint);
        m_details.Add(detail);
    }

    public bool IsError
    {
        get { return m_worstType == EConstraintType.Error; }
    }

    public bool IsFatalError
    {
        get { return m_worstType == EConstraintType.FatalError; }
    }
}

