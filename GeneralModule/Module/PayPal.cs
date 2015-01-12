using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using GeneralModule.Utilily.Extension;

namespace GeneralModule.Module
{
    public class PayPal
    {
        /// <summary>
        /// PayPal IPN Module
        /// https://developer.paypal.com/docs/classic/ipn/integration-guide/IPNIntro/
        /// </summary>
        public class IpnModule
        {

            #region Properties
            //Information about you:
            public string receiver_email { get; set; }
            public string receiver_id { get; set; }
            public string residence_country { get; set; }

            //Information about the transaction:
            public string test_ipn { get; set; }    //Testing with the Sandbox if equal 1
            public string transaction_subject { get; set; }
            public string txn_id { get; set; }      //Keep this ID to avoid processing the transaction twice
            public string txn_type { get; set; }    //Type of transaction

            //Information about your buyer:
            public string payer_email { get; set; }
            public string payer_id { get; set; }
            public string payer_status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string address_city { get; set; }
            public string address_country { get; set; }
            public string address_country_code { get; set; }
            public string address_name { get; set; }
            public string address_state { get; set; }
            public string address_status { get; set; }
            public string address_street { get; set; }
            public string address_zip { get; set; }

            public string auction_buyer_id { get; set; }

            //Information about the payment:
            public string custom { get; set; }  //Your custom field
            public string handling_amount { get; set; }
            public string item_name { get; set; }
            public string item_number { get; set; }
            public string mc_currency { get; set; }
            public string mc_fee { get; set; }
            public string mc_gross { get; set; }
            public string payment_date { get; set; }
            public string payment_fee { get; set; }
            public string payment_gross { get; set; }
            public string payment_status { get; set; }  //Status, which determines whether the transaction is complete
            public string payment_type { get; set; }    //Kind of payment
            public string protection_eligibility { get; set; }
            public string quantity { get; set; }
            public string shipping { get; set; }
            public string tax { get; set; }


            //Other information about the transaction:
            public string notify_version { get; set; }
            public string charset { get; set; }
            public string verify_sign { get; set; }
            public string invoice { get; set; }

            //public string OrderSource { get; set; }
            public string ReqParams { get; private set; }
            private NameValueCollection Paras { get; set; }
            #endregion

            public IpnModule()
            {
                var req = HttpContext.Current.Request;
                this.Paras = req.Params;
                PropertyInfo[] properties = this.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var proName = property.Name;
                    var value = this.QueryPara(proName);
                    if (value == null) continue;
                    if (!property.CanWrite) continue;

                    property.SetValue(this, value, null);
                }
                this.ReqParams = req.eStreamToString();


                if (this.ReqParams.eIsSpace() && this.Paras != null)
                {
                    this.ReqParams = this.Paras
                        .OfType<string>()
                        .Aggregate((pre, next) =>
                                String.Format("{0}={1}&{2}={3}",
                                pre,
                                HttpUtility.HtmlEncode(this.Paras[pre]),
                                next,
                                HttpUtility.HtmlEncode(this.Paras[next])
                                ));
                }
            }
            public IpnModule(HttpRequestBase req)
            {
                this.Paras = req.Params;
                PropertyInfo[] properties = this.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var proName = property.Name;
                    var value = QueryPara(proName);
                    if (value == null) continue;
                    if (!property.CanWrite) continue;

                    property.SetValue(this, value, null);
                }
                this.ReqParams = req.eStreamToString();


                if (this.ReqParams.eIsSpace() && this.Paras != null)
                {
                    this.ReqParams = this.Paras
                        .OfType<string>()
                        .Aggregate((pre, next) =>
                                String.Format("{0}={1}&{2}={3}",
                                pre,
                                HttpUtility.HtmlEncode(this.Paras[pre]),
                                next,
                                HttpUtility.HtmlEncode(this.Paras[next])
                                ));
                }

            }

            private string QueryPara(string name)
            {
                return this.Paras.Get(name);
            }

            public string SendPost(string reqUrl)
            {
                if (String.IsNullOrWhiteSpace(this.ReqParams))
                {
                    throw new Exception("No params");
                }

                var req = WebRequest.Create(reqUrl);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                var newParams = this.ReqParams + "&cmd=_notify-validate";
                req.ContentLength = newParams.Length;
                using (var writer = new StreamWriter(req.GetRequestStream(), Encoding.ASCII))
                {
                    writer.Write(newParams);
                }
                string rep;
                using (var reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.ASCII))
                {
                    rep = reader.ReadToEnd();
                }

