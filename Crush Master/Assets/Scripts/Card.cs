using UnityEngine;
using UnityEngine.UI;

// Card.cs
public class Card : MonoBehaviour
{
    [SerializeField] private Image frontImage;
    [SerializeField] private Image backImage;
    [SerializeField] private Button button;

    private int _id;
    private bool _isFlipped;

    public int ID => _id;
    public bool IsFlipped => _isFlipped;

    public void Initialize(int id, Sprite symbol)
    {
        _id = id;
        frontImage.sprite = symbol;
        button.onClick.AddListener(OnClick);
    }

    public void Flip(bool showFront)
    {
        _isFlipped = showFront;
        frontImage.gameObject.SetActive(showFront);
        backImage.gameObject.SetActive(!showFront);
    }

    private void OnClick()
    {
        if (!_isFlipped)
            GameManager.Instance.OnCardClicked(this);
    }
}
