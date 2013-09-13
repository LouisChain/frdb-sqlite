using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace N_tier
{
    public class FzQueryDAL
    {
        public static List<String> ListOfQueryName(FzDbBLL fdb)
        {
            List<String> queryNames = new List<String>();
            foreach (FzQueryBLL query in fdb.Queries)
            {
                queryNames.Add(query.QueryName);
            }

            return queryNames;
        }

        public static FzQueryBLL GetQueryByName(String queryName, FzDbBLL fdb)
        {
            foreach (var item in fdb.Queries)
            {
                if (queryName.Equals(item.QueryName))
                {
                    return item;
                }
            }
            return null;
        }
    }
}
