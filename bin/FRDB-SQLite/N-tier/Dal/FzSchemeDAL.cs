using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace N_tier
{
    public class FzSchemeDAL
    {
        #region Publics

        public static Boolean IsInherited(FzSchemeBLL scheme, List<FzRelationBLL> relations)
        {
            try
            {
                foreach (FzRelationBLL relation in relations)
                {
                    if (scheme.Equals(relation.Scheme))
                    {
                        return true;
                        //break;
                    }
                }
            }
            catch { }

            return false;
        }

        public static Boolean IsInherited(FzDbBLL fdb)
        {
            try
            {
                foreach (var item in fdb.Schemes)
                    if (IsInherited(item, fdb.Relations))
                        return false;
            }
            catch { }

            return true;
        }

        public static FzSchemeBLL GetSchemeByName(String schemeName, FzDbBLL fdb)
        {
            foreach (FzSchemeBLL s in fdb.Schemes)
            {
                if (s.SchemeName == schemeName)
                {
                    return s;
                }
            }

            return null;
        }

        public static List<String> GetListSchemeName(FzDbBLL fdb)
        {
            List<String> schemeNames = new List<String>();

            foreach (FzSchemeBLL scheme in fdb.Schemes)
            {
                schemeNames.Add(scheme.SchemeName);
            }

            return schemeNames;
        }

        public static void RenameScheme(String oldName, String newName, FzDbBLL fdb)
        {
            foreach (var item in fdb.Schemes)
            {
                if (item.SchemeName == oldName)
                {
                    item.SchemeName = newName;
                    break;
                }
            }
        }

        public static Boolean DeleteAllSchemes(FzDbBLL fdb)
        {
            foreach (var item in fdb.Schemes)
            {
                fdb.Schemes.Remove(item);
            }
            return true;
        }

        #endregion

        #region Privates
        #endregion
    }
}
