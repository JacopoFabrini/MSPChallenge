using UnityEngine;
using UnityEngine.UI;

public class SavedChangesIcon : MonoBehaviour
{
    [SerializeField] private Image m_icon;

    [SerializeField] private Color m_unsavedColour;
    [SerializeField] private Sprite m_unsavedIcon;

    [SerializeField] private Color m_savedColour;
    [SerializeField] private Sprite m_savedIcon;

    public void SetSaved(bool a_saved)
    {
        if (a_saved)
        {
            m_icon.sprite = m_savedIcon;
            m_icon.color = m_savedColour;
        }
        else
        {
            m_icon.sprite = m_unsavedIcon;
            m_icon.color = m_unsavedColour;
        }
    }
}
