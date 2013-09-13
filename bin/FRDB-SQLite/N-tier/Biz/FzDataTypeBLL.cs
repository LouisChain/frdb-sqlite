using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace N_tier
{
    public class FzDataTypeBLL
    {
        #region 1. Fields

        private String _typeName;// TypeName != DataType if DataType == "User-Defined"
        private String _dataType;
        private String _domainString;
        private List<String> _domainValues;

        #endregion

        #region 2. Properties

        public String TypeName 
        {
            get { return _typeName; }
            set { _typeName = value; }
        }

        public String DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        public String DomainString
        {
            get { return _domainString; }
            set { _domainString = value; }
        }

        public List<String> DomainValues
        {
            get { return _domainValues; }
            set { _domainValues = value; }
        }

        #endregion

        #region 3. Contructors

        public FzDataTypeBLL()
        {
            this._typeName = String.Empty;
            this._dataType = String.Empty;
            this._domainString = String.Empty;
            this._domainValues = new List<String>();
        }

        public FzDataTypeBLL(String typeName, String domain)
        {
            this._typeName = typeName;
            this._domainString = domain;
            GetDomain(domain);
            this._dataType = "UserDefined";
            GetDataType();
        }

        #endregion

        #region 4. Publics

        public Boolean CheckDataType(Object value)
        {
            try
            {
                switch (this._dataType)
                {
                    case "Int16": Int16 a = 0; return (Int16.TryParse(value.ToString(), out a));
                    case "Int32": Int32 b = 0; return (Int32.TryParse(value.ToString(), out b));
                    case "Int64": Int64 c = 0; return (Int64.TryParse(value.ToString(), out c));
                    case "Byte": Byte d = 0; return (Byte.TryParse(value.ToString(), out d));
                    case "String": return true;
                    case "DateTime": return true;//DateTime e = DateTime.Today; return (DateTime.TryParse(value.ToString(), out e));
                    case "Decimal": Decimal f = 0; return (Decimal.TryParse(value.ToString(), out f));
                    case "Single": Single g = 0; return (Single.TryParse(value.ToString(), out g));
                    case "Double": Double h = 0; return (Double.TryParse(value.ToString(), out h));
                    case "Boolean":
                        if (value.ToString().ToLower() == "true" || value.ToString().ToLower() == "false") return true;
                        else return false;//Boolean k = true; return (Boolean.TryParse(value.ToString(), out k));
                    case "Binary": return (IsBinaryType(value));
                    case "Currency": return (IsCurrencyType(value));
                    case "UserDefined": return (this.DomainValues.Contains(value.ToString()));
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        #endregion

        #region 5. Privates

        private void GetDomain(String domain)
        {
            try
            {
                if (this._typeName == "UserDefined")
                {
                    domain = domain.Replace("{", "");
                    domain = domain.Replace("}", "");
                    char[] seperator = { ',' };
                    String[] temp = domain.Split(seperator);
                    _domainValues = new List<String>();

                    foreach (String value in temp)
                    {
                        _domainValues.Add(value.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR:\n" + ex.Message);
            }
        }

        private void GetDataType()
        {
            try
            {
                switch (this._typeName)
                {
                    case "Int16": this._dataType = "Int16"; break;
                    case "Int64": this._dataType = "Int64"; break;
                    case "Int32": this._dataType = "Int32"; break;
                    case "Byte": this._dataType = "Byte"; break;
                    case "Decimal": this._dataType = "Decimal"; break;
                    case "Currency": this._dataType = "Currency"; break;
                    case "String": this._dataType = "String"; break;
                    case "DateTime": this._dataType = "DateTime"; break;
                    case "Binary": this._dataType = "Binary"; break;
                    case "Single": this._dataType = "Single"; break;
                    case "Double": this._dataType = "Double"; break;
                    case "Boolean": this._dataType = "Boolean"; break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR:\n" + ex.Message);
            }
        }

        private static bool IsBinaryType(object v)
        {
            try
            {
                foreach (char i in v.ToString())
                {
                    if (i != '0' && i != '1')
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private static bool IsCurrencyType(object v)
        {
            try
            {
                double MINCURRENCY = 1.0842021724855044340074528008699e-19;
                double MAXCURRENCY = 9223372036854775807.0;
                double temp = Convert.ToDouble(v);

                if (temp - MINCURRENCY >= 0)
                {
                    if (temp - MAXCURRENCY <= 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        private static bool IsNumber(String pText)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(pText);
        }

        #endregion
    }
}
