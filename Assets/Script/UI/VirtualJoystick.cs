using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    private GameObject[] skillButton;
    public static VirtualJoystick instance;

    private Image bgImg;
    private Image joystickImg;
    private Vector3 InputVector;

    void Start()
    {
        instance = this;

        bgImg       = GetComponent<Image>();
        joystickImg  = transform.FindChild("JoystickImage").GetComponent<Image>();
        InputVector  = Vector3.zero;
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos = Vector2.zero;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            float x = (bgImg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
            float y = (bgImg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

            InputVector = new Vector3(x, 0, y);
            InputVector = (InputVector.magnitude > 1.0f) ? InputVector.normalized : InputVector;

            //InputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);

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

    public void SkillButtonActive(int index)
    {
        skillButton[index].SetActive(true);
    }

    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ스킬 호출ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//
    public void Attack_Button()
    {
        AttackSkill.instance.Attack();
    }

    public void HealSkill()
    {
        SoundManager.Instance.PlayEffect("Heal");
        Heal.instance.Buff();
    }

    public void BranDish()
    {
        SoundManager.Instance.PlayEffect("Brandish");
        Brandish.instance.Attack();
    }

    public void HastSkill_Button()
    {
        HasteBuff.instance.Buff();
    }

    public void SwordBooster()
    {
        SoundManager.Instance.PlayEffect("RaisingSword");
        RaisingSword.instance.Buff();
    }
}