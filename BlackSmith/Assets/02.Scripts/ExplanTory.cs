using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplanTory : MonoBehaviour
{
    public Scrollbar horiScrollbar;
    int explanToryNum;
    
    //버튼을 누르면 옆으로 이동하면서 사진이변경된다
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (explanToryNum < 0)
            explanToryNum = 4;
        else if (explanToryNum > 4)
            explanToryNum = 0;
        switch (explanToryNum)
        {
            case 0:
                horiScrollbar.value = 0;
                break;
            case 1:
                horiScrollbar.value = 0.25f;
                break;
            case 2:
                horiScrollbar.value = 0.5f;
                break;
            case 3:
                horiScrollbar.value = 0.75f;
                break;
            case 4:
                horiScrollbar.value = 1.0f;
                break;
        }
    }
    public void PanelChange(int i)
    {
        explanToryNum += i;
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
