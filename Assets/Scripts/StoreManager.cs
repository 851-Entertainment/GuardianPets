using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Store;

public class StoreManager : MonoBehaviour 
{
    /// <summary>Panel for the upgrades</summary>
    public GameObject m_StorePanel;

    /// <summary>Button prefab for the items sold in the store</summary>
    public GameObject m_GoodsButtonPrefab;

    /// <summary>Shield sprite for the store panel</summary>
    public Sprite m_ShieldSprite;

    /// <summary>Game Controller script</summary>
    private GameController gc_;

    /// <summary>UI Controller script</summary>
    private UIController uc_;

	void Start () 
    {
        gc_ = Camera.main.GetComponent<GameController>();
        uc_ = Camera.main.GetComponent<UIController>();
	}

    public void PopulateStore()
    {
        float buttonWidth = m_GoodsButtonPrefab.GetComponent<RectTransform>().sizeDelta.x * 3;
        float buttonHeight = m_GoodsButtonPrefab.GetComponent<RectTransform>().sizeDelta.y * 1.25f;
        float startXPos = 0.0f - (buttonWidth * 1.25f);
        float startYPos = 0.0f + (buttonHeight * 1.25f);
        int row = 0;
        int col = 0;
        int maxCol = 3;
        int maxRow = 100;

        m_StorePanel.SetActive(!m_StorePanel.activeSelf);

        foreach (Item item in gc_.m_Items)
        {
            GameObject go = (GameObject)Instantiate(m_GoodsButtonPrefab, new Vector3(startXPos, startYPos, 0.0f), Quaternion.identity);
            go.gameObject.transform.SetParent(GameObject.Find(item.m_Category).transform, false);
            go.name = item.m_ItemName;
            go.GetComponentInChildren<Image>().sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
            go.GetComponentInChildren<Button>().onClick.AddListener(delegate { uc_.UnlockItem(go); });
            go.GetComponentInChildren<Text>().text = item.m_Description + " This costs " + item.m_Cost + " shields.";

            if (col < maxCol)
            {
                col++;
                startXPos += buttonWidth;
                if (col >= maxCol)
                {
                    if (row < maxRow)
                    {
                        col = 0;
                        startXPos = 0.0f - (buttonWidth * 1.25f);
                        row++;
                        startYPos -= buttonHeight;
                    }
                }
            }
        }

        foreach (VirtualCurrencyPack vcp in StoreInfo.CurrencyPacks)
        {
            string itemID = vcp.ItemId;
            GameObject go = (GameObject)Instantiate(m_GoodsButtonPrefab, new Vector3(startXPos, startYPos, 0.0f), Quaternion.identity);
            go.gameObject.transform.SetParent(GameObject.Find("Shields").transform, false);
            go.GetComponentInChildren<Image>().sprite = m_ShieldSprite;
            go.GetComponentInChildren<Button>().onClick.AddListener(delegate { StoreInventory.BuyItem(itemID); });
            go.GetComponentInChildren<Text>().text = vcp.Description;

            if (col < maxCol)
            {
                col++;
                startXPos += buttonWidth;
                if (col >= maxCol)
                {
                    if (row < maxRow)
                    {
                        col = 0;
                        startXPos = 0.0f - (buttonWidth * 1.25f);
                        row++;
                        startYPos -= buttonHeight;
                    }
                }
            }
        }
    }
}
