using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StagePreview : MonoBehaviour
{
    [SerializeField] private GameObject previewImage;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private TextMeshProUGUI levelText;
    private int level;


    public void SetStagePreview(int levelNumber, bool lockState)
    {
        previewImage.SetActive(lockState);
        lockIcon.SetActive(!lockState);
        levelText.text = $"CHAPTER {levelNumber + 1}";
        level = levelNumber;
    }
    
    
}