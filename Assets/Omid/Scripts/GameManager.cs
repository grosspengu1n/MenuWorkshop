using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.MaterialProperty;

public class GameManager : MonoBehaviour
{
    public bool isStore;
    public GameObject interactive;

    public GameObject blackPanel;
    public Text dayText;
    public float fadeDuration = 1f;
    private CanvasGroup canvasGroup;
    private int currentDay = 1;
    private bool isFading = false;

    public GameObject[] items;

    private GameObject player;
    private GameObject currentItem;

    public static int coins;
    public static int debt;

    public static bool sleep;
    public static int itId;
    public static int itPrice;
    private void Start()
    {
        coins = 200;
        player = GameObject.Find("Player");
        blackPanel.SetActive(true);
        canvasGroup = blackPanel.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;

    }


    private void Update()
    {
        if (PlayerController.canTalk || sleep || itId!=0)
        {
            interactive.SetActive(true);
        }
        else
        {
            interactive.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && itId != 0)
        {
            if (itPrice<=coins)
            {
                Debug.Log("works");
                BuyItem(itId);
                coins -= itPrice;
            }

        }

        if (Input.GetKeyDown(KeyCode.E) && sleep && !isFading)
        {
            StartCoroutine(ChangeDay());
        }

    }

    // Метод для покупки предмета
    public void BuyItem(int itemId)
    {
        if (currentItem != null)
        {
            Debug.Log("Уже есть предмет над головой игрока.");
            return;
        }

        foreach (GameObject item in items)
        {
            ItemProperties properties = item.GetComponent<ItemProperties>();
            if (properties.itemId == itemId)
            {
                currentItem = Instantiate(item, player.transform.position + Vector3.up, Quaternion.identity);
                currentItem.transform.parent = player.transform;
                break;
            }
        }
    }

    // Метод для сохранения предмета между сценами
    public void SaveCurrentItem()
    {
        if (currentItem != null)
        {
            DontDestroyOnLoad(currentItem);
        }
    }



    private IEnumerator ChangeDay()
    {
        isFading = true;

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
            Debug.Log(Time.deltaTime / fadeDuration);
        }


        yield return new WaitForSeconds(1f);

        currentDay++;
        dayText.text = "Day " + currentDay;

        yield return new WaitForSeconds(1f);


        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
            Debug.Log(Time.deltaTime / fadeDuration);
        }

        isFading = false;
    }
}
