using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] Image _itemIcon;
    [SerializeField] Sprite _nullImage;
    [SerializeField] Text _amountTxt;

    ItemTooltipUI _tooltipUI;

    public int _index { get; private set; }
    public void SetSlotIndex(int index) => _index = index;
    //public int _key;

    DefineEnumHelper.ItemType _itemType;
    DefineEnumHelper.UsedItemKind _usedItemKind;

    public struct SlotData
    {
        public int _key;
        public bool _hasItem;
        public int _amount;
        public DataTableSt.stItemData _itemData;
    }
    public int _maxAmount = 10;
    public int _minAmount = 1;

    public SlotData _slotData = new SlotData();
    private void Awake()
    {
        _tooltipUI = transform.parent.parent.GetChild(1).GetComponent<ItemTooltipUI>();
        HideText();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_slotData._key != 0)
        {
            _itemIcon.color = Color.yellow;
            _tooltipUI.Show();
            _tooltipUI.SetRectPosition(_itemIcon.rectTransform);
            _tooltipUI.SetItemInfo(_slotData._key);
            _itemType = _tooltipUI.ItemType(_slotData._key);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _itemIcon.color = Color.white;
        _tooltipUI.Hide();
    }
    //드래그 관련
    #region[.]
    [SerializeField] Vector2 _dragOffset = Vector2.zero;
    GameObject _dragginObject;
    RectTransform _canvasTf;
    Vector3 _originpos;


    void UpdateDragginObjectPos(PointerEventData eventData)
    {
        if (_itemIcon.sprite != _nullImage)
        {
            if (_dragginObject != null)
            {
                //드래그중 아이콘의 화면좌표계산
                Vector3 screenPos = eventData.position + _dragOffset;

                Vector3 newPos = Vector3.zero;
                Camera cam = eventData.pressEventCamera;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvasTf, screenPos, cam, out newPos))
                {
                    _dragginObject.transform.position = newPos;
                    _dragginObject.transform.rotation = _canvasTf.rotation;
                }
            }

        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_dragginObject != null)
        {
            Destroy(_dragginObject);
        }
        if (_itemIcon.sprite != _nullImage)
        {
            //드래그 중인 아이콘 생성
            _dragginObject = new GameObject("Dragging Object");

            // 드래그 아이콘을 해당 캔버스의 종속후 가장 마지막으로 옮겨서 가장 최상위에 그려지도록 한다.
            _dragginObject.transform.SetParent(_itemIcon.canvas.transform);
            _dragginObject.transform.SetAsLastSibling();
            _dragginObject.transform.localScale = Vector3.one;

            //블록 레이캐스트 속성을 블록되지 않게 한다.
            CanvasGroup canvasGrp = _dragginObject.AddComponent<CanvasGroup>();
            canvasGrp.blocksRaycasts = false;
            Image dragIcon = _dragginObject.AddComponent<Image>();
            dragIcon.sprite = _itemIcon.sprite;
            dragIcon.rectTransform.sizeDelta = _itemIcon.rectTransform.sizeDelta;
            dragIcon.color = _itemIcon.color;
            dragIcon.material = _itemIcon.material;

            _canvasTf = dragIcon.canvas.transform as RectTransform;
            UpdateDragginObjectPos(eventData);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        //드래그를 시작하면 해당 이미지를 끔
        if (_itemIcon.sprite != _nullImage)
        {
            HideText();
            _originpos = transform.position;
            UpdateDragginObjectPos(eventData);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        ItemDropPr drop = eventData.pointerEnter.gameObject.GetComponentInParent<ItemDropPr>();
        ItemSlot slot = eventData.pointerEnter.gameObject.GetComponentInParent<ItemSlot>();
        if (slot == null)
        {
            Destroy(_dragginObject);
            switch ((DefineEnumHelper.ItemType)_slotData._itemData._type)
            {
                case DefineEnumHelper.ItemType.사용아이템:
                    RemoveItem(-1);
                    break;
                case DefineEnumHelper.ItemType.장착아이템:
                    RemoveItem(-1);
                    break;
            }
            return;
        }
        else
        {
            _itemIcon.gameObject.SetActive(true);
            _itemIcon.sprite = drop._slotImage;
            SwapItem(slot);
            Destroy(_dragginObject);
        }

    }
    void SwapItem(ItemSlot slot)
    {
        if (slot._slotData._itemData._type.Equals((int)DefineEnumHelper.ItemType.사용아이템) && _slotData._itemData._type.Equals((int)DefineEnumHelper.ItemType.사용아이템)
            && slot._slotData._itemData._item_kind.Equals(_slotData._itemData._item_kind) && slot._slotData._amount + _slotData._amount < _maxAmount)
        {
            slot._slotData._amount += _slotData._amount;
            slot.SetItemAmount(slot._slotData._amount);
            RemoveItem(-1);
        }
        else
        {
            SlotData temp = slot._slotData;
            slot._slotData = _slotData;
            _slotData = temp;
            slot.SetItemAmount(slot._slotData._amount);
            SetItemAmount(_slotData._amount);
        }
    }
    #endregion
    public void SetItemAmount(int amount)
    {
        _slotData._amount = amount;
        if (_slotData._key != 0 && amount > 1)
            ShowText();
        else
            HideText();

        _amountTxt.text = _slotData._amount.ToString();
    }
    void ShowText() => _amountTxt.gameObject.SetActive(true);
    void HideText() => _amountTxt.gameObject.SetActive(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button.Equals(PointerEventData.InputButton.Left))
        {
            return;
        }
        UsedItem();
    }

    void UsedItem()
    {
        CharacterCtrl character = GameManager._instance.character;
        EquipMentWindow mount = UIManager._instance.equipMentWindow;
        switch ((DefineEnumHelper.ItemType)_slotData._itemData._type)
        {
            case DefineEnumHelper.ItemType.사용아이템:
                _slotData._amount += character.UseItem(_slotData._key);
                RemoveItem(_slotData._amount);
                SetItemAmount(_slotData._amount);
                break;
            case DefineEnumHelper.ItemType.장착아이템:
                mount.MountItem(_slotData._key, this);
                break;
        }
    }
    public void RemoveItem(int amount)
    {
        if (amount <= 0)
        {
            _slotData = new SlotData();
            _itemIcon.sprite = _nullImage;
        }
    }
}
