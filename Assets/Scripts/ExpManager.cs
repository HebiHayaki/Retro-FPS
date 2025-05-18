using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpManager : MonoBehaviour
{
    [Header("Exp")]
    [SerializeField] AnimationCurve expCurve;

    public int currentLevel, totalExp;
    int previousLevelsExp, nextLevelsExp;

    [Header("Interface")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image expFill;

    void Start()
    {
        UpdateLevel();
    }

    void Update()
    {
        
    }

    public void AddExp(int amount)
    {
        totalExp += amount;
        CheckForLevelUp();
        UpdateInterface();
    }

    void CheckForLevelUp()
    {
        if (totalExp >= nextLevelsExp)
        {
            currentLevel++;
            UpdateLevel();

            UpgradeManager.instance.ShowUpgradeOptions();
        }
    }

    void UpdateLevel()
    {
        previousLevelsExp = (int)expCurve.Evaluate(currentLevel);
        nextLevelsExp = (int)expCurve.Evaluate(currentLevel + 1);
        UpdateInterface();
    }

    void UpdateInterface()
    {
        int start = totalExp - previousLevelsExp;
        int end = nextLevelsExp - previousLevelsExp;

        levelText.text = currentLevel.ToString();
        expFill.fillAmount = (float)start / (float)end;
    }
}
