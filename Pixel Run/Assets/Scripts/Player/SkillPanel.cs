using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    private int skillIdx = 0;
    private List<SkillParent> skills;
    private PlayerStat player;
    [SerializeField] private List<Sprite> skillSprite;
    [SerializeField] private Image skillButtonImage;
    [SerializeField] private Text text;
    //[SerializeField] private SkillParent asdf;

    void Start()
    {
        skills = new List<SkillParent>();
        foreach (Transform c in transform)
        {
            skills.Add(c.gameObject.GetComponent<SkillParent>());
        }
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
        skillButtonImage.sprite = skillSprite[skillIdx];
        text.text = skills[skillIdx].GetCost().ToString();
        //skills.Add(transform.GetChild(0).GetComponent<SkillParent>());
        //skills.Add(asdf);
        //Debug.Log("ASIOAIFSHASLHAFLH" + skills);
    }

    private void Update() // FOR DEBUGGING
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Button_UseSkill();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            Button_ChangeSkill();
        }
    }

    public void Button_ChangeSkill() {
        skillIdx = (skillIdx + 1) % skills.Count;
        skillButtonImage.sprite = skillSprite[skillIdx];
        text.text = skills[skillIdx].GetCost().ToString();
    }

    public void Button_UseSkill() {
        int mana = 0;
        skills[skillIdx].UseSkill(out mana);
        player.PayMP(mana);
        //asdf.UseSkill(out mana);
        //return mana;
    }
}
