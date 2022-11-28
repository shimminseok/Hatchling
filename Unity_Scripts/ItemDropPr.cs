using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ItemDropPr : MonoBehaviour, IDropHandler 
{
    [Header("외부 참조")]
    [SerializeField] Image _slotBg;
    [SerializeField] Image _slotIcon;
    [Header("Parameta")]
    [SerializeField] Color _enterColor = Color.yellow;

    Color _originColor;
    Sprite _originImage;
    public Sprite _slotImage
    {
        get { return _originImage; }
    }
    void Start()
    {
        _originColor = _slotBg.color;
    }

    public void OnDrop(PointerEventData eventData)
    {
        _originImage = _slotIcon.sprite;
        transform.GetChild(0).gameObject.SetActive(true);
        Image dropImage = eventData.pointerDrag.transform.GetChild(0).GetComponent<Image>();
        _slotIcon.sprite = dropImage.sprite;
        _slotIcon.color = Color.white;
        _slotBg.color = _originColor;
    }
}
