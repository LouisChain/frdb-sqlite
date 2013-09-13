using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace N_tier
{
    public class FzRelationDAL
    {
        
        #region Publics

        public static List<String> GetListRelationName(FzDbBLL fdb)
        {
            List<String> relationNames = new List<String>();

            foreach (FzRelationBLL relation in fdb.Relations)
            {
                relationNames.Add(relation.RelationName);
            }

            return relationNames;
        }

        public static FzRelationBLL GetRelationByName(String relationName, FzDbBLL fdb)
        {
            foreach (FzRelationBLL r in fdb.Relations)
            {
                if (r.RelationName == relationName)
                {
                    return r;
                }
            }

            return null;
        }

        #endregion

        #region Privates
        #endregion
    }
}
