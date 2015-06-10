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

        //Test version
        public VirtualCurrencyPack[] GetCurrencyPacks()
        {
            return new VirtualCurrencyPack[] { TENSHIELD_PACK, FIFTYSHIELD_PACK, FOURHUNDSHIELD_PACK, THOUSANDSHIELD_PACK };
        }

        //Live version
        /*public VirtualCurrencyPack[] GetCurrencyPacks()
        {
            return new VirtualCurrencyPack[] { HUNDREDEIGHTY_PACK, FIVEHUNDRED_PACK, TWELVEHUNDRED_PACK, THIRTYONEHUNDRED_PACK, SIXTYFIVEHUNDRED_PACK, FOURTEENTHOUSANDSHIELD_PACK };
        }*/

        public VirtualCategory[] GetCategories()
        {
            return new VirtualCategory[] { };
        }

        public const string SHIELD_CURRENCY_ITEM_ID = "currency_shield";
        public const string TENSHIELD_PACK_PRODUCT_ID = "android.test.refunded";
        public const string FIFTYSHIELD_PACK_PRODUCT_ID = "android.test.canceled";
        public const string FOURHUNDSHIELD_PACK_PRODUCT_ID = "android.test.purchased";
        public const string THOUSANDSHIELD_PACK_PRODUCT_ID = "1000_pack";
        
        public const string HUNDREDEIGHTY_PACK_PRODUCT_ID = "180_pack";
        public const string FIVEHUNDRED_PACK_PRODUCT_ID = "500_pack";
        public const string TWELVEHUNDRED_PACK_PRODUCT_ID = "1200_pack";
        public const string THIRTYONEHUNDRED_PACK_PRODUCT_ID = "3100_pack";
        public const string SIXTYFIVETHOUSANDSHIELD_PACK_PRODUCT_ID = "6500_pack";
        public const string FOURTEENTHOUSANDSHIELD_PACK_PRODUCT_ID = "14000_pack";

        //Virtual Currencies
        public static VirtualCurrency SHIELD_CURRENCY = new VirtualCurrency(
            "Shields",                                                   //Name
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
            new PurchaseWithMarket(FIFTYSHIELD_PACK_PRODUCT_ID, 1.99)
            );

        public static VirtualCurrencyPack FOURHUNDSHIELD_PACK = new VirtualCurrencyPack(
            "400 Shields",                                                //Name
            "Test refund of an item",                                     //Description
            "shields_400",                                                //Item ID
            400,
            SHIELD_CURRENCY_ITEM_ID,                                      //The currency associated with this pack
            new PurchaseWithMarket(FOURHUNDSHIELD_PACK_PRODUCT_ID, 4.99)
            );

        public static VirtualCurrencyPack THOUSANDSHIELD_PACK = new VirtualCurrencyPack(
            "1000 Shields",                                                //Name
            "1000 Shields!\nThis costs $8.99",                             //Description
            "shields_1000",                                                //Item ID
            1000,
            SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
            new PurchaseWithMarket(THOUSANDSHIELD_PACK_PRODUCT_ID, 8.99)
            );


/*        ACTUAL VALUES                                                                          */
        public static VirtualCurrencyPack HUNDREDEIGHTY_PACK = new VirtualCurrencyPack(
           "180 Shields",                                                //Name
           "180 Shields!\nThis costs $1.99",                             //Description
           "shields_180",                                                //Item ID
           180,
           SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
           new PurchaseWithMarket(HUNDREDEIGHTY_PACK_PRODUCT_ID, 1.99)
           );

        public static VirtualCurrencyPack FIVEHUNDRED_PACK = new VirtualCurrencyPack(
           "500 Shields",                                                //Name
           "500 Shields!\n11% More Shields!\nThis costs $4.99",          //Description
           "shields_500",                                                //Item ID
           500,
           SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
           new PurchaseWithMarket(FIVEHUNDRED_PACK_PRODUCT_ID, 4.99)
           );

        public static VirtualCurrencyPack TWELVEHUNDRED_PACK = new VirtualCurrencyPack(
           "1200 Shields",                                                //Name
           "1200 Shields!\n33% More Shields!\nThis costs $9.99",          //Description
           "shields_1200",                                                //Item ID
           1200,
           SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
           new PurchaseWithMarket(TWELVEHUNDRED_PACK_PRODUCT_ID, 9.99)
           );

        public static VirtualCurrencyPack THIRTYONEHUNDRED_PACK = new VirtualCurrencyPack(
           "3100 Shields",                                                //Name
           "3100 Shields!\n38% More Shields!\nThis costs $24.99",         //Description
           "shields_3100",                                                //Item ID
           3100,
           SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
           new PurchaseWithMarket(THIRTYONEHUNDRED_PACK_PRODUCT_ID, 24.99)
           );

        public static VirtualCurrencyPack SIXTYFIVEHUNDRED_PACK = new VirtualCurrencyPack(
            "6500 Shields",                                                //Name
            "6500 Shields!\n44% More Shields!\nThis costs $49.99",         //Description
            "shields_6500",                                                //Item ID
            6500,
            SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
            new PurchaseWithMarket(SIXTYFIVETHOUSANDSHIELD_PACK_PRODUCT_ID, 49.99)
            );

        public static VirtualCurrencyPack FOURTEENTHOUSANDSHIELD_PACK = new VirtualCurrencyPack(
            "14000 Shields",                                                //Name
            "14000 Shields!\n56% More Shields!\nThis costs $99.99",         //Description
            "shields_14000",                                                //Item ID
            14000,
            SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
            new PurchaseWithMarket(FOURTEENTHOUSANDSHIELD_PACK_PRODUCT_ID, 99.99)
            );
    }
}