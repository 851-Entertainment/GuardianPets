using UnityEngine;
using System.Collections;
using Soomla.Store;

public class Achievement : MonoBehaviour 
{
    public string m_AchName;
    public int m_RewardValue;
    public int m_Progress;
    public int m_AmtNeeded;
    public bool m_Locked;

    void Update()
    {
        if(m_Progress >= m_AmtNeeded)
        {
            if (!m_Locked)
            {
                m_Locked = true;
                StoreInventory.GiveItem(GuardianPetsAssets.SHIELD_CURRENCY_ITEM_ID, m_RewardValue);
            }
        }
    }

    public void IncrementProgress()
    {
        if (m_Progress < m_AmtNeeded)
        {
            m_Progress++;
        }
    }
}