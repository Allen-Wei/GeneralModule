using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using GeneralModule.Utilily.Extension;
using GMU = GeneralModule.Utilily;

namespace GeneralModule.Module
{

    public partial class GMContext
    {
        #region Extensibility Method Definitions

        partial void InsertSEO(SEO instance);
        partial void UpdateSEO(SEO instance);
        partial void DeleteSEO(SEO instance);
        #endregion

        #region Table Class
        public System.Data.Linq.Table<SEO> SEO
        {
            get
            {
                return this.GetTable<SEO>();
            }
        }
        #endregion
    }


    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.SEO")]
    public partial class SEO : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private long _Id;

        private string _Url;

        private bool _IsRegex;

        private string _Title;

        private string _MetaName;

        private string _MetaContent;

        private string _MetaHttpEquiv;

        private string _Note1;

        private string _Note2;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        partial void OnUrlChanging(string value);
        partial void OnUrlChanged();
        partial void OnIsRegexChanging(bool value);
        partial void OnIsRegexChanged();
        partial void OnTitleChanging(string value);
        partial void OnTitleChanged();
        partial void OnMetaNameChanging(string value);
        partial void OnMetaNameChanged();
        partial void OnMetaContentChanging(string value);
        partial void OnMetaContentChanged();
        partial void OnMetaHttpEquivChanging(string value);
        partial void OnMetaHttpEquivChanged();
        partial void OnNote1Changing(string value);
        partial void OnNote1Changed();
        partial void OnNote2Changing(string value);
        partial void OnNote2Changed();
        #endregion

