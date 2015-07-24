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
            return new VirtualCurrency[] { SHIELD_CURRENCY, SCANNER_CURRENCY };
        }

        public VirtualGood[] GetGoods()
        {
            return new VirtualGood[] { UNLIM_SCANS_LTVG };
        }

        public VirtualCurrencyPack[] GetCurrencyPacks()
        {
            return new VirtualCurrencyPack[] { HUNDREDEIGHTY_PACK, FIVEHUNDRED_PACK, TWELVEHUNDRED_PACK, THIRTYONEHUNDRED_PACK, SIXTYFIVEHUNDRED_PACK, FOURTEENTHOUSANDSHIELD_PACK, TWENTYFIVE_SCANS_PACK };
        }

        public VirtualCategory[] GetCategories()
        {
            return new VirtualCategory[] { };
        }

        public const string SHIELD_CURRENCY_ITEM_ID = "currency_shield";
        public const string SCANNER_CURRENCY_ITEM_ID = "currency_scanner";

        public const string HUNDREDEIGHTY_PACK_PRODUCT_ID = "180_pack";
        public const string FIVEHUNDRED_PACK_PRODUCT_ID = "500_pack";
        public const string TWELVEHUNDRED_PACK_PRODUCT_ID = "1200_pack";
        public const string THIRTYONEHUNDRED_PACK_PRODUCT_ID = "3100_pack";
        public const string SIXTYFIVETHOUSANDSHIELD_PACK_PRODUCT_ID = "6500_pack";
        public const string FOURTEENTHOUSANDSHIELD_PACK_PRODUCT_ID = "14000_pack";
        public const string TWENTYFIVE_SCANS_PACK_PRODUCT_ID = "25_scans_pack";

        public const string UNLIMITED_SCANS_PRODUCT_ID = "unlim_scans";

        //Virtual Currencies
        public static VirtualCurrency SHIELD_CURRENCY = new VirtualCurrency(
            "Shields",                                                   //Name
            "",                                                          //Description
            SHIELD_CURRENCY_ITEM_ID);                                    //Item ID

        public static VirtualCurrency SCANNER_CURRENCY = new VirtualCurrency(
            "Scans",                                                     //Name
            "",                                                          //Description
            SCANNER_CURRENCY_ITEM_ID);                                   //Item ID

        //Virtual Currency Packs
        public static VirtualCurrencyPack HUNDREDEIGHTY_PACK = new VirtualCurrencyPack(
           "180 Shields",                                                //Name
           "This costs $1.99",                                           //Description
           "shields_180",                                                //Item ID
           180,
           SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
           new PurchaseWithMarket(HUNDREDEIGHTY_PACK_PRODUCT_ID, 1.99)
           );

        public static VirtualCurrencyPack FIVEHUNDRED_PACK = new VirtualCurrencyPack(
           "500 Shields",                                                //Name
           "11% More Shields!\nThis costs $4.99",                        //Description
           "shields_500",                                                //Item ID
           500,
           SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
           new PurchaseWithMarket(FIVEHUNDRED_PACK_PRODUCT_ID, 4.99)
           );

        public static VirtualCurrencyPack TWELVEHUNDRED_PACK = new VirtualCurrencyPack(
           "1200 Shields",                                                //Name
           "33% More Shields!\nThis costs $9.99",                         //Description
           "shields_1200",                                                //Item ID
           1200,
           SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
           new PurchaseWithMarket(TWELVEHUNDRED_PACK_PRODUCT_ID, 9.99)
           );

        public static VirtualCurrencyPack THIRTYONEHUNDRED_PACK = new VirtualCurrencyPack(
           "3100 Shields",                                                //Name
           "38% More Shields!\nThis costs $24.99",                        //Description
           "shields_3100",                                                //Item ID
           3100,
           SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
           new PurchaseWithMarket(THIRTYONEHUNDRED_PACK_PRODUCT_ID, 24.99)
           );

        public static VirtualCurrencyPack SIXTYFIVEHUNDRED_PACK = new VirtualCurrencyPack(
            "6500 Shields",                                                //Name
            "44% More Shields!\nThis costs $49.99",                        //Description
            "shields_6500",                                                //Item ID
            6500,
            SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
            new PurchaseWithMarket(SIXTYFIVETHOUSANDSHIELD_PACK_PRODUCT_ID, 49.99)
            );

        public static VirtualCurrencyPack FOURTEENTHOUSANDSHIELD_PACK = new VirtualCurrencyPack(
            "14000 Shields",                                                //Name
            "56% More Shields!\nThis costs $99.99",                         //Description
            "shields_14000",                                                //Item ID
            14000,
            SHIELD_CURRENCY_ITEM_ID,                                       //The currency associated with this pack
            new PurchaseWithMarket(FOURTEENTHOUSANDSHIELD_PACK_PRODUCT_ID, 99.99)
            );

        public static VirtualCurrencyPack TWENTYFIVE_SCANS_PACK = new VirtualCurrencyPack(
            "25 Scans",                                                     //Name
            "Scan an additional 25 times!\nThis costs $1.99",                                 //Description
            "scans_25",                                                     //Item ID
            25,
            SCANNER_CURRENCY_ITEM_ID,
            new PurchaseWithMarket(TWENTYFIVE_SCANS_PACK_PRODUCT_ID, 1.99)
            );

        public static VirtualGood UNLIM_SCANS_LTVG = new LifetimeVG(
            "Unlimited Scans", 											    //Name
            "Unlimited Scans for Life!",				 					//Description
            "unlim_scans",													//Item ID
            new PurchaseWithMarket(UNLIMITED_SCANS_PRODUCT_ID, 4.99));	    //The way this virtual good is purchased
    }
}