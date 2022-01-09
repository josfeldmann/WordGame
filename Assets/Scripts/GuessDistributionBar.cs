using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuessDistributionBar : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI distributionText;
    public float minFill = 0.08f;

    public void Set(float distributionAmount, float distributionPercentage) {
        fillImage.fillAmount = Mathf.Max(distributionPercentage, minFill);
        distributionText.SetText(distributionAmount.ToString());
        if (distributionAmount == 0) {
            fillImage.color = WordColors.instance.GREY;
        } else {
            fillImage.color = WordColors.instance.GREEN;
        }
    }
}