        public SEO()
        {
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "id", Storage = "_Id", AutoSync = AutoSync.OnInsert, DbType = "BigInt NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if ((this._Id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "url", Storage = "_Url", DbType = "VarChar(2000) NOT NULL", CanBeNull = false)]
        public string Url
        {
            get
            {
                return this._Url;
            }
            set
            {
                if ((this._Url != value))
                {
                    this.OnUrlChanging(value);
                    this.SendPropertyChanging();
                    this._Url = value;
                    this.SendPropertyChanged("Url");
                    this.OnUrlChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "isRegex", Storage = "_IsRegex", DbType = "Bit NOT NULL")]
        public bool IsRegex
        {
            get
            {
                return this._IsRegex;
            }
            set
            {
                if ((this._IsRegex != value))
                {
                    this.OnIsRegexChanging(value);
                    this.SendPropertyChanging();
                    this._IsRegex = value;
                    this.SendPropertyChanged("IsRegex");
                    this.OnIsRegexChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "title", Storage = "_Title", DbType = "VarChar(8000)")]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if ((this._Title != value))
                {
                    this.OnTitleChanging(value);
                    this.SendPropertyChanging();
                    this._Title = value;
                    this.SendPropertyChanged("Title");
                    this.OnTitleChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "metaName", Storage = "_MetaName", DbType = "VarChar(8000)")]
        public string MetaName
        {
            get
            {
                return this._MetaName;
            }
            set
            {
                if ((this._MetaName != value))
                {
                    this.OnMetaNameChanging(value);
                    this.SendPropertyChanging();
                    this._MetaName = value;
                    this.SendPropertyChanged("MetaName");
                    this.OnMetaNameChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "metaContent", Storage = "_MetaContent", DbType = "VarChar(8000)")]
        public string MetaContent
        {
            get
            {
                return this._MetaContent;
            }
            set
            {
                if ((this._MetaContent != value))
                {
                    this.OnMetaContentChanging(value);
                    this.SendPropertyChanging();
                    this._MetaContent = value;
                    this.SendPropertyChanged("MetaContent");
                    this.OnMetaContentChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "metaHttpEquiv", Storage = "_MetaHttpEquiv", DbType = "VarChar(8000)")]
        public string MetaHttpEquiv
        {
            get
            {
                return this._MetaHttpEquiv;
            }
            set
            {
                if ((this._MetaHttpEquiv != value))
                {
                    this.OnMetaHttpEquivChanging(value);
                    this.SendPropertyChanging();
                    this._MetaHttpEquiv = value;
                    this.SendPropertyChanged("MetaHttpEquiv");
                    this.OnMetaHttpEquivChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "note1", Storage = "_Note1", DbType = "VarChar(8000)")]
        public string Note1
        {
            get
            {
                return this._Note1;
            }
            set
            {
                if ((this._Note1 != value))
                {
                    this.OnNote1Changing(value);
                    this.SendPropertyChanging();
                    this._Note1 = value;
                    this.SendPropertyChanged("Note1");
                    this.OnNote1Changed();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "note2", Storage = "_Note2", DbType = "NText", UpdateCheck = UpdateCheck.Never)]
        public string Note2
        {
            get
            {
                return this._Note2;
            }
            set
            {
                if ((this._Note2 != value))
                {
                    this.OnNote2Changing(value);
                    this.SendPropertyChanging();
                    this._Note2 = value;
                    this.SendPropertyChanged("Note2");
                    this.OnNote2Changed();
                }
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public partial class SEO
    {
        public static string DBConnection = String.Empty;
        public SEO(string connectionString)
        {
            SEO.DBConnection = connectionString;
        }

        public static List<SEO> LoadAll(string dbConnection = "")
        {
            string strConnection = SEO.DBConnection;
            if (!String.IsNullOrWhiteSpace(dbConnection))
            {
                strConnection = dbConnection;
            }
            GMContext db = new GMContext(strConnection);
            return db.SEO.ToList();

        }
        public static List<SEO> LoadAll(TimeSpan time, string cnString = "")
        {
            List<SEO> seos = GMU.WebTool.GetCache("SEO-List") as List<SEO>;
            if (seos == null)
            {
                seos = SEO.LoadAll(cnString);
                GMU.WebTool.SetCache("SEO-List", seos, time);
                return seos;
            }
            else
            {
                return seos;
            }
        }

        public static void AddHeader(List<SEO> seos, Page p)
        {
            string url = GMU.WebTool.req.RawUrl;
            foreach (SEO s in seos)
            {
                HtmlMeta meta = new HtmlMeta();
                if (!String.IsNullOrWhiteSpace(s.MetaName))
                {
                    meta.Name = s.MetaName;
                }
                if (!String.IsNullOrWhiteSpace(s.MetaHttpEquiv))
                {
                    meta.HttpEquiv = s.MetaHttpEquiv;
                }
                if (!String.IsNullOrWhiteSpace(s.MetaName))
                {
                    meta.Content = s.MetaContent;
                }

                if (s.IsRegex)
                {
                    Regex reg = new Regex(s.Url);
                    if (reg.IsMatch(url))
                    {
                        p.Header.Controls.Add(meta);
                        if (!String.IsNullOrWhiteSpace(s.Title))
                        {
                            p.Title = s.Title;
                        }
                    }
                }
                else
                {
                    string trimURL = url.TrimStart('/');
                    if (s.Url.ToLower() == url.ToLower() || s.Url.ToLower() == trimURL.ToLower())
                    {
                        p.Header.Controls.Add(meta);
                        if (!String.IsNullOrWhiteSpace(s.Title))
                        {
                            p.Title = s.Title;
                        }
                    }
                }
            }
        }

        public static void AddHeader(string connection, string url, Page p)
        {
            var headers = GetHeader(connection, url);
            headers.ForEach(h =>
            {
                var meta = new HtmlMeta();
                if (!h.MetaName.eIsSpace())
                {
                    meta.Name = h.MetaName;
                }
                if (!h.MetaHttpEquiv.eIsSpace())
                {
                    meta.HttpEquiv = h.MetaHttpEquiv;
                }
                if (!h.MetaContent.eIsSpace())
                {
                    meta.Content = h.MetaContent;
                }
                p.Header.Controls.Add(meta);
                p.Title = h.Title;
            });
        }

        public static List<SEO> GetHeader(string connection, string url)
        {
            var db = new GMContext(connection);
            return db.SEO.Where(s => s.Url.ToLower() == url.ToLower()).ToList();
        }

        public bool Insert()
        {
            if (String.IsNullOrWhiteSpace(SEO.DBConnection) || String.IsNullOrWhiteSpace(this.Url))
            {
                return false;
            }
            GMContext db = new GMContext(SEO.DBConnection);
            db.SEO.InsertOnSubmit(this);
            db.SubmitChanges();
            return true;
        }

        public bool Delete()
        {
            if (String.IsNullOrWhiteSpace(SEO.DBConnection) || String.IsNullOrWhiteSpace(this.Url))
            {
                return false;
            }
            GMContext db = new GMContext(SEO.DBConnection);
            var seos = from q in db.SEO
                       where q.Url == this.Url
                       select q;
            if (seos == null)
            {
                return true;
            }
            db.SEO.DeleteAllOnSubmit(seos);
            db.SubmitChanges();
            return true;
        }

        public bool IsURLExist()
        {
            if (String.IsNullOrWhiteSpace(SEO.DBConnection) || String.IsNullOrWhiteSpace(this.Url))
            {
                return false;
            }
            GMContext db = new GMContext(SEO.DBConnection);
            int count = db.SEO.Where(s => s.Url == this.Url).Count();
            if (count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Update(string url, string title, string description, string keyWords, string dbConnection = "")
        {
            string strConnection = SEO.DBConnection;
            if (!String.IsNullOrWhiteSpace(dbConnection))
            {
                strConnection = dbConnection;
            }

            if (String.IsNullOrWhiteSpace(strConnection) || String.IsNullOrWhiteSpace(url))
            {
                return false;
            }
            GMContext db = new GMContext(SEO.DBConnection);
            var query = (from s in db.SEO
                         where s.Url == url
                         select s).ToList();

            foreach (var q in query)
            {
                if (q.MetaName == "description")
                {
                    q.MetaContent = description;
                }
                if (q.MetaName == "keywords")
                {
                    q.MetaContent = keyWords;
                }
                q.Title = title;
            }
            db.SubmitChanges();
            return true;
        }
        public static bool Insert(string url, string title, string description, string keyWrods, string dbConnection = "")
        {
            string strConnection = SEO.DBConnection;
            if (!String.IsNullOrWhiteSpace(dbConnection))
            {
                strConnection = dbConnection;
            }
            if (String.IsNullOrWhiteSpace(strConnection))
            {
                return false;
            }
            GMContext db = new GMContext(strConnection);
            SEO sDesc = new SEO();
            sDesc.Url = url;
            sDesc.Title = title;
            sDesc.MetaName = "description";
            sDesc.MetaContent = description;
            db.SEO.InsertOnSubmit(sDesc);
            SEO sKey = new SEO();
            sKey.Url = url;
            sKey.Title = title;
            sKey.MetaName = "keywords";
            sKey.MetaContent = keyWrods;
            db.SEO.InsertOnSubmit(sKey);
            db.SubmitChanges();
            return true;
        }
        public override string ToString()
        {
            return GeneralModule.Utilily.Utilily.Serialize(this);
        }
    }
}