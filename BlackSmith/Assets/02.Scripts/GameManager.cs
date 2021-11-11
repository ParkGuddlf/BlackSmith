using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    Player player;
    ScrollATM atm;
    Skybox_material skyMat;
    public bool nextDate = false;
    public bool startGame;//시작버튼 불리언
    [SerializeField] GameObject titleAndBut;
    public GameObject fadeout;
    [Header("invenSys---------")]
    [SerializeField] GameObject bag;
    public bool isBagOn;
    public Text moneyText;
    public int money;
    public GameObject explanePanel;
    public RectTransform canvasRt;
    RectTransform explanePanelRt;

    IEnumerator pointerCo;

    public GameObject explantory;
    
  
    void Start()
    {
       
        explanePanelRt = explanePanel.GetComponent<RectTransform>();
        player = FindObjectOfType<Player>();
        atm = FindObjectOfType<ScrollATM>();
        skyMat = FindObjectOfType<Skybox_material>();

    }

    void Update()
    {

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRt, Input.mousePosition, Camera.main, out Vector2 pos);
        explanePanelRt.anchoredPosition = pos - new Vector2(-100, 150);
        Cam();        
        Inventory();
        Money();
        if (skyMat.timer >= 240)
        {
            skyMat.timer = 0;
            nextDate = true;
            StartCoroutine(DateOut()); 
            print("1");
        }

        /* else if (skyMat.timer <= 0.01f && nextDate)
         {
             StartCoroutine(DateIN());
             print("2");
         }*/
    }

    void Inventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isBagOn = !isBagOn;
            bag.SetActive(isBagOn);    //가방여닫기
        }
    }
    private void Cam()
    {
        titleAndBut.SetActive(!startGame);
        if (startGame)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, player.transform.position + new Vector3(0, 3, 8), 2 * Time.deltaTime);
        }
    }
    void Money()
    {
        moneyText.text = money.ToString();
    }

    IEnumerator DateOut()
    {   //페이드 아웃되면서 상자안의 물건이 팔리고 하루가 지나가는 느낌 닭울음소리
        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeout.GetComponent<Image>().color = new Color(0, 0, 0, fadeCount);
        }
        yield return new WaitForSeconds(2.0f);
        player.transform.position = new Vector3(-6, -6, 0);
        yield return new WaitForSeconds(2.0f);
        while (fadeCount > 0)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeout.GetComponent<Image>().color = new Color(0, 0, 0, fadeCount);
        }
        yield return new WaitForSeconds(2.0f);
        //의뢰개수를 받을수있는상태로 바꾸고 소환개수를 0로바꾼다.
        atm.setScroll = true;
        atm.spawnNum = 0;
        //상자물품판매
        Chest sell_chest = FindObjectOfType<Chest>();
        sell_chest.SellBut();
        
        
        nextDate = false;
    }
    /*IEnumerator DateIN()//페이드인
    {        
        float fadeCount = 1.0f;
        while (fadeCount > 0)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeout.GetComponent<Image>().color = new Color(0, 0, 0, fadeCount);
        }
        
    }*/
    public void StartGame()
    {
        startGame = true;
    }
    public void PointEnter(int slotNum)//설명문 텍스터변경
    {
        pointerCo = PointEnterDelay(slotNum);
        StartCoroutine(pointerCo);
        if (player.inventory.transform.GetChild(slotNum).GetComponent<Bag>().item != null)
        {
            explanePanel.transform.GetChild(1).GetComponent<Text>().text = player.inventory.transform.GetChild(slotNum).GetComponent<Bag>().item.itemName;
            if (player.inventory.transform.GetChild(slotNum).GetComponent<Bag>().item.prefab.GetComponent<Scroll>() != null)
            {
                explanePanel.transform.GetChild(2).GetComponent<Text>().text = "combination" + "\n";
                for (int i = 0; i < player.inventory.transform.GetChild(slotNum).GetComponent<Bag>().item.prefab.GetComponent<Scroll>().itemRicipe.Length; i++)
                {
                    explanePanel.transform.GetChild(2).GetComponent<Text>().text += player.inventory.transform.GetChild(slotNum).GetComponent<Bag>().item.prefab.GetComponent<Scroll>().itemRicipe[i] + "\n";
                }
            }
            else
                explanePanel.transform.GetChild(2).GetComponent<Text>().text = "";
        }
        else
        {
            explanePanel.transform.GetChild(1).GetComponent<Text>().text = "";
            explanePanel.transform.GetChild(2).GetComponent<Text>().text = "";
        }
    }
    IEnumerator PointEnterDelay(int slotNum)//마우스가까이가면 설명창띄운다
    {
        yield return new WaitForSeconds(0.5f);
        explanePanel.SetActive(true);
    }
    public void PointExit(int slotNum)//마우스가까이가면 설명창끈다
    {
        StopCoroutine(pointerCo);
        explanePanel.SetActive(false);
    }
    public void Explantory()
    {
        explantory.SetActive(true);
    }
}
