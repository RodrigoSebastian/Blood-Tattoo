using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurseBar : MonoBehaviour
{
    private Image curseImage;
    private Curse curse;
    private void Awake()
    {
        curseImage = transform.Find("CurseAmount").GetComponent<Image>();
        curse = new Curse();
        Time.timeScale = 0;
    }

    private void Update()
    {
        curse.Update();
        curseImage.fillAmount = curse.GetCurseNormalized();
    }
}

public class Curse
{
    public const int CURSE_MAX = 100;
    private float curseAmount;
    private float curseRegAmount;

    public Curse()
    {
        curseAmount = 0;
        curseRegAmount = 1f;
    }

    public void Update()
    {
        curseAmount += curseRegAmount * Time.deltaTime;
    }

    public float GetCurseNormalized()
    {
        return curseAmount / CURSE_MAX;
    }
}
