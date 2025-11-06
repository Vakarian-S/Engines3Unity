using Cainos.PixelArtTopDown_Basic;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameHUDController : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private UIDocument uiDocument;

    [SerializeField] private Sprite bombIconSprite;

    [Header("Data Source")] [SerializeField]
    private TopDownCharacterController mainPlayerCharacter;
    // BombInventory is any script of yours that knows current bomb count and raises an event.

    private Label bombCountLabel;
    private Image bombIconImage;

    private void Awake()
    {
        if (uiDocument == null)
        {
            uiDocument = GetComponent<UIDocument>();
        }
    }

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;
        bombCountLabel = root.Q<Label>("BombCountLabel");
        bombIconImage = root.Q<Image>("BombIcon");
        bombCountLabel.text = "Initail Text";
        if (bombIconImage != null && bombIconSprite != null)
        {
#if UNITY_2022_3_OR_NEWER
            bombIconImage.sprite = bombIconSprite;
#else
            // Fallback for older UI Toolkit versions:
            bombIconImage.style.backgroundImage = new StyleBackground(bombIconSprite);
#endif
        }

        UpdateBombCount(mainPlayerCharacter != null ? mainPlayerCharacter.numberOfBombs : 0);

        if (mainPlayerCharacter != null)
        {
            mainPlayerCharacter.OnBombCountChanged += HandleBombCountChanged;
        }
    }

    private void OnDisable()
    {
        if (mainPlayerCharacter != null)
        {
            mainPlayerCharacter.OnBombCountChanged -= HandleBombCountChanged;
        }
    }

    private void HandleBombCountChanged(int newCount)
    {
        UpdateBombCount(newCount);
    }

    private void UpdateBombCount(int newCount)
    {
        if (bombCountLabel != null)
        {
            bombCountLabel.text = newCount.ToString();
        }
    }
}