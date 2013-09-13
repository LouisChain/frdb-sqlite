using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N_tier
{
    public class FzRelationBLL
    {
         #region 1. Fields

        private String _relationName;
        private FzSchemeBLL _scheme;//Each relation has a respective scheme
        private List<FzTupleBLL> _tuples;//List of tuples
        #endregion

        #region 2. Properties

        public String RelationName
        {
            get { return _relationName; }
            set { _relationName = value; }
        }

        public FzSchemeBLL Scheme
        {
            get { return _scheme; }
            set { _scheme = value; }
        }

        public List<FzTupleBLL> Tuples
        {
            get
            {
                //List<FzTupleEntity> result = new List<FzTupleEntity>();
                //foreach (FzTupleEntity item in this._tuples)
                //{
                //    FzTupleEntity tmp = new FzTupleEntity(item);
                //    result.Add(tmp);
                //}
                //return result;
                return _tuples;
            }
            set
            {
                foreach (FzTupleBLL item in value)
                {
                    FzTupleBLL tmp = new FzTupleBLL(item);
                    _tuples.Add(tmp);
                }
                //_tuples = value;
            }
        }

        #endregion

        #region 3. Contructors

        public FzRelationBLL()
        {
            this._relationName = String.Empty;
            this._scheme = new FzSchemeBLL();
            this._tuples = new List<FzTupleBLL>();
        }

        public FzRelationBLL(String relationName)
        {
            this._relationName = relationName;
            this._scheme = new FzSchemeBLL();
            this._tuples = new List<FzTupleBLL>();
        }

        public FzRelationBLL(FzRelationBLL old)
        {
            this._relationName = old._relationName;
            this._scheme = old._scheme;
            this._tuples = old._tuples;

        }
        #endregion

        #region 4. Methods 
        public static List<String> GetListRelationName(FzDbBLL fdb)
        {
            return FzRelationDAL.GetListRelationName(fdb);
        }

        public static FzRelationBLL GetRelationByName(String relationName, FzDbBLL fdb)
        {
            return FzRelationDAL.GetRelationByName(relationName, fdb);
        }
        #endregion

        #region 5. Privates (none)
        #endregion
    }
}
