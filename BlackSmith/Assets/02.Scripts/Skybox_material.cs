using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skybox_material : MonoBehaviour
{
    [SerializeField] Transform light_Transform;
    public Material[] skyMaterial;
    public int i;
    Skybox skybox;
    public float timer;
    public string currentDay;

    [SerializeField] GameObject BGM;
    private void Start()
    {
        skybox = GetComponent<Skybox>();
        currentDay = "Night";
    }
    // Update is called once per frame
    void Update()
    {        
        if (!GameManager.instance.nextDate)
        {
            timer += Time.deltaTime;
            if (timer < 120 && currentDay == "Night")//�ð��� ���� ��� �ϴú���
            {
                ChangeSkyMaterial(0, new Vector3(50, -30, 0));
                currentDay = "Day";
                BGM.SetActive(false);
                BGM.SetActive(true);
                Debug.Log(0);
            }
            else if (timer > 120 && currentDay == "Day")
            {
                ChangeSkyMaterial(1, new Vector3(0, -30, 0));
                currentDay = "Night";
                BGM.SetActive(false);
                BGM.SetActive(true);
                Debug.Log(1);

            }

        }

    }
    void ChangeSkyMaterial(int i , Vector3 lightAngle)
    {
        skybox.material = skyMaterial[i];
        light_Transform.eulerAngles = lightAngle;
    }
}
