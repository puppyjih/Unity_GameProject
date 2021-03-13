using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics;

public class Joystick : MonoBehaviour
{
    private Image bgImg;
    private Image joystickImg;
    private Vector3 inputVector;
    private bool isFixed = false;
    private Vector2 touchPosition = new Vector2(224, 253);
    private Text txt;
    private PointerEventData tmp;
    private int touchNumber;

   

    private void Start()
    {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
        txt = GameObject.Find("Text").GetComponent<Text>();
        
    }

    public void batchJoystick(Vector2 touch, int i)
    {
        //Debug.Log("Joystick >>> OnDrag()");
        //txt.text = "Joystick >>> OnDrag()";
        touchNumber = i;
        //ped.position = touchPosition;

        //transform.position = Input.mousePosition - Vector3.right * bgImg.rectTransform.sizeDelta.x / 2 + Vector3.down * bgImg.rectTransform.sizeDelta.y / 2;
        //startTouch = Input.mousePosition;
        if (Vector2.Distance(touch, transform.position) > (bgImg.rectTransform.sizeDelta.x * 0.5f) * 2f)
            transform.position = touch;
        //-Vector2.right * bgImg.rectTransfor.sizeDelta.x / 2 + Vector2.down * bgImg.rectTransform.sizeDelta.y / 2;
        //return touch = Input.mousePosition;
    }

    public void pushJoystick(Vector2 touch)
    {
        //txt.text = "Joystick >>> OnPointerDown()";

        //tmp.position = touchPosition;
        Vector2 pos = new Vector2(touch.x - transform.position.x, touch.y - transform.position.y);
        pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
        pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);
        float x = pos.x * 2;
        float y = pos.y * 2;
        //float x = (bgImg.rectTransform.pivot.x == 1.0f) ? pos.x * 2 : pos.x * 2;
        //float y = (bgImg.rectTransform.pivot.y == 1.0f) ? pos.y * 2 : pos.y * 2;
        inputVector = new Vector3(x, y, 0);
        inputVector = (inputVector.magnitude > 1) ? inputVector.normalized : inputVector;
        joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3), inputVector.y * (bgImg.rectTransform.sizeDelta.y / 3));

        
    }

    public void releaseJoystick(Vector2 touch)
    {
        //txt.text = "Joystick >>> OnPointerUp()";
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
        //transform.position = defaultPos;
    }

    public float GetHorizontalValue()
    {
        return inputVector.x;
    }

    public float GetVerticalValue()
    {
        return inputVector.y;
    }

    public int GetTouchNumber()
    {
        return touchNumber;
    }
}