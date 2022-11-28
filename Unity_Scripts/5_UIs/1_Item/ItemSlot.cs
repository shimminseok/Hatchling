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
    public int _key;

    DefineEnumHelper.ItemType _itemType;
    DefineEnumHelper.UsedItemKind _usedItemKind;

    public bool _hasItem = false;
    public DataTableSt.stItemData _itemData;
    public int _amount = 0;
    public int _maxAmount = 10;
    public int _minAmount = 1;

    private void Awake()
    {
        _tooltipUI = transform.parent.parent.GetChild(1).GetComponent<ItemTooltipUI>();
        HideText();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_key != 0)
        {
            _itemIcon.color = Color.yellow;
            _tooltipUI.Show();
            _tooltipUI.SetRectPosition(_itemIcon.rectTransform);
            _tooltipUI.SetItemInfo(_key);
            _itemType = _tooltipUI.ItemType(_key);
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
        if(slot == null)
        {
            Destroy(_dragginObject);
            switch((DefineEnumHelper.ItemType)_itemData._type)
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
        bool tempHasItem = slot._hasItem;
        slot._hasItem = _hasItem;
        _hasItem = tempHasItem;
        //키
        int tempKey = slot._key;
        slot._key = _key;
        _key = tempKey;
        //아이템 데이터
        DataTableSt.stItemData temp = slot._itemData;
        slot._itemData = _itemData;
        _itemData = temp;
        //수량
        int tempAmount = slot._amount;
        slot._amount = _amount;
        _amount = tempAmount;

        //아이템 갯수 출력
        slot.SetItemAmount(slot._amount);
        SetItemAmount(_amount);
    }
    #endregion
    public void SetItemAmount(int amount)
    {
        _amount = amount;
        if (_key != 0 && amount > 1)
            ShowText();
        else
            HideText();

        _amountTxt.text = _amount.ToString();
    }
    void ShowText() => _amountTxt.gameObject.SetActive(true);
    void HideText() => _amountTxt.gameObject.SetActive(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button.Equals(PointerEventData.InputButton.Left))
        {
            return;
        }
        UsedItem();
    }

    void UsedItem()
    {
        CharacterCtrl character = GameManager._instance.character;
        EquipMentWindow mount = UIManager._instance.equipMentWindow;
        switch ((DefineEnumHelper.ItemType)_itemData._type)
        {
            case DefineEnumHelper.ItemType.사용아이템:
                _amount += character.UseItem(_key);
                RemoveItem(_amount);
                SetItemAmount(_amount);
                break;
            case DefineEnumHelper.ItemType.장착아이템:
                mount.MountItem(_key,this);
                break;
        }
    }

    public void RemoveItem(int amount)
    {
        if(amount <= 0)
        {
            _amount = 0;
            _itemData = new DataTableSt.stItemData();
            _hasItem = false;
            _itemIcon.sprite = _nullImage;
            _key = 0;
        }

    }
}
