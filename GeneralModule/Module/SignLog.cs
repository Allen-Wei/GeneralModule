using System;
using System.Web;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using GeneralModule.Utilily.Extension;
using GeneralModule.Utilily.LinqDynamic;

namespace GeneralModule.Module
{



    partial class GMContext
    {


        #region 可扩展性方法定义
        partial void InsertSignLog(SignLog instance);
        partial void UpdateSignLog(SignLog instance);
        partial void DeleteSignLog(SignLog instance);
        #endregion


        public System.Data.Linq.Table<SignLog> SignLogs
        {
            get
            {
                return this.GetTable<SignLog>();
            }
        }

    }

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.SignLog")]
    public partial class SignLog : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private long _ID;

        private System.Nullable<int> _UserID;

        private string _UserCode;

        private string _UserType;

        private string _LogID;

        private System.DateTime _SignDate;

        private bool _IsValid;

        private string _Via;

        private string _RefValue;

        private int _RemeberHours;

        private System.Nullable<System.DateTime> _ExpireDate;

        private string _Note1;

        private string _Note2;

        private string _Note3;

        private string _Note4;

        private string _Note5;

        #region 可扩展性方法定义
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIDChanging(long value);
        partial void OnIDChanged();
        partial void OnUserIDChanging(System.Nullable<int> value);
        partial void OnUserIDChanged();
        partial void OnUserCodeChanging(string value);
        partial void OnUserCodeChanged();
        partial void OnUserTypeChanging(string value);
        partial void OnUserTypeChanged();
        partial void OnLogIDChanging(string value);
        partial void OnLogIDChanged();
        partial void OnSignDateChanging(System.DateTime value);
        partial void OnSignDateChanged();
        partial void OnIsValidChanging(bool value);
        partial void OnIsValidChanged();
        partial void OnViaChanging(string value);
        partial void OnViaChanged();
        partial void OnRefValueChanging(string value);
        partial void OnRefValueChanged();
        partial void OnRemeberHoursChanging(int value);
        partial void OnRemeberHoursChanged();
        partial void OnExpireDateChanging(System.Nullable<System.DateTime> value);
        partial void OnExpireDateChanged();
        partial void OnNote1Changing(string value);
        partial void OnNote1Changed();
        partial void OnNote2Changing(string value);
        partial void OnNote2Changed();
        partial void OnNote3Changing(string value);
        partial void OnNote3Changed();
        partial void OnNote4Changing(string value);
        partial void OnNote4Changed();
        partial void OnNote5Changing(string value);
        partial void OnNote5Changed();
        #endregion

