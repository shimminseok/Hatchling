using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDragPr : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] protected Image _itemIcon;
    [SerializeField] protected Sprite _nullImage;

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
            _itemIcon.gameObject.SetActive(false);
            _originpos = transform.position;
            UpdateDragginObjectPos(eventData);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        ItemDropPr drop = eventData.pointerEnter.gameObject.GetComponentInParent<ItemDropPr>();
        if (drop == null)
        {
            _itemIcon.gameObject.SetActive(true);
        }
        else
        {
            _itemIcon.gameObject.SetActive(true);
            _itemIcon.sprite = drop._slotImage;
        }
        Destroy(_dragginObject);
    }
}

