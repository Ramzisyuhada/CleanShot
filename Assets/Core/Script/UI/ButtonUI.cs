using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    private Button button; 
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(AnimatedOnClick);
    }

    private void AnimatedOnClick()
    {
        button.transform.DOScale(1.5f, 0.1f).OnComplete(() => transform.DOScale(1f,0.1f));
    }

}
