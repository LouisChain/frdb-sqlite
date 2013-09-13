using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace N_tier
{
    public class FzDbBLL
    {
         #region 1. Fields

        private String _FzDbName;
        private String _FzDbPath;    
        private List<FzSchemeBLL> _schemes;
        private List<FzRelationBLL> _relations;
        private List<FzQueryBLL> _queries;
        private String _connString;
        private DataSet _dataSet;

        #endregion

        #region 2. Properties

        public String FzDbName
        {
            get { return _FzDbName; }
            set { _FzDbName = value; }
        }

        public String FzDbPath
        {
            get { return _FzDbPath; }
            set { _FzDbPath = value; }
        }

        public List<FzSchemeBLL> Schemes
        {
            get { return _schemes; }
            set { _schemes = value; }
        }

        public List<FzRelationBLL> Relations
        {
            get 
            {
                //List<FzRelationBLL> result = new List<FzRelationBLL>();
                //foreach (FzRelationBLL item in this._relations)
                //{
                //    FzRelationBLL tmp = new FzRelationBLL(item);
                //    result.Add(tmp);
                //}
                //return result;
                return _relations;
            }
            set 
            {
                foreach (FzRelationBLL item in value)
                {
                    FzRelationBLL tmp = new FzRelationBLL(item);
                    _relations.Add(tmp);
                }
                //_relations = value;
            }
        }

        public List<FzQueryBLL> Queries
        {
            get { return _queries; }
            set { _queries = value; }
        }

        public String ConnString
        {
            get { return _connString; }
            set { _connString = value; }
        }

        public DataSet DataSet
        {
            get { return _dataSet; }
            set { _dataSet = value; }
        }

        #endregion

        #region 3. Contructors

        public object Clone()//To copy to another area memory
        {
            return this.MemberwiseClone();
        }

        public FzDbBLL()
        {
            this._FzDbName = String.Empty;
            this._FzDbPath = String.Empty;
            this._schemes = new List<FzSchemeBLL>();
            this._relations = new List<FzRelationBLL>();
            this._queries = new List<FzQueryBLL>();
            this._connString = String.Empty;
            this._dataSet = new DataSet();
        }

        public FzDbBLL(String FzDbPath)
        {
            this._FzDbName = String.Empty;//Get the path for fuzzy db
            this._FzDbPath = FzDbPath;
            this._FzDbName = this.GetFzDbName(FzDbPath);
            this._connString = @"Data Source = " + FzDbPath + "; Version = 3;";
            this._schemes = new List<FzSchemeBLL>();
            this._relations = new List<FzRelationBLL>();
            this._queries = new List<FzQueryBLL>();
            this._dataSet = new DataSet();
        }

        #endregion

        #region 4. Methods  
        public static String GetRootPath(String path)
        {
            return FzDbDAL.GetRootPath(path);
        }

        public  Boolean CheckConnection()
        {
            return new FzDbDAL().CheckConnection(this);
        }

        public bool CreateBlankDatabaseTbb()
        {
            return new FzDbDAL().CreateBlankFdb(this);
        }

        public bool CreateFuzzyDatabase()
        {
            return new FzDbDAL().CreateFuzzyDatabase(this);
        }

        public bool OpenFuzzyDatabase()
        {
            return new FzDbDAL().OpenFuzzyDatabase(this);
        }

        public void DropFuzzyDatabase()
        {
            new FzDbDAL().DropFuzzyDatabase(this);
        }

        public bool SaveFuzzyDatabaseAs()
        {
            return new FzDbDAL().SaveFuzzyDatabaseAs(this);
        }

        public bool SaveFuzzyDatabase()
        {
            return new FzDbDAL().SaveFuzzyDatabase(this);
        }
        #endregion

        #region 5. Privates

        public String GetFzDbName(String FzDbPath)
        {
            String FzDbName = String.Empty;

            //Get FzDbName from the source path
            for (int i = FzDbPath.Length - 1; i >= 0; i-- )
			{
                if (FzDbPath[i] == '\\')
                    break;
                else
                    FzDbName = FzDbPath[i] + FzDbName;
			}

            //Cut extention "."
            for (int i = FzDbName.Length - 1; i >= 0; i--)
            {
                if (FzDbName[i] == '.')
                {
                    FzDbName = FzDbName.Remove(i);
                    break;
                }
            }

            return FzDbName;
        }



        #endregion
    }
}
