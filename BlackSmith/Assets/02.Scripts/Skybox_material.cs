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
            if (timer < 120 && currentDay == "Night")//시간에 따라 밝기 하늘변경
            {
                i = 0;
                currentDay = "Day";
                BGM.SetActive(false);
                BGM.SetActive(true);
            }
            else if (timer > 120 && currentDay == "Day")
            {
                i = 1;
                currentDay = "Night";
                BGM.SetActive(false);
                BGM.SetActive(true);
            }
            switch (i)
            {
                case 0:
                    skybox.material = skyMaterial[i];
                    light_Transform.eulerAngles = new Vector3(50, -30, 0);
                    break;
                case 1:
                    skybox.material = skyMaterial[i];
                    light_Transform.eulerAngles = new Vector3(0, -30, 0);
                    break;
            }

        }

    }
}
