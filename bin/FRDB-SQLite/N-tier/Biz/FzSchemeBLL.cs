using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N_tier
{
    public class FzSchemeBLL
    {
        #region 1. Fields

        private String _schemeName;
        private List<FzAttributeBLL> _attributes; //List of attributes of scheme

        #endregion

        #region 2. Properties

        public String SchemeName
        {
            get { return _schemeName; }
            set { _schemeName = value; }
        }

        public List<FzAttributeBLL> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        #endregion

        #region 3. Contructors

        public FzSchemeBLL()
        {
            this._schemeName = String.Empty;
            this._attributes = new List<FzAttributeBLL>();
        }

        public FzSchemeBLL(String schemeName, List<FzAttributeBLL> attributes)
        {
            this._schemeName = schemeName;
            this._attributes = attributes;
        }

        public FzSchemeBLL(String schemeName)
        {
            this._schemeName = schemeName;
            this._attributes = new List<FzAttributeBLL>();
        }

        #endregion

        #region 4. Methods

        public static Boolean IsInherited(FzSchemeBLL scheme, List<FzRelationBLL> relations)
        {
            if (scheme.Attributes.Count > 0)
            {
                return FzSchemeDAL.IsInherited(scheme, relations);
            }
            return false;
        }

        public static Boolean IsInherited(FzDbBLL fdb)
        {
            return FzSchemeDAL.IsInherited(fdb);
        }

        public static FzSchemeBLL GetSchemeByName(String name, FzDbBLL fdb)
        {
            return FzSchemeDAL.GetSchemeByName(name, fdb);
        }

        public static List<String> GetListSchemeName(FzDbBLL fdb)
        {
            return FzSchemeDAL.GetListSchemeName(fdb);
        }

        public static void RenameScheme(String oldName, String newName, FzDbBLL fdb)
        {
            if (fdb.Schemes.Count > 0)
            {
                FzSchemeDAL.RenameScheme(oldName, newName, fdb);
            }
        }

        public static Boolean DeleteAllSchems(FzDbBLL fdb)
        {
            return FzSchemeDAL.DeleteAllSchemes(fdb);
        }
       
        #endregion

        #region 5. Privates



        #endregion
    }
}
