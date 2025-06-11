using System.Collections.Generic;
using MarksAssets.FirebaseWebGL.Analytics;
using An = MarksAssets.FirebaseWebGL.Analytics.Analytics;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Analytics product on your firebase console.
 * Make sure you have 'analytics' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Follow https://firebase.google.com/docs/analytics/get-started?platform=web
 * Leave your browser's javascript console (web inspector) open to check the results and if there are any errors in the process.
 * This example does NOT support the emulator. Analytics is not on the supported list. See https://firebase.google.com/docs/emulator-suite#feature-matrix .
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Analytics {
        public class Quickstart : FirebaseExample {

            async void Start() {
                var analytics = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (analytics is null) return;

                //https://firebase.google.com/docs/analytics/user-properties?platform=web#set_user_properties_5
                An.setUserProperties(analytics, new Dictionary<string, object>() { ["favorite_food"] = "apples" });

                //https://firebase.google.com/docs/analytics/userid#setting_the_user_id
                An.setUserId(analytics, "123456");

                //https://firebase.google.com/docs/analytics/get-started?platform=web#start_logging_events_2
                An.logEvent(analytics, "notification_received");
                //https://firebase.google.com/docs/analytics/events?platform=web#log_events_5
                An.logEvent(analytics, EventNameString.select_content);

                //https://firebase.google.com/docs/analytics/screenviews#manually_track_screens
                An.logEvent(analytics, EventNameString.screen_view, new EventParams() { firebase_screen = "Quickstart", firebase_screen_class = "Analytics"});

                //https://firebase.google.com/docs/analytics/measure-ecommerce#implementation
                var item_jeggings = new Item() {
                    item_id = "SKU_123",
                    item_name = "jeggings",
                    item_category = "pants",
                    item_variant = "black",
                    item_brand = "Google",
                    price = 9.99,
                };

                var item_boots = new Item() {
                    item_id = "SKU_456",
                    item_name = "boots",
                    item_category = "shoes",
                    item_variant = "brown",
                    item_brand = "Google",
                    price = 24.99,
                };

                var item_socks = new Item() {
                    item_id = "SKU_789",
                    item_name = "ankle_socks",
                    item_category = "socks",
                    item_variant = "red",
                    item_brand = "Google",
                    price = 5.99,
                };

                //https://firebase.google.com/docs/analytics/measure-ecommerce#select_product
                EventParams params1 = new EventParams() {
                    item_list_id = "L001",
                    item_list_name = "Related products",
                    items = new Item[] { item_jeggings , item_boots, item_socks }
                };

                An.logEvent(analytics, EventNameString.view_item_list, params1);

                EventParams params2 = new EventParams() {
                    item_list_id = "L001",
                    item_list_name = "Related products",
                    items = new Item[] { item_jeggings }
                };

                An.logEvent(analytics, EventNameString.select_item, params2);

                //https://firebase.google.com/docs/analytics/measure-ecommerce#view_product
                EventParams params3 = new EventParams() {
                    currency = "USD",
                    value = 9.99,
                    items = new Item[] { item_jeggings }
                };

                An.logEvent(analytics, EventNameString.view_item_list, params3);

                //https://firebase.google.com/docs/analytics/measure-ecommerce#add_remove_product
                var item_jeggings_quantity = new Item() {
                    item_id = "SKU_123",
                    item_name = "jeggings",
                    item_category = "pants",
                    item_variant = "black",
                    item_brand = "Google",
                    price = 9.99,
                    quantity = 2
                };

                EventParams params4 = new EventParams() {
                    currency = "USD",
                    value = 19.98,
                    items = new Item[] { item_jeggings_quantity }
                };

                An.logEvent(analytics, EventNameString.add_to_wishlist, params4);
                An.logEvent(analytics, EventNameString.add_to_cart, params4);

                var item_boots_quantity = new Item() {
                    item_id = "SKU_123",
                    item_name = "jeggings",
                    item_category = "pants",
                    item_variant = "black",
                    item_brand = "Google",
                    price = 9.99,
                    quantity = 1
                };

                EventParams params5 = new EventParams() {
                    currency = "USD",
                    value = 44.97,
                    items = new Item[] { item_jeggings_quantity, item_boots_quantity }
                };

                An.logEvent(analytics, EventNameString.view_cart, params5);

                EventParams params6 = new EventParams() {
                    currency = "USD",
                    value = 24.99,
                    items = new Item[] { item_jeggings }
                };

                An.logEvent(analytics, EventNameString.remove_from_cart, params6);

                //https://firebase.google.com/docs/analytics/measure-ecommerce#initiate_checkout

                EventParams params7 = new EventParams() {
                    currency = "USD",
                    value = 14.98,
                    coupon = "SUMMER_FUN",
                    items = new Item[] { item_jeggings }
                };

                An.logEvent(analytics, EventNameString.begin_checkout, params7);

                EventParams params8 = new EventParams() {
                    currency = "USD",
                    value = 14.98,
                    coupon = "SUMMER_FUN",
                    shipping_tier = "Ground",
                    items = new Item[] { item_jeggings }
                };

                An.logEvent(analytics, EventNameString.add_shipping_info, params8);

                EventParams params9 = new EventParams() {
                    currency = "USD",
                    value = 14.98,
                    coupon = "SUMMER_FUN",
                    payment_type = "Visa",
                    items = new Item[] { item_jeggings }
                };

                An.logEvent(analytics, EventNameString.add_payment_info, params9);

                //https://firebase.google.com/docs/analytics/measure-ecommerce#make_purchase_refund
                EventParams params10 = new EventParams() {
                    transaction_id = "T12345",
                    affiliation = "Google Store",
                    currency = "USD",
                    value = 14.98,
                    tax = 2.85,
                    shipping = 5.34,
                    coupon = "SUMMER_FUN",
                    items = new Item[] { item_jeggings }
                };

                An.logEvent(analytics, EventNameString.purchase, params10);

                EventParams params11 = new EventParams() {
                    transaction_id = "T12345",
                    affiliation = "Google Store",
                    currency = "USD",
                    value = 14.98,
                    items = new Item[] { new Item() { item_id = "SKU_123", quantity = 1 } }
                };

                An.logEvent(analytics, EventNameString.refund, params11);

                //https://firebase.google.com/docs/analytics/measure-ecommerce#apply_promotions
                var params12 = new Dictionary<string, object>() {
                    ["promotion_id"] = "ABC123",
                    ["promotion_name"] = "Summer Sale",
                    ["creative_slot"] = "summer2020_promo.jpg",
                    ["value"] = 14.98,
                    ["location_id"] = "HERO_BANNER",
                    ["items"] = new Item[] { new Item() { item_id = "SKU_123", quantity = 1 } }
                };

                An.logEvent(analytics, "view_promotion", params12);

            }
        }

    }

    
}
