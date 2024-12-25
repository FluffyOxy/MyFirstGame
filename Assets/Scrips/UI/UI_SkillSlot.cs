using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    public bool isUnlocked;

    [Space]
    [SerializeField] private string skillName;
    [SerializeField] Image skillIcon;
    [TextArea][SerializeField] private string skillDescription;
    [Space]
    [SerializeField] UI_SkillSlot[] shouldBeUnlock;
    [SerializeField] UI_SkillSlot[] shouldBeLocked;
    [Space]
    [SerializeField] private Color lockedSkillColor;
    [Space]
    [SerializeField] private int skillPrice;

    private void OnValidate()
    {
        gameObject.name = "Skill = " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => TryUnlock());
    }

    private void Start()
    {
        skillIcon = GetComponent<Image>();

        if(isUnlocked)
        {
            skillIcon.color = Color.white;
        }
        else
        {
            skillIcon.color = lockedSkillColor;
        }
    }

    private bool TryUnlock()
    {
        if(isUnlocked)
        {
            return true;
        }

        foreach(var skill in shouldBeUnlock)
        {
            if(!skill.isUnlocked)
            {
                Debug.Log(skill.gameObject.name + " should be Unlock");
                return false;
            }
        }

        foreach (var skill in shouldBeLocked)
        {
            if (skill.isUnlocked)
            {
                Debug.Log(skill.gameObject.name + " should be lock");
                return false;
            }
        }

        if(PlayerManager.instance.TrySpendMoney(skillPrice))
        {
            isUnlocked = true;
            skillIcon.color = Color.white;
            return true;
        }
        return false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.instance.skillToolTip.Setup(skillDescription, skillName, skillIcon.sprite);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI.instance.skillToolTip.Hide();
    }

    public void LoadData(GameData _data)
    {
        string skillId = skillName;
        if (_data.skillTree.TryGetValue(skillId, out bool value))
        {
            isUnlocked = value;
        }
        else
        {
            isUnlocked = false;
        }

        if(isUnlocked)
        {
            if(GetComponent<Button>() != null)
            {
                GetComponent<Button>().onClick.Invoke();
            }
            else
            {
                Debug.Log("NULL");
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        string skillId = skillName;
        if (skillId != "")
        {
            if(_data.skillTree.ContainsKey(skillId))
            {
                _data.skillTree.Remove(skillId);
            }
            _data.skillTree.Add(skillId, isUnlocked);
        }
    }
}
