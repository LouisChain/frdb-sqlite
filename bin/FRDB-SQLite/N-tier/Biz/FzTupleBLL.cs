using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N_tier
{
    public class FzTupleBLL
    {
        #region 1. Fields
        private List<Object> _valuesOnPerRow;//List of values on a row of a tuple
        #endregion

        #region 2. Properties

        public List<Object> ValuesOnPerRow
        {
            get { return _valuesOnPerRow; }
            set {
                foreach (Object item in value)
                {
                    _valuesOnPerRow.Add(item);
                } 
            }
        }

        #endregion

        #region 3. Contructors

        public FzTupleBLL()
        {
            this._valuesOnPerRow = new List<object>();
        }

        public FzTupleBLL(String valuesOnPerRow)///consist of values of column on the same row
        {
            this._valuesOnPerRow = new List<Object>();

            Char[] seperator = { ',' };
            String[] values = valuesOnPerRow.Split(seperator);

            for (int i = 0; i < values.Length; i++)
            {
                this._valuesOnPerRow.Add(values[i]);
            }
        }

        public FzTupleBLL(FzTupleBLL old)
        {
            foreach (Object item in old._valuesOnPerRow)
            {
                this.ValuesOnPerRow.Add(item);
            }
        }

        #endregion

        #region 4. Methods (none)


       
        #endregion

        #region 5. Privates (none)



        #endregion
    }
}