                return rep;
            }
            public void SendPost(string reqUrl, Action<string> verified, Action<string> invalid)
            {

                string rep = this.SendPost(reqUrl);
                if (String.IsNullOrWhiteSpace(rep))
                {
                    invalid(rep);
                    return;
                }
                rep = rep.ToUpper();

                if (rep == "VERIFIED")
                {
                    verified(rep);
                    return;
                }
                invalid(rep);
            }



            public static void Listening(
                HttpRequestBase req,
                string payPalUrl,
                Func<IpnModule, bool> preSendPost,
                Action<string, IpnModule> verified,
                Action<string, IpnModule> invalid)
            {
                var module = new IpnModule(req);
                if (!preSendPost(module)) return;
                module.SendPost(
                    payPalUrl,
                    rep => verified(rep, module),
                    rep => invalid(rep, module));
            }
        }

        public class BuildLink
        {
            #region Properties
            //Allowable Values for the cmd HTML Variable
            /*
             * _xclick	The button that the person clicked was a Buy Now button.
             * _cart	For shopping cart purchases. The following variables specify the kind of shopping cart button that the person clicked:
             *          add – Add to Cart buttons for the PayPal Shopping Cart
             *          display – View Cart buttons for the PayPal Shopping Cart
             *          upload – The Cart Upload command for third-party carts
             * _oe-gift-certificate	The button that the person clicked was a Buy Gift Certificate button.
             * _xclick-subscriptions	The button that the person clicked was a Subscribe button.
             * _xclick-auto-billing	The button that the person clicked was an Automatic Billing button.
             * _xclick-payment-plan	The button that the person clicked was an Installment Plan button.
             * _donations	The button that the person clicked was a Donate button.
             * _s-xclick	The button that the person clicked was protected from tampering by using encryption, or the button was saved in the merchant's PayPal account. PayPal determines which kind of button was clicked by decoding the encrypted code or by looking up the saved button in the merchant's account.
             */
            public string cmd { get; set; }



            //HTML Variables for Special PayPal Features
            //Optional: The URL to which PayPal posts information about the payment, in the form of Instant Payment Notification messages.
            public string notify_url { get; set; }

            /*
             * Required for buttons that have been saved in PayPal accounts; otherwise, not allowed.
             * The identifier of a button that was saved in a merchant's PayPal account. PayPal assigns the value when payment buttons are first created and saved in merchants' PayPal accounts.
             * Note: A merchant's PayPal account can have a maximum of 1,000 saved payment buttons.
             */
            public string hosted_button_id { get; set; }

            /*
             * Optional
             * An identifier of the source that built the code for the button that the buyer clicked, sometimes known as the build notation. Specify a value using the following format: 
             * <Company>_<Service>_<Product>_<Country> 
             * Substitute <Service> with an appropriate value from the following list:
             * -----
             * BuyNow
             * AddToCart
             * Donate
             * Subscribe
             * AutomaticBilling
             * InstallmentPlan
             * BuyGiftCertifcate
             * ShoppingCart
             * Substitute <Product> with WPS always for PayPal Payments Standard payment buttons and for the PayPal Payments Standard Cart Upload command.
             * Substitute <Country> with an appropriate two-letter country code from codes defined by the ISO 3166-1 standard.
             * -----
             * For example, a Buy Now button on your website that you coded yourself might have the following line of code:
             * bn="DesignerFotos_BuyNow_WPS_US"
             * Note: HTML button code that you create on the PayPal website includes bn variables with valid values generated by PayPal.
             */
            public string bn { get; set; }

            //HTML Variables for Individual Items

            /*
             * The price or amount of the product, service, or contribution, not including shipping, handling, or tax. If this variable is omitted from Buy Now or Donate buttons, buyers enter their own amount at the time of payment. 
             * Required for Add to Cart buttons
             * Optional for Buy Now and Donate buttons
             * Not used with Subscribe or Buy Gift Certificate buttons
             */
            public string amount { get; set; }

            /*
             * Optional
             * 
             * Discount amount associated with an item. 
             * It must be less than the selling price of the item. If you specify discount_amount and discount_amount2 is not defined, then this flat amount is applied regardless of the quantity of items purchased. 
             * Valid only for Buy Now and Add to Cart buttons.
             */
            public string discount_amount { get; set; }

            /*
             * Optional
             * 
             * Discount amount associated with each additional quantity of the item. 
             * It must be equal to or less than the selling price of the item. A discount_amount must also be specified as greater than or equal to 0 for discount_amount2 to take effect.
             * Valid only for Buy Now and Add to Cart buttons.
             */
            public string discount_amount2 { get; set; }

            /*
             * Optional
             * 
             * Discount rate (percentage) associated with an item. 
             * It must be set to a value less than 100. If you do not set discount_rate2, the value in discount_rate applies only to the first item regardless of the quantity of items purchased. 
             * Valid only for Buy Now and Add to Cart buttons.
             */
            public string discount_rate { get; set; }

            /*
             * Optional
             * 
             * Discount rate (percentage) associated with each additional quantity of the item.  
             * It must be equal to or less 100. A discount_rate must also be specified as greater than or equal to 0 for discount_rate2 to take effect. 
             * Valid only for Buy Now and Add to Cart buttons.
             */
            public string discount_rate2 { get; set; }

            /*
             * Pass-through variable for you to track product or service purchased or the contribution made. The value you specify is passed back to you upon payment completion. This variable is required if you want PayPal to track inventory or track profit and loss for the item the button sells.
             */
            public string discount_num { get; set; }

            /*
             * Optional
             * 
             * Number of items. If profile-based shipping rates are configured with a basis of quantity, the sum of quantity values is used to calculate the shipping charges for the payment. PayPal appends a sequence number to identify uniquely the item in the PayPal Shopping Cart, for example, quantity1, quantity2, and so on.
             * Note: The value for quantity must be a positive integer. Null, zero, or negative numbers are not allowed.
             */
            public string quantity { get; set; }

            /*
             * Optional
             * 
             * The cost of shipping this item. If you specify shipping and shipping2 is not defined, this flat amount is charged regardless of the quantity of items purchased. 
             * This shipping variable is valid only for Buy Now and Add to Cart buttons.
             * Default – If profile-based shipping rates are configured, buyers are charged an amount according to the shipping methods they choose.
             */
            public string shipping { get; set; }

            /*
             * Optional
             * 
             * The cost of shipping each additional unit of this item. If this variable is omitted and profile-based shipping rates are configured, buyers are charged an amount according to the shipping methods they choose.
             * This shipping variable is valid only for Buy Now and Add to Cart buttons.
             */
            public string shipping2 { get; set; }

            /*
             * Optional
             * 
             * Transaction-based tax override variable. Set this variable to a flat tax amount to apply to the payment regardless of the buyer's location. This value overrides any tax settings set in your account profile. Valid only for Buy Now and Add to Cart buttons. Default – Profile tax settings, if any, apply.
             */
            public string tax { get; set; }

            /*
             * Optional
             * 
             * Transaction-based tax override variable. Set this variable to a percentage that applies to the amount multiplied by the quantity selected during checkout. This value overrides any tax settings set in your account profile. Allowable values are numbers 0.001 through 100. Valid only for Buy Now and Add to Cart buttons. Default – Profile tax settings, if any, apply.
             */
            public string tax_rate { get; set; }

            /*
             * Optional
             * 
             * 1 – allows buyers to specify the quantity.
             * Optional for Buy Now buttons
             * Not used with other buttons
             */
            public string undefined_quantity { get; set; }

            /*
             * Optional
             * 
             * Weight of items. If profile-based shipping rates are configured with a basis of weight, the sum of weight values is used to calculate the shipping charges for the payment. 
             * Allowable values are decimals numbers, with 2 significant digits to the right of the decimal point.
             */
            public string weight { get; set; }

            /*
             * Optional
             * 
             * The unit of measure if weight is specified. 
             * Allowable values are:
             * 
             * lbs
             * kgs
             * 
             * The default is lbs.
             */
            public string weight_unit { get; set; }

            /*
             * Optional
             * 
             * First option field name and label. The os0 variable contains the corresponding value for this option field. For example, if on0 is size, os0 could be large.
             * Optional for Buy Now, Add to Cart, Subscribe, Automatic Billing, and Installment Plan buttons
             * Not used with Donate or Buy Gift Certificate buttons
             */
            public string on0 { get; set; }

            /*
             * Optional
             * 
             * Second option field name and label. The os1 variable contains the corresponding value for this option field. For example, if on1 is color then os1 could be blue. 
             * You can specify a maximum of 7 option field names (6 with Subscribe buttons) by incrementing the option name index (on0 through on6).
             * Optional for Buy Now, Add to Cart, Subscribe, Automatic Billing, and Installment Plan buttons
             * Not used with Donate or Buy Gift Certificate buttons
             */
            public string on1 { get; set; }

            //HTML Variables for Payment Transactions
            /*
             * Optional
             * 
             * 1 – The address specified with automatic fill-in variables overrides the PayPal member's stored address. Buyers see the addresses that you pass in, but they cannot edit them. PayPal does not show addresses if they are invalid or omitted. 
             */
            public string address_override { get; set; }

            /*
             * Optional
             * 
             * The currency of the payment. The default is USD
             * For allowable values, https://developer.paypal.com/docs/classic/api/currency_codes/#id09A6G0U0GYK
             */
            public string currency_code { get; set; }
            /*
             * Optional
             * 
             * Pass-through variable for your own tracking purposes, which buyers do not see. 
             * Default – No variable is passed back to you.
             */
            public string custom { get; set; }

            /*
             * Optional
             * 
             * Handling charges. This variable is not quantity-specific. The same handling cost applies, regardless of the number of items on the order. 
             * Default – No handling charges are included.
             */
            public string handling { get; set; }

            /*
             * Optional
             * 
             * Pass-through variable you can use to identify your invoice number for this purchase. 
             * Default – No variable is passed back to you.
             */
            public string invoice { get; set; }

            /*
             * Optional
             * 
             * Cart-wide tax, overriding any individual item tax_x value
             */
            public string tax_cart { get; set; }

            /*
             * Optional
             * 
             * If profile-based shipping rates are configured with a basis of weight, PayPal uses this value to calculate the shipping charges for the payment. This value overrides the weight values of individual items. 
             * Allowable values are decimals numbers, with 2 significant digits to the right of the decimal point.
             */
            public string weight_cart { get; set; }

            // HTML Variables for Shopping Carts

            /*
             * Add an item to the PayPal Shopping Cart.
             * This variable must be set as follows:
             * add="1"
             * The alternative is the display="1" variable, which displays the contents of the PayPal Shopping Cart to the buyer. 
             * If both add and display are specified, display takes precedence.
             */
            public string add { get; set; }
            /*
             * Required
             * 
             * The amount associated with item x. To pass an aggregate amount for the entire cart, use amount_1. 
             * Applies only to the Cart Upload command.
             */
            public string amount_x { get; set; }
            /*
             * Required
             * 
             * Your PayPal ID or an email address associated with your PayPal account. Email addresses must be confirmed.
             */
            public string business { get; set; }
            /*
             * Optional
             * 
             * Single discount amount charged cart-wide. 
             * It must be less than the selling price of all items combined in the cart. This variable overrides any individual item discount_amount_x values, if present. 
             * Applies only to the Cart Upload command.
             */
            public string discount_amount_cart { get; set; }

            /*
             * Optional
             * 
             * The discount amount associated with item x.  
             * It must be less than the selling price of the associated item. This amount is added to any other item discounts in the cart. 
             * Applies only to the Cart Upload command.
             */
            public string discount_amount_x { get; set; }
            /*
             * Optional
             * 
             * Single discount rate (percentage) to be charged cart-wide. 
             * It must be set to a value less than 100. The variable overrides any individual item discount_rate_x values, if present. 
             * Applies only to the Cart Upload command.
             */
            public string discount_rate_cart { get; set; }

            /*
             * Optional
             * 
             * The discount rate associated with item x.  
             * It must be set to a value less than 100. The variable takes into account all quantities of item x. 
             * Applies only to the Cart Upload command.
             */
            public string discount_rate_x { get; set; }
            /*
             * Display the contents of the PayPal Shopping Cart to the buyer. This variable must be set as follows:
             * display="1"
             * The alternative is the add="1" variable, which adds an item to the PayPal Shopping Cart. 
             * If both add and display are specified, display takes precedence.
             */
            public string display { get; set; }
            /*
             * Optional
             * 
             * Single handling fee charged cart-wide. If handling_cart is used in multiple Add to Cart buttons, the handling_cart value of the first item is used.
             */
            public string handling_cart { get; set; }
            /*
             * Required
             * 
             * The name associated with item x. To pass an aggregate name for the entire cart, use item_name_1. 
             * Applies only to the Cart Upload command.
             */
            public string item_name_x { get; set; }
            /*
             * Optional
             * 
             * Indicates whether the payment is a final sale or an authorization for a final sale, to be captured later.
             * Allowable values are:
             * 
             * sale
             * authorization
             * order
             * 
             * The default value is sale. Set the value to authorization to place a hold on the PayPal account for the authorized amount. Set the value to order to authorize the payment without placing a hold on the PayPal account.
             * Important Tip : If you set paymentaction to order, use the Authorization & Capture API to authorize and capture the payment payments. The Merchant Services on the PayPal website let you capture payments only for authorizations, not for orders.
             */
            public string paymentaction { get; set; }
            /*
             * Optional
             * 
             * The URL of the page on the merchant website that buyers go to when they click the Continue Shopping button on the PayPal Shopping Cart page. 
             */
            public string shopping_url { get; set; }

            /*
             * Upload the contents of a third-party shopping cart or a custom shopping cart.
             * This variable must be set as follows:
             * upload="1"
             * The alternatives are the add="1" variable and the display="1" variables, which are used with the PayPal Shopping Cart.
             */
            public string upload { get; set; }

            //Subscribe Button HTML Variables
            /*
             * Optional
             * 
             * Reattempt on failure. If a recurring payment fails, PayPal attempts to collect the payment two more times before canceling the subscription.
             * Allowable values are:
             * 
             * 0 – do not reattempt failed recurring payments
             * 1 – reattempt failed recurring payments before canceling
             * 
             * The default is 1.
             */
            public string sra { get; set; }
            /*
             * Optional
             * 
             * Modification behavior. 
             * Allowable values are:
             * 
             * 0 – allows subscribers only to sign up for new subscriptions
             * 1 – allows subscribers to sign up for new subscriptions and modify their current subscriptions
             * 2 – allows subscribers to modify only their current subscriptions
             * 
             * The default value is 0. 
             */
            public string modify { get; set; }

            //HTML Variables for Displaying PayPal Checkout Pages
            /*
             * Optional
             * 
             * The URL to which PayPal redirects buyers' browser after they complete their payments. For example, specify a URL on your site that displays a "Thank you for your payment" page. 
             * Default – PayPal redirects the browser to a PayPal webpage.
             */
            public string @return { get; set; }
            /*
             * Optional
             * 
             * A URL to which PayPal redirects the buyers' browsers if they cancel checkout before completing their payments. For example, specify a URL on your website that displays a "Payment Canceled" page.
             * Default – PayPal redirects the browser to a PayPal webpage.
             */
            public string cancel_return { get; set; }

            //HTML Variables for Filling Out PayPal Checkout Pages Automatically for Buyers
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string city { get; set; }
            /*
             * Optional
             * 
             * Sets shipping and billing country. 
             * For allowable values, see https://developer.paypal.com/docs/classic/api/country_codes/#CountryCodes_id083SG0U0OY4
             */
            public string country { get; set; }
            public string email { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            #endregion

            public string GetParams()
            {
                var properties = this.GetType().GetProperties();
                var param = new StringBuilder();

                foreach (var property in properties)
                {
                    var value = property.GetValue(this, null);
                    if (value == null) continue;
                    var valueString = value.ToString();
                    if (String.IsNullOrWhiteSpace(valueString)) continue;
                    param.AppendFormat("{0}={1}&", property.Name, valueString);
                }
                var paramString = param.ToString();
                if (paramString.eLastIs("&"))
                {
                    paramString = paramString.Remove(paramString.Length - 1, 1);
                }
                return paramString;
            }
        }
    }
}