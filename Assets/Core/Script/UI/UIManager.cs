using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager UIManagerSingle;

    [Header("Refrences")]
    [SerializeField] TMP_Text Score_Text;
    [SerializeField] private RectTransform popup;
    [SerializeField] private GameObject Transation;


    private void Start()
    {
        GameManager.GameInstance.UpdateScore += SetScoreText;
    }
    private void OnDestroy()
    {
        GameManager.GameInstance.UpdateScore -= SetScoreText;

    }


    public void Restart()
    {
        GameObject test = Instantiate(Transation);

        popup.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                popup.gameObject.SetActive(false);

                test.GetComponent<SceneTransitioner>().LoadScene("Main");
            });
    }
    public void ShowPopup()
    {
        popup.transform.gameObject.SetActive(true);
        popup.localScale = Vector3.zero; 

        popup.DOScale(Vector3.one, 0.5f)
             .SetEase(Ease.OutBack);
    }
    private void Awake()
    {
        if (UIManagerSingle != null  && UIManagerSingle != this )
        {
            Destroy(this.gameObject); // Hancurkan duplikat
            return;
        }

        UIManagerSingle = this;
    }

    public void SetScoreText(float Value)
    {
        Score_Text.text = Value.ToString();
    }
   
   
}
