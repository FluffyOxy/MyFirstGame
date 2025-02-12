using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillLearningBlock : MonoBehaviour
{
    [SerializeField] private RectTransform choiceSlotsParent;
    [SerializeField] private RectTransform skillParent;
    [SerializeField] private int maxChoiceAmount;
    [SerializeField] private GameObject choiceSlotPrefab;

    [Header("Test")]
    [SerializeField] private List<UI_SkillSlot> skills = new List<UI_SkillSlot>();

    public bool TrySetup()
    {
        UI.instance.ActivateAllUI();

        UI_SkillSlot[] skillsTemp = skillParent.GetComponentsInChildren<UI_SkillSlot>();
        foreach(var skill in skillsTemp)
        {
            if(skill.CanUnlock())
            {
                skills.Add(skill);
            }
        }

        List<GameObject> randomSkills = new List<GameObject>();
        for (int i = 0; i < maxChoiceAmount; i++)
        {
            if (randomSkills.Count >= maxChoiceAmount || skills.Count <= 0)
            {
                break;
            }

            int randomIndex = Random.Range(0, skills.Count);
            GameObject newRandomSkill = skills[randomIndex].gameObject;
            skills[randomIndex] = skills[skills.Count - 1];
            skills.RemoveAt(skills.Count - 1);
            randomSkills.Add(newRandomSkill);
        }
        UI.instance.HideAllUI();

        if(randomSkills.Count <= 0)
        {
            skills.Clear();
            return false;
        }

        for (int i = 0; i < randomSkills.Count; ++i)
        {
            GameObject newSlot = Instantiate(choiceSlotPrefab, choiceSlotsParent);
            GameObject newSkillChoice = Instantiate(randomSkills[i], newSlot.GetComponent<RectTransform>());
            newSkillChoice.GetComponent<RectTransform>().localPosition = Vector3.zero;
            newSkillChoice.GetComponent<UI_SkillSlot>().parentSlot = randomSkills[i].GetComponent<UI_SkillSlot>();
            newSkillChoice.GetComponent<UI_SkillSlot>().canbeUnlocked = true;
        }

        skills.Clear();

        return true;
    }

    public void skillChooseFinish()
    {
        RectTransform[] choices = choiceSlotsParent.GetComponentsInChildren<RectTransform>();
        foreach (var choice in choices)
        {
            if(choice != choiceSlotsParent)
            {
                Destroy(choice.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
