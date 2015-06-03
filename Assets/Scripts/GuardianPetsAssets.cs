using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
    public class GuardianPetsAssets : IStoreAssets
    {
        public int GetVersion()
        {
            return 0;
        }

        public VirtualCurrency[] GetCurrencies()
        {
            return new VirtualCurrency[] { SHIELD_CURRENCY };
        }

        public VirtualGood[] GetGoods()
        {
            return new VirtualGood[] { };
        }

        public VirtualCurrencyPack[] GetCurrencyPacks()
        {
            return new VirtualCurrencyPack[] {TENSHIELD_PACK, FIFTYSHIELD_PACK, FOURHUNDSHIELD_PACK, THOUSANDSHIELD_PACK};
        }

        public VirtualCategory[] GetCategories()
        {
            return new VirtualCategory[] { };
        }

        public const string SHIELD_CURRENCY_ITEM_ID = "currency_shield";
        public const string TENSHIELD_PACK_PRODUCT_ID = "android.test.refunded";
        public const string FIFTYSHIELD_PACK_PRODUCT_ID = "android.test.canceled";
        public const string FOURHUNDSHIELD_PACK_PRODUCT_ID = "android.test.purchased";
        public const string THOUSANDSHIELD_PACK_PRODUCT_ID = "2500_pack";

        //Virtual Currencies
        public static VirtualCurrency SHIELD_CURRENCY = new VirtualCurrency(
            "Muffins",                                                   //Name
            "",                                                          //Description
            SHIELD_CURRENCY_ITEM_ID);                                    //Item ID


        //Virtual Currency Packs
        public static VirtualCurrencyPack TENSHIELD_PACK = new VirtualCurrencyPack(
            "10 Shields",                                                //Name
            "Test refund of an item",                                    //Description
            "shields_10",                                                //Item ID
            10,
            SHIELD_CURRENCY_ITEM_ID,                                     //The currency associated with this pack
            new PurchaseWithMarket(TENSHIELD_PACK_PRODUCT_ID, 0.99)
            );

        public static VirtualCurrencyPack FIFTYSHIELD_PACK = new VirtualCurrencyPack(
            "50 Shields",                                                //Name
            "Test cancellation of an item",                              //Description
            "shields_50",                                                //Item ID
            50,
            SHIELD_CURRENCY_ITEM_ID,                                     //The currency associated with this pack
            new PurchaseWithMarket(TENSHIELD_PACK_PRODUCT_ID, 1.99)
            );

        public static VirtualCurrencyPack FOURHUNDSHIELD_PACK = new VirtualCurrencyPack(
            "400 Shields",                                                //Name
            "Test refund of an item",                                     //Description
            "shields_400",                                                //Item ID
            400,
            SHIELD_CURRENCY_ITEM_ID,                                      //The currency associated with this pack
            new PurchaseWithMarket(TENSHIELD_PACK_PRODUCT_ID, 4.99)
            );

        public static VirtualCurrencyPack THOUSANDSHIELD_PACK = new VirtualCurrencyPack(
            "1000 Shields",                                                //Name
            "Test refund of an item",                                      //Description
            "shields_1000",                                                //Item ID
            1000,
            SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
            new PurchaseWithMarket(TENSHIELD_PACK_PRODUCT_ID, 8.99)
            );
    }
}