        public SignLog()
        {
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ID", AutoSync = AutoSync.OnInsert, DbType = "BigInt NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public long ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                if ((this._ID != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._ID = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UserID", DbType = "Int")]
        public System.Nullable<int> UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                if ((this._UserID != value))
                {
                    this.OnUserIDChanging(value);
                    this.SendPropertyChanging();
                    this._UserID = value;
                    this.SendPropertyChanged("UserID");
                    this.OnUserIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UserCode", DbType = "VarChar(MAX)", UpdateCheck = UpdateCheck.Never)]
        public string UserCode
        {
            get
            {
                return this._UserCode;
            }
            set
            {
                if ((this._UserCode != value))
                {
                    this.OnUserCodeChanging(value);
                    this.SendPropertyChanging();
                    this._UserCode = value;
                    this.SendPropertyChanged("UserCode");
                    this.OnUserCodeChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UserType", DbType = "VarChar(1000)")]
        public string UserType
        {
            get
            {
                return this._UserType;
            }
            set
            {
                if ((this._UserType != value))
                {
                    this.OnUserTypeChanging(value);
                    this.SendPropertyChanging();
                    this._UserType = value;
                    this.SendPropertyChanged("UserType");
                    this.OnUserTypeChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LogID", DbType = "VarChar(100) NOT NULL", CanBeNull = false)]
        public string LogID
        {
            get
            {
                return this._LogID;
            }
            set
            {
                if ((this._LogID != value))
                {
                    this.OnLogIDChanging(value);
                    this.SendPropertyChanging();
                    this._LogID = value;
                    this.SendPropertyChanged("LogID");
                    this.OnLogIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SignDate", DbType = "DateTime NOT NULL")]
        public System.DateTime SignDate
        {
            get
            {
                return this._SignDate;
            }
            set
            {
                if ((this._SignDate != value))
                {
                    this.OnSignDateChanging(value);
                    this.SendPropertyChanging();
                    this._SignDate = value;
                    this.SendPropertyChanged("SignDate");
                    this.OnSignDateChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsValid", DbType = "Bit NOT NULL")]
        public bool IsValid
        {
            get
            {
                return this._IsValid;
            }
            set
            {
                if ((this._IsValid != value))
                {
                    this.OnIsValidChanging(value);
                    this.SendPropertyChanging();
                    this._IsValid = value;
                    this.SendPropertyChanged("IsValid");
                    this.OnIsValidChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Via", DbType = "VarChar(MAX)", UpdateCheck = UpdateCheck.Never)]
        public string Via
        {
            get
            {
                return this._Via;
            }
            set
            {
                if ((this._Via != value))
                {
                    this.OnViaChanging(value);
                    this.SendPropertyChanging();
                    this._Via = value;
                    this.SendPropertyChanged("Via");
                    this.OnViaChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RefValue", DbType = "VarChar(MAX)", UpdateCheck = UpdateCheck.Never)]
        public string RefValue
        {
            get
            {
                return this._RefValue;
            }
            set
            {
                if ((this._RefValue != value))
                {
                    this.OnRefValueChanging(value);
                    this.SendPropertyChanging();
                    this._RefValue = value;
                    this.SendPropertyChanged("RefValue");
                    this.OnRefValueChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RemeberHours", DbType = "Int NOT NULL")]
        public int RemeberHours
        {
            get
            {
                return this._RemeberHours;
            }
            set
            {
                if ((this._RemeberHours != value))
                {
                    this.OnRemeberHoursChanging(value);
                    this.SendPropertyChanging();
                    this._RemeberHours = value;
                    this.SendPropertyChanged("RemeberHours");
                    this.OnRemeberHoursChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ExpireDate", AutoSync = AutoSync.Always, DbType = "DateTime", IsDbGenerated = true, UpdateCheck = UpdateCheck.Never, Expression = "(dateadd(hour,[RemeberHOurs],[SignDate]))")]
        public System.Nullable<System.DateTime> ExpireDate
        {
            get
            {
                return this._ExpireDate;
            }
            set
            {
                if ((this._ExpireDate != value))
                {
                    this.OnExpireDateChanging(value);
                    this.SendPropertyChanging();
                    this._ExpireDate = value;
                    this.SendPropertyChanged("ExpireDate");
                    this.OnExpireDateChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Note1", DbType = "VarChar(MAX)", UpdateCheck = UpdateCheck.Never)]
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Note2", DbType = "VarChar(MAX)", UpdateCheck = UpdateCheck.Never)]
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Note3", DbType = "VarChar(MAX)", UpdateCheck = UpdateCheck.Never)]
        public string Note3
        {
            get
            {
                return this._Note3;
            }
            set
            {
                if ((this._Note3 != value))
                {
                    this.OnNote3Changing(value);
                    this.SendPropertyChanging();
                    this._Note3 = value;
                    this.SendPropertyChanged("Note3");
                    this.OnNote3Changed();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Note4", DbType = "VarChar(MAX)", UpdateCheck = UpdateCheck.Never)]
        public string Note4
        {
            get
            {
                return this._Note4;
            }
            set
            {
                if ((this._Note4 != value))
                {
                    this.OnNote4Changing(value);
                    this.SendPropertyChanging();
                    this._Note4 = value;
                    this.SendPropertyChanged("Note4");
                    this.OnNote4Changed();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Note5", DbType = "VarChar(MAX)", UpdateCheck = UpdateCheck.Never)]
        public string Note5
        {
            get
            {
                return this._Note5;
            }
            set
            {
                if ((this._Note5 != value))
                {
                    this.OnNote5Changing(value);
                    this.SendPropertyChanging();
                    this._Note5 = value;
                    this.SendPropertyChanged("Note5");
                    this.OnNote5Changed();
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

    partial class SignLog
    {
        public static string CONNECTION
        {
            get
            {
                return ModuleInfo.GetModuleDataPosition("SignLog");
            }
        }
        //构造函数是用来赋值的
        //public static [return type] Initial用来从数据库初始化
        private SignLog(object user)
        {
            if (user.GetType().IsValueType) this.UserID = user.eToInt(-1);
            else this.UserCode = user.ToString();

        }

        public SignLog(
            object user,
            string uType,
            string via,
            string refValue,
            bool isValid,
            int stayHours,
            string logId,
            DateTime signDate)
            : this(user)
        {
            this.UserType = uType;
            this.Via = via;
            this.RefValue = refValue;
            this.IsValid = isValid;
            this.RemeberHours = stayHours;
            this.LogID = logId;
            this.SignDate = signDate;
        }

        public SignLog(object user, string uType, string via) : this(user, uType, via, "", true, 24 * 7, Guid.NewGuid().ToString(), DateTime.Now) { }
        public static SignLog InitialThis(int id, string cnString = null)
        {
            GMContext db = new GMContext(cnString.eIfEmpty(CONNECTION));
            return db.SignLogs.FirstOrDefault(sl => sl.ID == id);
        }
        public static List<SignLog> Initial(int userID, string cnString = null)
        {
            GMContext db = new GMContext(cnString.eIfEmpty(CONNECTION));
            return db.SignLogs.Where(sl => sl.UserID == userID).ToList();
        }
        public static List<SignLog> Initial(string userCode, string cnString = null)
        {
            GMContext db = new GMContext(cnString.eIfEmpty(CONNECTION));
            return db.SignLogs.Where(sl => sl.UserCode == userCode).ToList();
        }

        public bool Insert(string cnString = null)
        {
            if (this.UserCode.eIsEmpty() && this.UserID == null) return false;

            GMContext db = new GMContext(cnString.eIfEmpty(CONNECTION));
            db.SignLogs.InsertOnSubmit(this);
            db.SubmitChanges();
            return true;
        }
        public static SignLog Insert(object user, string userType, string via, string cnString = null)
        {
            SignLog log = new SignLog(user, userType, via);
            bool loged = log.Insert(cnString);
            return loged ? log : null;
        }

        public static SignLog Insert(
            object user,
            string userType,
            string via,
            string refValue,
            bool valid,
            int stayHours,
            string logId,
            DateTime signDate,
            string cnString = "")
        {
            SignLog log = new SignLog(user, userType, via, refValue, valid, stayHours, logId, signDate);
            bool loged = log.Insert(cnString);
            return loged ? log : null;
        }

        public bool RemoveThis(string cnString = null)
        {
            if (this.ID.eIsNull()) return false;
            GMContext db = new GMContext(cnString.eIfEmpty(CONNECTION));
            SignLog log = db.SignLogs.FirstOrDefault(sl => sl.ID == this.ID);
            db.SignLogs.DeleteOnSubmit(log);
            db.SubmitChanges();
            return true;
        }
        public bool Remove(string cnString = null)
        {
            if (this.UserCode.eIsEmpty() && this.UserID == null) return false;

            GMContext db = new GMContext(cnString.eIfEmpty(CONNECTION));
            List<SignLog> logs = db.SignLogs.Where(sl => sl.UserID == this.UserID || sl.UserCode == this.UserCode).ToList();
            db.SignLogs.DeleteAllOnSubmit(logs);
            db.SubmitChanges();
            return true;
        }

        public static bool Remove(object user, string cnString = null)
        {
            SignLog log = new SignLog(user);
            return log.Remove(cnString);
        }
        public bool SetInvalid(string cnString = null)
        {
            GMContext db = new GMContext(cnString.eIfNull(CONNECTION));
            SignLog log = db.SignLogs.FirstOrDefault(sl => sl.ID == this.ID);
            if (log.eIsNull()) return false;
            log.IsValid = false;
            db.SubmitChanges();
            return true;
        }

        public bool SetValid(int addHours, string cnString = null)
        {
            GMContext db = new GMContext(cnString.eIfNull(CONNECTION));
            SignLog log = db.SignLogs.FirstOrDefault(sl => sl.ID == this.ID);
            if (log.eIsNull()) return false;
            log.IsValid = true;
            DateTime now = DateTime.Now;
            TimeSpan span = now - log.SignDate;
            int onlineHours = span.Hours;
            log.RemeberHours = addHours + onlineHours;
            db.SubmitChanges();
            return true;
        }
        public bool SetValidAddHoursWhenExpire(int addHours, string cnString = null)
        {
            GMContext db = new GMContext(cnString.eIfNull(CONNECTION));
            SignLog log = db.SignLogs.FirstOrDefault(sl => sl.ID == this.ID);
            if (log.eIsNull()) return false;
            log.IsValid = true;
            DateTime now = DateTime.Now;
            TimeSpan span = now - log.SignDate;
            int onlineHours = span.Hours;
            if (span.Hours >= log.RemeberHours) log.RemeberHours = addHours + onlineHours;
            db.SubmitChanges();
            return true;
        }



        public static SignLog InitialByLogID(string logId, bool cache = false, string cnString = null)
        {
            if (cache)
            {
                object cacheObject = GeneralModule.Utilily.WebTool.GetCache("Alan-Module-SignLog-" + logId);
                if (!cacheObject.eIsNull()) return (SignLog)cacheObject;
            }
            GMContext db = new GMContext(cnString.eIfNull(CONNECTION));
            SignLog log = db.SignLogs.FirstOrDefault(sl => sl.LogID == logId && sl.IsValid && sl.ExpireDate > DateTime.Now);
            if (cache && !log.eIsNull())
            {
                GeneralModule.Utilily.WebTool.SetCache("Alan-Module-SignLog-" + logId, log);
            }
            return log;
        }
        public static bool IsOnline(string logId, bool cache, string cnString = null)
        {
            SignLog log = InitialByLogID(logId, cache, cnString);
            return log.eIsNull();
        }

        public static bool IsUserType(string logId, string uType, string cnString = null)
        {
            SignLog log = InitialByLogID(logId);
            if (log.eIsNull()) return false;
            return log.UserType == uType;
        }
    }
}
