using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour
{
    Button btn;   
    [SerializeField] SoundEventType btnSoundEventType = SoundEventType.UI_ButtonClick;


    void Awake()
    {
        btn= GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        SoundManager.Instance.Invoke(transform, btnSoundEventType); //버튼 소리재생.
    }


}
