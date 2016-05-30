using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public VirtualJoystick instance;

    private Image bgImg;
    private Image joystickImg;
    private Vector3 InputVector;

    void Start()
    {
        instance = this;
        bgImg       = GetComponent<Image>();

        joystickImg  = transform.FindChild("JoystickImage").GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            InputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            InputVector = (InputVector.magnitude > 1.0f) ? InputVector.normalized : InputVector;

            joystickImg.rectTransform.anchoredPosition = new Vector3(InputVector.x * (bgImg.rectTransform.sizeDelta.x / 3),
                                                                     InputVector.z * (bgImg.rectTransform.sizeDelta.y / 3));
        }
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        InputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public float Horizontal()
    {
        if (InputVector.x != 0)
            return InputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        if (InputVector.z != 0)
            return InputVector.z;
        else
            return Input.GetAxis("Vertical");
    }

    public void Attack_Button()
    {
        AttackSkill.instance.Attack();
    }

    public void HastSkill_Button()
    {
        if (HasteBuff.instance.buffState != false)
        {
            HasteBuff.instance.buffState = false;
            //모션
            //이펙트
            //스킬 호출
            HasteBuff.instance.Buff();
            Debug.Log(" 스킬 사용 ");
        }
        Debug.Log("헤이스트 버튼 호출");
        Debug.Log("ture/false : " + HasteBuff.instance.buffState);
    }
}