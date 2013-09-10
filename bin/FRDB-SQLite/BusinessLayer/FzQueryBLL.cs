using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class FzQueryBLL
    {
         #region 1. Fields

        private String _queryName;//Each query has a name
        private String _queryString;// And each queryName contents a string query

        #endregion

        #region 2. Properties

        public String QueryName
        {
            get { return _queryName; }
            set { _queryName = value; }
        }

        public String QueryString
        {
            get { return _queryString; }
            set { _queryString = value; }
        }

        #endregion

        #region 3. Contructors

        public FzQueryBLL()
        {
            this._queryName = String.Empty;
            this._queryString = String.Empty;
        }

        public FzQueryBLL(String queryString)
        {
            this._queryName = String.Empty;
            this._queryString = queryString;
        }

        public FzQueryBLL(String queryName, String queryString)
        {
            this._queryName = queryName;
            this._queryString = queryString;
        }

        #endregion

        #region 4. Methods


       
        #endregion

        #region 5. Privates



        #endregion
    }
}
