using Nop.Core;
using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.eWay
{
    /// <summary>
    /// Summary description for GatewayRequest.
    /// Copyright Web Active Corporation Pty Ltd  - All rights reserved. 1998-2004
    /// This code is for exclusive use with the eWAY payment gateway
    /// </summary>
    public class GatewayRequest
    {
        private string _txCustomerID = "";
        private int _txAmount = 0;
        private string _txCardholderName = "";
        private string _txCardNumber = "";
        private string _txCardExpiryMonth = "01";
        private string _txCardExpiryYear = "00";
        private string _txTransactionNumber = "";
        private string _txCardholderFirstName = "";
        private string _txCardholderLastName = "";
        private string _txCardholderEmailAddress = "";
        private string _txCardholderAddress = "";
        private string _txCardholderPostalCode = "";
        private string _txInvoiceReference = "";
        private string _txInvoiceDescription = "";
        private string _txCVN = "";
        private string _txOption1 = "";
        private string _txOption2 = "";
        private string _txOption3 = "";

        /// <summary>
        /// Gets or sets an Eway customer identifier
        /// </summary>
        public string EwayCustomerID
        {
            get
            {
                return _txCustomerID;
            }

            set
            {
                _txCustomerID = value;
            }
        }

        /// <summary>
        /// Gets or sets an invoice amount
        /// </summary>
        public int InvoiceAmount
        {
            get
            {
                return _txAmount;
            }

            set
            {
                _txAmount = value;
            }
        }

        /// <summary>
        /// Gets or sets a card holder name
        /// </summary>
        public string CardHolderName
        {
            get
            {
                return _txCardholderName;
            }

            set
            {
                _txCardholderName = value;
            }
        }

        /// <summary>
        /// Gets or sets a card expiry month
        /// </summary>
        public string CardExpiryMonth
        {
            get
            {
                return _txCardExpiryMonth;
            }

            set
            {
                _txCardExpiryMonth = value;
            }
        }

        /// <summary>
        /// Gets or sets a card expiry year
        /// </summary>
        public string CardExpiryYear
        {
            get
            {
                return _txCardExpiryYear;
            }

            set
            {
                _txCardExpiryYear = value;
            }
        }

        /// <summary>
        /// Gets or sets a transaction number
        /// </summary>
        public string TransactionNumber
        {
            get
            {
                return _txTransactionNumber;
            }

            set
            {
                _txTransactionNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets a purchaser first name
        /// </summary>
        public string PurchaserFirstName
        {
            get
            {
                return _txCardholderFirstName;
            }

            set
            {
                _txCardholderFirstName = value;
            }
        }

        /// <summary>
        /// Gets or sets a purchaser last name
        /// </summary>
        public string PurchaserLastName
        {
            get
            {
                return _txCardholderLastName;
            }

            set
            {
                _txCardholderLastName = value;
            }
        }

        /// <summary>
        /// Gets or sets a card number
        /// </summary>
        public string CardNumber
        {
            get
            {
                return _txCardNumber;
            }

            set
            {
                _txCardNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets a purchaser address
        /// </summary>
        public string PurchaserAddress
        {
            get
            {
                return _txCardholderAddress;
            }

            set
            {
                _txCardholderAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets a purchaser postal code
        /// </summary>
        public string PurchaserPostalCode
        {
            get
            {
                return _txCardholderPostalCode;
            }

            set
            {
                _txCardholderPostalCode = value;
            }
        }

        /// <summary>
        /// Gets or sets a purchaser email address
        /// </summary>
        public string PurchaserEmailAddress
        {
            get
            {
                return _txCardholderEmailAddress;
            }

            set
            {
                _txCardholderEmailAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets an invoice reference
        /// </summary>
        public string InvoiceReference
        {
            get { return _txInvoiceReference; }
            set { _txInvoiceReference = value; }
        }

        /// <summary>
        /// Gets or sets an invoice description
        /// </summary>
        public string InvoiceDescription
        {
            get { return _txInvoiceDescription; }
            set { _txInvoiceDescription = value; }
        }

        /// <summary>
        /// Gets or sets a CVN
        /// </summary>
        public string CVN
        {
            get { return _txCVN; }
            set { _txCVN = value; }
        }

        /// <summary>
        /// Gets or sets an Eway option 1
        /// </summary>
        public string EwayOption1
        {
            get { return _txOption1; }
            set { _txOption1 = value; }
        }

        /// <summary>
        /// Gets or sets an Eway option 2
        /// </summary>
        public string EwayOption2
        {
            get { return _txOption2; }
            set { _txOption2 = value; }
        }

        /// <summary>
        /// Gets or sets an Eway option 3
        /// </summary>
        public string EwayOption3
        {
            get { return _txOption3; }
            set { _txOption3 = value; }
        }

        /// <summary>
        /// Converts a request to XML
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            // We don't really need the overhead of creating an XML DOM object
            // to really just concatenate a string together.
            string _xml = "<ewaygateway>";
            _xml += CreateNode("ewayCustomerID", XmlHelper.XmlEncode(_txCustomerID));
            _xml += CreateNode("ewayTotalAmount", XmlHelper.XmlEncode(_txAmount.ToString()));
            _xml += CreateNode("ewayCardHoldersName", XmlHelper.XmlEncode(_txCardholderName));
            _xml += CreateNode("ewayCardNumber", XmlHelper.XmlEncode(_txCardNumber));
            _xml += CreateNode("ewayCardExpiryMonth", XmlHelper.XmlEncode(_txCardExpiryMonth));
            _xml += CreateNode("ewayCardExpiryYear", XmlHelper.XmlEncode(_txCardExpiryYear));
            _xml += CreateNode("ewayTrxnNumber", XmlHelper.XmlEncode(_txTransactionNumber));
            _xml += CreateNode("ewayCustomerInvoiceDescription", XmlHelper.XmlEncode(_txInvoiceDescription));
            _xml += CreateNode("ewayCustomerFirstName", XmlHelper.XmlEncode(_txCardholderFirstName));
            _xml += CreateNode("ewayCustomerLastName", XmlHelper.XmlEncode(_txCardholderLastName));
            _xml += CreateNode("ewayCustomerEmail", XmlHelper.XmlEncode(_txCardholderEmailAddress));
            _xml += CreateNode("ewayCustomerAddress", XmlHelper.XmlEncode(_txCardholderAddress));
            _xml += CreateNode("ewayCustomerPostcode", XmlHelper.XmlEncode(_txCardholderPostalCode));
            _xml += CreateNode("ewayCustomerInvoiceRef", XmlHelper.XmlEncode(_txInvoiceReference));
            _xml += CreateNode("ewayCVN", XmlHelper.XmlEncode(_txCVN));
            _xml += CreateNode("ewayOption1", XmlHelper.XmlEncode(_txOption1));
            _xml += CreateNode("ewayOption2", XmlHelper.XmlEncode(_txOption2));
            _xml += CreateNode("ewayOption3", XmlHelper.XmlEncode(_txOption3));
            _xml += "</ewaygateway>";

            return _xml;
        }

        /// <summary>
        /// Builds a simple XML node.
        /// </summary>
        /// <param name="NodeName">The name of the node being created.</param>
        /// <param name="NodeValue">The value of the node being created.</param>
        /// <returns>An XML node as a string.</returns>
        private string CreateNode(string NodeName, string NodeValue)
        {
            return "<" + NodeName + ">" + NodeValue + "</" + NodeName + ">";
        }
    }
}
