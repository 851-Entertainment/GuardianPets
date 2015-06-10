using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class StoreEventHandler : MonoBehaviour
{
    public StoreEventHandler()
    {
        StoreEvents.OnMarketPurchase += onMarketPurchase;
		StoreEvents.OnMarketRefund += onMarketRefund;
		StoreEvents.OnItemPurchased += onItemPurchased;
		StoreEvents.OnGoodEquipped += onGoodEquipped;
		StoreEvents.OnGoodUnEquipped += onGoodUnequipped;
		StoreEvents.OnGoodUpgrade += onGoodUpgrade;
		StoreEvents.OnBillingSupported += onBillingSupported;
		StoreEvents.OnBillingNotSupported += onBillingNotSupported;
		StoreEvents.OnMarketPurchaseStarted += onMarketPurchaseStarted;
		StoreEvents.OnItemPurchaseStarted += onItemPurchaseStarted;
		StoreEvents.OnUnexpectedErrorInStore += onUnexpectedErrorInStore;
		StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
		StoreEvents.OnGoodBalanceChanged += onGoodBalanceChanged;
		StoreEvents.OnMarketPurchaseCancelled += onMarketPurchaseCancelled;
		StoreEvents.OnRestoreTransactionsStarted += onRestoreTransactionsStarted;
		StoreEvents.OnRestoreTransactionsFinished += onRestoreTransactionsFinished;
		StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreInitialized;
#if UNITY_ANDROID && !UNITY_EDITOR
		StoreEvents.OnIabServiceStarted += onIabServiceStarted;
		StoreEvents.OnIabServiceStopped += onIabServiceStopped;
#endif
	}

    // Handles a market purchase event.
    public void onMarketPurchase(PurchasableVirtualItem pvi, string payload, Dictionary<string, string> extra)
    {

    }

    // Handles a market refund event.
    public void onMarketRefund(PurchasableVirtualItem pvi)
    {

    }

    // Handles an item purchase event.
    public void onItemPurchased(PurchasableVirtualItem pvi, string payload)
    {

    }

    // Handles a good equipped event.
    public void onGoodEquipped(EquippableVG good)
    {

    }

    // Handles a good unequipped event.
    public void onGoodUnequipped(EquippableVG good)
    {

    }

    // Handles a good upgraded event.
    public void onGoodUpgrade(VirtualGood good, UpgradeVG currentUpgrade)
    {

    }

    // Handles a billing supported event.
    public void onBillingSupported()
    {

    }

    // Handles a billing NOT supported event.
    public void onBillingNotSupported()
    {

    }

    // Handles a market purchase started event.
    public void onMarketPurchaseStarted(PurchasableVirtualItem pvi)
    {

    }

    // Handles an item purchase started event.
    public void onItemPurchaseStarted(PurchasableVirtualItem pvi)
    {

    }

    // Handles an item purchase cancelled event.
    public void onMarketPurchaseCancelled(PurchasableVirtualItem pvi)
    {

    }

    // Handles an unexpected error in store event.
    public void onUnexpectedErrorInStore(string message)
    {

    }

    /// <summary>
    /// Handles a currency balance changed event.
    /// </summary>
    /// <param name="virtualCurrency">Virtual currency whose balance has changed.</param>
    /// <param name="balance">Balance of the given virtual currency.</param>
    /// <param name="amountAdded">Amount added to the balance.</param>
    public void onCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded)
    {

    }

    /// <summary>
    /// Handles a good balance changed event.
    /// </summary>
    /// <param name="good">Virtual good whose balance has changed.</param>
    /// <param name="balance">Balance.</param>
    /// <param name="amountAdded">Amount added.</param>
    public void onGoodBalanceChanged(VirtualGood good, int balance, int amountAdded)
    {

    }

    // Handles a restore Transactions process started event.
    public void onRestoreTransactionsStarted()
    {

    }

    // Handles a restore transactions process finished event.
    // success - true if successful
    public void onRestoreTransactionsFinished(bool success)
    {

    }

    // Handles a store controller initialized event.
    public void onSoomlaStoreInitialized()
    {

    }

#if UNITY_ANDROID && !UNITY_EDITOR
	public void onIabServiceStarted()
    {

	}

	public void onIabServiceStopped() 
    {

	}
#endif
}