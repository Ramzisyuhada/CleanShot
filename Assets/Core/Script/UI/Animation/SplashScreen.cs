using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{


    [Header("Refrences")]
    [SerializeField] private Image Background;

    [SerializeField] private TMP_Text Text;



    [Header("Setting")]
    [SerializeField] private float DurationFadeOutText;

    [SerializeField] private float DurationFadeInText;

    [SerializeField] private float DurationColorBackground;



    void Start()
    {

        Sequence seq = DOTween.Sequence();

        seq.Append(Text.DOFade(1f, DurationFadeInText))
            .AppendInterval(3f);

        seq.Append(Text.DOFade(0f, DurationFadeInText));
        seq.Append(Background.DOFade(0f, DurationColorBackground)).OnComplete(() =>
            Background.transform.localScale = Vector3.zero
        );
        
            
            
     

    }
}
