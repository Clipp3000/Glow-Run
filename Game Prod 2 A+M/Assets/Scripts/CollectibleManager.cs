using TMPro;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public TextMeshProUGUI CollectibleText;

    private int MedicationScore = 0;

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int score = 1)
    {
        MedicationScore += score;
        UpdateUI();
    }

    void UpdateUI()
    {
        CollectibleText.text = "Medication: " + MedicationScore;
    }
}
