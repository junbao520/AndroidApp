using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Foundation.Providers;
using TwoPole.Chameleon3.Infrastructure.Map;

namespace TwoPole.Chameleon3.Infrastructure.Services
{
    public class DataService : SQLiteOpenHelper, IDataService
    {
        private const string GlobalSettingsGroupName = "GlobalSettings";

        //默认的新添加的配置 的ID
        private const int DefaultNewSettingID = -2;

        private List<KeyValuePair<string, string>> lstReplaceExamItem = new List<KeyValuePair<string, string>>() {
            new KeyValuePair<string, string>("公共汽车站","公交车站")
        };


        //PRAGMA user_version=1
        //PRAGMA usr
        #region TODO:直接通过静态变量 缓存部分读取数据库操作 以后待成熟之后再进行使用缓存机制
        public static GlobalSettings settings = null;
        public static GlobalSettings tempsettings = null;
        public static LightExamItem[] lightExamItems = null;
        public static LightRule[] lightRules = null;
        public static ExamItem[] examItems = null;
        public static DeductionRule[] deductionRules = null;
        public static Setting[] allSettings = null;
        //public static IList<ISettings> 
        //地图缓存
        public static IList<MapLine> lstMaplineCache = null;


        #endregion


        #region Public Properties

        //static readonly string DB_Name=Android.OS.Environment.ExternalStorageDirectory+"/conntect

        //static readonly string DB_NAME = Android.OS.Environment.ExternalStorageDirectory + "/content-chengdu-longquan.db";

        static readonly int version = 1; //数据库版本
        /**********************,****************************/
        public DataService(Context context, string DataBaseName) : base(context, DataBaseName, null, version)
        {
            //从这里面取FolderNamer
            this.FolderName = DataBaseName.Split('-')[2].Substring(0, DataBaseName.Split('-')[2].Length - 3);
        }
        public string FolderName { get; set; }
        public override void OnCreate(SQLiteDatabase db)
        {

        }

        /// <summary>
        /// 更新新规扣分规则
        /// </summary>
        public bool UpdateNewRules()
        {
            try
            {
                SQLiteDatabase db = CurrentDataBase;
                string sql = "update DeductionRule set DeductedScores = 100 where RuleName like '%转向灯%'";
                db.ExecSQL(sql);
                sql = "update DeductionRule  set RuleName = '掉头前未开启左转向灯', DeductedReason = '掉头前未开启左转向灯', voiceText = '掉头前未开启左转向灯' where RuleCode = '41503'";
                db.ExecSQL(sql);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public void Cache2()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var TempDeductionRules = AllDeductionRules;
            //缓存灯光分组
            var TempLightGroups = AllLightExamItems;
            //缓存灯光模拟规则
            var TempLightRules = AllLightRules;
            sw.Stop();
            LogManager.WriteSystemLog("InitCache2" + sw.Elapsed.TotalMilliseconds.ToString());
        }
        //多线程进行缓存
        public void Cache()
        {

            //考试项目缓存
            var TempAllExamItems = AllExamItems;
            //配置缓存
            var TempGetSettings = GetSettings();
            //地图缓存
            var TempMaps = ALLMapLines;

            var TempDeductionRules = AllDeductionRules;
            //缓存灯光分组
            var TempLightGroups = AllLightExamItems;
            //缓存灯光模拟规则
            var TempLightRules = AllLightRules;

        }


        public override void OnOpen(SQLiteDatabase db)
        {
            base.OnOpen(db);
        }
        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {

        }

        /// <summary>
        /// 这个是在数据库重新读取的
        /// </summary>
        public Setting[] AllSettings
        {
            get
            {
                //使用静态变量进行缓存
                if (allSettings == null)
                {
                    allSettings = GetAllSetting();
                }
                return allSettings;
            }
        }
        private TSettings BuildSettings<TSettings>(TSettings settings, IList<Setting> settingItems, string parentKey = "")
        where TSettings : class, ISettings, new()
        {
            parentKey = parentKey ?? string.Empty;
            settings = settings ?? new TSettings();
            var properties = from prop in typeof(TSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             where prop.CanWrite && prop.CanRead
                             where prop.GetGetMethod(true).GetParameters().Length == 0
                             //where !prop.PropertyType.IsClass || typeof(ISettings).IsAssignableFrom(prop.PropertyType)
                             where settingItems.Any(d => d.Key.StartsWith(parentKey, StringComparison.OrdinalIgnoreCase))
                             select prop;

            foreach (var property in properties)
            {
                string key = parentKey + property.Name;
                //if (typeof(ISettings).IsAssignableFrom(property.PropertyType))
                //{
                //    var method = typeof(ConfigurationProvider).GetMethod("BuildSettings", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(property.PropertyType);
                //    var value = method.Invoke(this, new object[] { property.GetValue(settings), settingItems, key + "." });
                //    property.SetValue(settings, value);
                //}
                //else
                //{
                var setting = settingItems.FirstOrDefault(d => string.Equals(d.Key, key, StringComparison.OrdinalIgnoreCase));
                if (setting != null)
                {
                    var converter = TypeDescriptor.GetConverter(property.PropertyType);
                    if (converter.CanConvertFrom(typeof(string)) && converter.IsValid(setting.Value))
                    {
                        var value = converter.ConvertFromInvariantString(setting.Value);
                        property.SetValue(settings, value);
                    }
                }
                //}
            }

            return settings;
        }

        private void BuildPropertySettings<TSettings>(TSettings settings, ICollection<Setting> settingItems, string groupName, string parentKey = "")
        where TSettings : class, ISettings, new()
        {
            var properties = from prop in typeof(TSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             where prop.CanWrite && prop.CanRead
                             where prop.GetGetMethod(true).GetParameters().Length == 0
                             // where prop.PropertyType == typeof(string) || !prop.PropertyType.IsClass || typeof(ISettings).IsAssignableFrom(prop.PropertyType)
                             select prop;

            foreach (var prop in properties)
            {
                string key = parentKey + prop.Name;
                if (typeof(ISettings).IsAssignableFrom(prop.PropertyType))
                {
                    var method = typeof(DataService).GetMethod("BuildPropertySettings", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(prop.PropertyType);
                    method.Invoke(this, new object[] { prop.GetValue(settings), settingItems, groupName, key + "." });
                }
                else
                {
                    var converter = TypeDescriptor.GetConverter(prop.PropertyType);
                    if (converter.CanConvertFrom(typeof(string)))
                    {
                        //  dynamic value = prop.GetValue(settings, null); mono无法支持 dynamic 关键字
                        object value = prop.GetValue(settings, null);
                        var valueStr = value == null ? string.Empty : converter.ConvertToInvariantString(value);
                        var settingItem = settingItems.FirstOrDefault(d =>
                            string.Equals(groupName, d.GroupName, StringComparison.OrdinalIgnoreCase) && string.Equals(d.Key, key, StringComparison.OrdinalIgnoreCase));
                        if (settingItem == null)
                        {
                            settingItem = new Setting();
                            settingItem.Id = DefaultNewSettingID;
                            settingItem.GroupName = groupName;
                            settingItem.Key = key;
                            settingItem.Value = valueStr;
                            settingItems.Add(settingItem);
                        }
                        else
                        {
                            settingItem.Value = valueStr;
                        }
                    }
                }
            }
        }

        public IList<Setting> GetListSettingIDByKey(IList<Setting> settingItems, string groupName)
        {

            List<Setting> lstTempSetting = new List<Setting>();
            foreach (var item in settingItems)
            {
                var settingItem = AllSettings.FirstOrDefault(d =>
                       string.Equals(groupName, d.GroupName, StringComparison.OrdinalIgnoreCase) && string.Equals(d.Key, item.Key, StringComparison.OrdinalIgnoreCase));

                if (settingItem == null)
                {
                    settingItem = new Setting();
                    settingItem.Id = DefaultNewSettingID;
                    settingItem.GroupName = groupName;
                    settingItem.Key = item.Key;
                    settingItem.Value = item.Value;
                    //settingItems.Add(settingItem);
                }
                else
                {
                    settingItem.Key = item.Key;
                    settingItem.GroupName = item.GroupName;
                    settingItem.Value = item.Value;
                }
                lstTempSetting.Add(settingItem);
            }
            return lstTempSetting;
        }


        public void UpdateCache(string key, object Value)
        {

        }

        public void RemoveCache(string key)
        {

        }

        public ExamItem[] AllExamItems
        {
            get
            {

                if (examItems == null)
                {
                    examItems = GetAllExamItems();
                }

                return examItems;
            }
        }
        public List<ExamItem> GetExamItemsList(bool IsContainCommonExamItem = false)
        {
            List<ExamItem> lstExamItem = new List<ExamItem>();
            var examItems = AllExamItems
                  .Where(x => x.IsEnable && (x.ItemCode != ExamItemCodes.CommonExamItem || IsContainCommonExamItem))
                  .OrderBy(x => x.SequenceNumber).ToList();

            //todo：这些好像没有用
            foreach (var item in examItems)
            {

                if (lstReplaceExamItem.Where(s => s.Key == item.ItemName).Count() > 0)
                {
                    item.ItemName = lstReplaceExamItem.Where(s => s.Key == item.ItemName).FirstOrDefault().Value;
                }
                lstExamItem.Add(item);
            }
            return lstExamItem;
        }
        //考试项目名称替换
        public string GetExamItemCode(string ItemName)
        {

            //if (lstReplaceExamItem.Where(s => s.Value==ItemName).Count() > 0)
            //{
            //    ItemName = lstReplaceExamItem.Where(s => s.Value== ItemName).FirstOrDefault().Key;
            //}
            if (ItemName.Contains("车站") || ItemName.Contains("公交"))
            {
                return ExamItemCodes.BusArea;
            }
            if (ItemName.Contains("综合"))
            {
                return ExamItemCodes.CommonExamItem;
            }
            //公共汽车站，公交车站都是一个(都叫公交车站)

            var ItemCoude = AllExamItems.Where(s => s.ItemName == ItemName).FirstOrDefault().ItemCode;
            return ItemCoude;
        }


        public Setting[] GetAllSetting()
        {
            SQLiteDatabase db = CurrentDataBase;
            ICursor c = db.Query("Setting", null, null, null, null, null, null, null);
            Setting[] settings = new Setting[16];
            List<Setting> lstSetting = new List<Setting>();
            Setting setting = null;
            while (c.MoveToNext())
            {
                setting = new Setting();
                setting.Id = c.GetInt(c.GetColumnIndex("Id"));
                setting.Key = c.GetString(c.GetColumnIndex("Key")).Trim();
                setting.Value = c.GetString(c.GetColumnIndex("Value")).Trim();
                setting.GroupName = c.GetString(c.GetColumnIndex("GroupName"));
                lstSetting.Add(setting);
            }
            settings = new Setting[lstSetting.Count];
            for (int i = 0; i < lstSetting.Count; i++)
            {
                settings[i] = lstSetting[i];
            }
            return settings;
        }
        public IList<Setting> GetISetting()
        {
            SQLiteDatabase db = CurrentDataBase;
            ICursor c = db.Query("Setting", null, null, null, null, null, null, null);
            Setting[] settings = new Setting[16];
            List<Setting> lstSetting = new List<Setting>();
            Setting setting = null;
            while (c.MoveToNext())
            {
                setting = new Setting();
                setting.Id = c.GetInt(c.GetColumnIndex("Id"));
                setting.Key = c.GetString(c.GetColumnIndex("Key"));
                setting.Value = c.GetString(c.GetColumnIndex("Value"));
                setting.GroupName = c.GetString(c.GetColumnIndex("GroupName"));
                lstSetting.Add(setting);
            }
            return lstSetting;
        }


        public bool UpdateExamItemsVoice(List<ExamItem> lstExamItem)
        {
            SQLiteDatabase db = CurrentDataBase;
            bool Result = true;
            try
            {
                db.BeginTransaction();
                //保存地图
                ContentValues cValue = new ContentValues();


                foreach (var item in lstExamItem)
                {
                    cValue = new ContentValues();
                    cValue.Put("VoiceText", item.VoiceText);
                    cValue.Put("EndVoiceText", item.EndVoiceText);
                    //不需要更新VoiceFile 保持对原来版本的一个兼容性
                    //cValue.Put("VoiceFile", item.VoiceText);
                    // cValue.Put("EndVoiceFile", item.EndVoiceText);
                    //执行数据库更新次数太多，应该是可以进行批量更新操作
                    db.Update("ExamItem", cValue, "ItemCode=?", new String[] { item.ItemCode });
                    //if (db.Update("ExamItem", cValue, "ItemCode=?", new String[] { item.ItemCode }) < 0)
                    //{
                    //    Result = false;
                    //    break;
                    //}
                }
                if (Result)
                {
                    //进行事物提交
                    db.SetTransactionSuccessful();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    if (null != db)
                    {
                        db.EndTransaction();
                        db.Close();
                        examItems = null;
                    }
                }
                catch (Exception e)
                {

                }
            }
            return Result;

        }

        public bool UpdateExamItemsVoice(ExamItem item)
        {
            SQLiteDatabase db = CurrentDataBase;
            bool Result = true;
            try
            {
                //保存地图
                ContentValues cValue = new ContentValues();
                cValue = new ContentValues();
                cValue.Put("VoiceText", item.VoiceText);
                cValue.Put("EndVoiceText", item.EndVoiceText);
                Result = db.Update("ExamItem", cValue, "ItemCode=?", new String[] { item.ItemCode }) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    if (null != db)
                    {
                        db.Close();
                        examItems = null;
                    }
                }
                catch (Exception e)
                {

                }
            }
            return Result;
        }
        public ExamItem[] GetAllExamItems()
        {
            //获取db操作对象
            SQLiteDatabase db = CurrentDataBase;
            ICursor c = db.Query("ExamItem", null, null, null, null, null, null, null);
            ExamItem[] examItems = new ExamItem[16];
            List<ExamItem> lstexamItem = new List<ExamItem>();
            ExamItem examItem = null;

            while (c.MoveToNext())
            {
                examItem = new ExamItem();
                examItem.ItemCode = c.GetString(c.GetColumnIndex("ItemCode"));
                examItem.ItemName = c.GetString(c.GetColumnIndex("ItemName"));
                examItem.VoiceText = c.GetString(c.GetColumnIndex("VoiceText"));
                examItem.VoiceFile = c.GetString(c.GetColumnIndex("VoiceText"));
                examItem.EndVoiceText = c.GetString(c.GetColumnIndex("EndVoiceText"));
                examItem.EndVoiceFile = c.GetString(c.GetColumnIndex("EndVoiceText"));
                examItem.SequenceNumber = c.GetInt(c.GetColumnIndex("SequenceNumber"));
                examItem.MapPointType = (MapPointType)c.GetInt(c.GetColumnIndex("MapPointType"));
                examItem.ExamItemType = c.GetString(c.GetColumnIndex("ExamItemType"));
                examItem.IsEnable = c.GetInt(c.GetColumnIndex("IsEnable")) == 1;
                lstexamItem.Add(examItem);
            }
            examItems = new ExamItem[lstexamItem.Count];
            for (int i = 0; i < lstexamItem.Count; i++)
            {
                examItems[i] = lstexamItem[i];
            }
            return examItems;
        }



        public LightRule[] GetLightRules()
        {
            SQLiteDatabase db = CurrentDataBase;
            ICursor c = db.Query("LightRule", null, null, null, null, null, null, null);
            LightRule[] lightRules = new LightRule[16];
            List<LightRule> lstLightRule = new List<LightRule>();
            LightRule lightRule = null;

            while (c.MoveToNext())
            {
                lightRule = new LightRule();
                lightRule.Id = c.GetInt(c.GetColumnIndex("Id"));
                lightRule.ItemCode = c.GetString(c.GetColumnIndex("ItemCode"));
                lightRule.ItemName = c.GetString(c.GetColumnIndex("ItemName"));
                lightRule.VoiceText = c.GetString(c.GetColumnIndex("VoiceText"));
                lightRule.VoiceFile = c.GetString(c.GetColumnIndex("VoiceText"));
                //lightRule.LightRuleType = c.GetString(c.GetColumnIndex("LightRuleType"));
                lightRule.LightRuleType = c.GetString(c.GetColumnIndex("LightRuleType")).Replace("\r", string.Empty).Replace("\n", string.Empty);
                lightRule.OperDes = c.GetString(c.GetColumnIndex("OperDes"));
                //
                //lightRule.IsEnable = c.GetInt(c.GetColumnIndex("IsEnable")) == 1;
                lstLightRule.Add(lightRule);
            }
            lightRules = new LightRule[lstLightRule.Count];
            for (int i = 0; i < lstLightRule.Count; i++)
            {
                lightRules[i] = lstLightRule[i];
            }
            return lightRules;

        }
        public List<DeductionRule> GetDeductionRuleByExamItem(List<string> lstExamItemCode)
        {

            return AllDeductionRules.Where(s => lstExamItemCode.Contains(s.ExamItemId.ToString())).ToList();
        }
        public DeductionRule[] GetAllDeductionRules()
        {
            SQLiteDatabase db = CurrentDataBase;
            ICursor c = db.Query("DeductionRule", null, null, null, null, null, null, null);
            DeductionRule[] deductionRules = new DeductionRule[16];
            List<DeductionRule> lstDeductionRule = new List<DeductionRule>();
            DeductionRule deductionRule = null;

            while (c.MoveToNext())
            {
                deductionRule = new DeductionRule();
                deductionRule.Id = c.GetInt(c.GetColumnIndex("Id"));
                deductionRule.ExamItemId = c.GetInt(c.GetColumnIndex("ExamItemId"));
                deductionRule.RuleCode = c.GetString(c.GetColumnIndex("RuleCode"));
                deductionRule.SubRuleCode = c.GetString(c.GetColumnIndex("SubRuleCode"));
                deductionRule.RuleName = c.GetString(c.GetColumnIndex("RuleName"));
                deductionRule.DeductedScores = c.GetInt(c.GetColumnIndex("DeductedScores"));
                deductionRule.IsRequired = c.GetInt(c.GetColumnIndex("IsRequired")) == 1;
                deductionRule.DeductedReason = c.GetString(c.GetColumnIndex("DeductedReason"));
                deductionRule.VoiceText = c.GetString(c.GetColumnIndex("VoiceText"));
                deductionRule.VoiceFile = c.GetString(c.GetColumnIndex("VoiceText"));
                deductionRule.IsAuto = c.GetInt(c.GetColumnIndex("IsAuto")) == 1;
                lstDeductionRule.Add(deductionRule);
            }
            deductionRules = new DeductionRule[lstDeductionRule.Count];
            for (int i = 0; i < lstDeductionRule.Count; i++)
            {
                deductionRules[i] = lstDeductionRule[i];
            }
            return deductionRules;
        }


        public bool UpdateDeductionRules(DeductionRule rule)
        {
            SQLiteDatabase db = CurrentDataBase;
            try
            {

                //保存地图

                ContentValues cValue = new ContentValues();
                cValue = new ContentValues();
                cValue.Put("RuleName", rule.DeductedReason);
                cValue.Put("DeductedReason", rule.DeductedReason);
                cValue.Put("VoiceText", rule.DeductedReason);
                cValue.Put("VoiceFile", rule.DeductedReason);
                cValue.Put("DeductedScores", rule.DeductedScores);
                cValue.Put("IsRequired", rule.IsRequired);
                //  cValue.Put("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                long result = db.Update("DeductionRule", cValue, "Id=?", new String[] { rule.Id.ToString() });
                //清空缓存
                deductionRules = null;
                //只有判断RuleCode 和 SubRuleCode 都相等的
                //使用Sql的方式更新数据库,
                string sql = string.Format("update DeductionRule set RuleName='{0}',DeductedReason='{0}',VoiceText='{0}',VoiceFile='{0}',DeductedScores='{1}',IsRequired='{2}' where Id='{3}'", rule.DeductedReason, rule.DeductedScores, rule.IsRequired, rule.Id);
                LogManager.WriteSystemLog(sql);
                //  db.ExecSQL(sql);
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.WriteSystemLog("Update DeductionRule exception" + ex.Message);
                return false;
            }
            finally
            {
                try
                {
                    if (null != db)
                    {
                        db.Close();
                        deductionRules = null;
                    }
                }
                catch (Exception e)
                {

                }
            }
        }
        public LightExamItem[] GetAllLightExamItems()
        {
            SQLiteDatabase db = CurrentDataBase;
            ICursor c = db.Query("LightExamItem", null, null, null, null, null, null, null);
            LightExamItem[] lightExamItems = new LightExamItem[10];
            List<LightExamItem> lstLightExamItem = new List<LightExamItem>();
            LightExamItem lightExamItem = null;

            while (c.MoveToNext())
            {
                lightExamItem = new LightExamItem();
                lightExamItem.Id = c.GetInt(c.GetColumnIndex("Id"));
                lightExamItem.GroupName = c.GetString(c.GetColumnIndex("GroupName"));
                lightExamItem.LightRules = c.GetString(c.GetColumnIndex("LightRules"));
                lstLightExamItem.Add(lightExamItem);
            }
            lightExamItems = new LightExamItem[lstLightExamItem.Count];
            for (int i = 0; i < lstLightExamItem.Count; i++)
            {
                lightExamItems[i] = lstLightExamItem[i];
            }
            return lightExamItems;
        }
        #region 固定的初始化数据
        //public LightRule[] GetLightRules()
        //{
        //    LightRule[] LightRules = new LightRule[]
        //  {
        //       new LightRule {Id=1,ItemCode="41601",ItemName="与对方会车,距对方来车150米,请使用",VoiceText="与对方会车,距对方来车150米,请使用",VoiceFile="与对方会车,距对方来车150米,请使用",LightRuleType="TwoPole.Chameleon3.Business.Rules.LowBeamLightRule,TwoPole.Chameleon3.Business",OperDes="开启近光灯，示廓灯" },
        //       new LightRule {Id=1,ItemCode="41601",ItemName="在夜间,在照明不良的条件下,您紧跟前车行驶,请使用",VoiceText="在夜间,在照明不良的条件下,您紧跟前车行驶,请使用",VoiceFile="在夜间,在照明不良的条件下,您紧跟前车行驶,请使用",LightRuleType="TwoPole.Chameleon3.Business.Rules.OpenHighLightRule,TwoPole.Chameleon3.Business",OperDes="开启近光灯，示廓灯" }
        //  };
        //    return LightRules;
        //}
        //public ExamItem[] GetAllExamItems()
        //{
        //    ExamItem[] examItems = new ExamItem[]
        //    {
        //        new ExamItem {ItemCode="40400",ItemName="加减档",VoiceFile="加减档",VoiceText="加减档",EndVoiceText="加减档完成",EndVoiceFile="加减档完成",SequenceNumber=1,MapPointType=MapPointType.ModifiedGear,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.ModifiedGear,TwoPole.Chameleon3.Business",IsEnable=true},
        //        new ExamItem {ItemCode="40000",ItemName="综合评判",VoiceFile="",VoiceText="",EndVoiceText="",EndVoiceFile="",SequenceNumber=2,MapPointType=MapPointType.CommonExamItem,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.CommonExamItem,TwoPole.Chameleon3.Business",IsEnable=true},
        //        new ExamItem {ItemCode="40100",ItemName="上车准备",VoiceFile="上车准备",VoiceText="上车准备",EndVoiceText="",EndVoiceFile="",SequenceNumber=3,MapPointType=MapPointType.PrepareDriving,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.PrepareDriving,TwoPole.Chameleon3.Business",IsEnable=true},
        //        new ExamItem {ItemCode="40200",ItemName="起步",VoiceFile="起步",VoiceText="起步",EndVoiceText="起步结束",EndVoiceFile="起步结束",SequenceNumber=3,MapPointType=MapPointType.VehicleStarting,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.VehicleStarting,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="40300",ItemName="直线行驶",VoiceFile="直线行驶",VoiceText="直线行驶",EndVoiceText="直线行驶结束",EndVoiceFile="直线行驶结束",SequenceNumber=3,MapPointType=MapPointType.StraightDriving,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.StraightDriving,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="40500",ItemName="变更车道",VoiceFile="变更车道",VoiceText="变更车道",EndVoiceText="变更车道结束",EndVoiceFile="变更车道结束",SequenceNumber=3,MapPointType=MapPointType.ChangeLines,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.ChangeLines,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="40700",ItemName="路口直行",VoiceFile="路口直行",VoiceText="路口直行",EndVoiceText="路口直行结束",EndVoiceFile="路口直行结束",SequenceNumber=3,MapPointType=MapPointType.StraightThrough,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.StraightThrough,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="40800",ItemName="路口左转",VoiceFile="路口左转",VoiceText="路口左转",EndVoiceText="路口左转结束",EndVoiceFile="路口左转结束",SequenceNumber=3,MapPointType=MapPointType.TurnLeft,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.TurnLeft,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="40900",ItemName="路口右转",VoiceFile="路口右转",VoiceText="路口右转",EndVoiceText="路口右转结束",EndVoiceFile="路口右转结束",SequenceNumber=3,MapPointType=MapPointType.TurnRight,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.TurnRight,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="41000",ItemName="人行横道",VoiceFile="人行横道",VoiceText="人行横道",EndVoiceText="人行横道结束",EndVoiceFile="人行横道结束",SequenceNumber=3,MapPointType=MapPointType.PedestrianCrossing,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.PedestrianCrossing,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="41100",ItemName="学校区域",VoiceFile="学校区域",VoiceText="学校区域",EndVoiceText="学校区域结束",EndVoiceFile="学校区域结束",SequenceNumber=3,MapPointType=MapPointType.SchoolArea,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.SchoolArea,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="41200",ItemName="公共汽车站",VoiceFile="公共汽车站",VoiceText="公共汽车站",EndVoiceText="公共汽车站结束",EndVoiceFile="公共汽车站结束",SequenceNumber=3,MapPointType=MapPointType.BusArea,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.BusArea,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="41300",ItemName="会车",VoiceFile="会车",VoiceText="会车",EndVoiceText="会车结束",EndVoiceFile="会车结束",SequenceNumber=3,MapPointType=MapPointType.Meeting,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.Meeting,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="41400",ItemName="超车",VoiceFile="超车",VoiceText="超车",EndVoiceText="超车结束",EndVoiceFile="超车结束",SequenceNumber=3,MapPointType=MapPointType.Overtaking,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.Overtaking,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="40600",ItemName="靠边停车",VoiceFile="靠边停车",VoiceText="靠边停车",EndVoiceText="靠边停车结束",EndVoiceFile="靠边停车结束",SequenceNumber=3,MapPointType=MapPointType.PullOver,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.PrepareDriving,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="41500",ItemName="掉头",VoiceFile="掉头",VoiceText="掉头",EndVoiceText="掉头结束",EndVoiceFile="掉头结束",SequenceNumber=3,MapPointType=MapPointType.TurnRound,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.TurnRound,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="41600",ItemName="灯光模拟",VoiceFile="灯光模拟",VoiceText="灯光模拟",EndVoiceText="灯光模拟结束",EndVoiceFile="灯光模拟结束",SequenceNumber=3,MapPointType=MapPointType.SimulationLights,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.SimulationLights,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="41800",ItemName="环岛",VoiceFile="环岛",VoiceText="环岛",EndVoiceText="环岛结束",EndVoiceFile="环岛结束",SequenceNumber=3,MapPointType=MapPointType.Roundabout,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.Roundabout,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="41900",ItemName="下一个项目",VoiceFile="下一个项目",VoiceText="下一个项目",EndVoiceText="",EndVoiceFile="",SequenceNumber=3,MapPointType=MapPointType.NextItem,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.NextItem,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="42000",ItemName="限速",VoiceFile="限速",VoiceText="限速",EndVoiceText="限速结束",EndVoiceFile="限速结束",SequenceNumber=3,MapPointType=MapPointType.StartSpeedLimit,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.StartSpeedLimit,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="41700",ItemName="急弯破路",VoiceFile="急弯破路",VoiceText="急弯破路",EndVoiceText="急弯破路结束",EndVoiceFile="急弯破路结束",SequenceNumber=3,MapPointType=MapPointType.SlopeWayParkingAndStarting,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.SlopeWayParkingAndStarting,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="41010",ItemName="人行横道",VoiceFile="人行横道",VoiceText="人行横道",EndVoiceText="人行横道结束",EndVoiceFile="人行横道结束",SequenceNumber=3,MapPointType=MapPointType.PedestrianCrossingHasPeople,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.PedestrianCrossingHasPeople,TwoPole.Chameleon3.Business",IsEnable=true},
        //       new ExamItem {ItemCode="42100",ItemName="减速让行",VoiceFile="减速让行",VoiceText="减速让行",EndVoiceText="减速让行结束",EndVoiceFile="减速让行结束",SequenceNumber=3,MapPointType=MapPointType.SlowSpeed,
        //                      Remark =null,IconFile=null,ExamItemType="TwoPole.Chameleon3.Business.ExamItems.SlowSpeed,TwoPole.Chameleon3.Business",IsEnable=true}
        //    };
        //    return examItems;
        //}
        #endregion



        /// <summary>
        /// 修改考试项目
        /// </summary>
        /// <param name="itemCode">编号</param>
        /// <param name="examItemType">指向路径</param>
        /// <returns></returns>
        public int UpdateExamItem(string itemCode, string examItemType)
        {
            //int num = ExecuteSql(string.Format("update ExamItem set ExamItemType='{0}' where ItemCode={1}", examItemType, itemCode));
            //return num;
            return 0;
        }
        public LightRule[] AllLightRules
        {
            get
            {

                if (lightRules == null)
                {
                    lightRules = GetLightRules();
                }
                return lightRules;
            }
        }

        public int UpdateLightRule(string itemCode, string operDes, string lightRuleType)
        {
            return 0;
        }
        public LightExamItem[] AllLightExamItems
        {
            get
            {
                if (lightExamItems == null)
                {
                    lightExamItems = GetAllLightExamItems();
                }
                return lightExamItems;
            }
        }

        public IList<MapLine> ALLMapLines
        {
            get
            {
                if (lstMaplineCache == null)
                {
                    lstMaplineCache = GetAllMapLines();
                }
                return GetAllMapLines();
            }
        }
        public DeductionRule[] AllDeductionRules
        {
            get
            {
                if (deductionRules == null)
                {
                    deductionRules = GetAllDeductionRules();
                }
                return GetAllDeductionRules();
            }
        }
        public DeductionRule[] AllDeductionRulesExamItems { get { return null; } }
        #endregion

        public IList<MapLine> GetAllMapLines()
        {
            SQLiteDatabase db = CurrentDataBase;
            ICursor c = db.Query("MapLine", null, null, null, null, null, "Name", null);
            MapLine[] MapLines = new MapLine[c.Count];
            List<MapLine> lstMapLine = new List<MapLine>();
            MapLine mapLine = null;
            while (c.MoveToNext())
            {
                mapLine = new MapLine();
                mapLine.Id = c.GetInt(c.GetColumnIndex("Id"));
                mapLine.Name = c.GetString(c.GetColumnIndex("Name"));
                mapLine.Distance = c.GetDouble(c.GetColumnIndex("Distance"));
                mapLine.Remark = c.GetString(c.GetColumnIndex("Remark"));
                mapLine.CreateOn = c.GetString(c.GetColumnIndex("CreatedOn")).ToDateTime().Value;
                mapLine.Points = mapLine.Remark.FromJson<IList<MapLinePoint>>();
                lstMapLine.Add(mapLine);
            }
            MapLines = new MapLine[lstMapLine.Count];
            for (int i = 0; i < lstMapLine.Count; i++)
            {
                MapLines[i] = lstMapLine[i];
            }

            var query = from item in MapLines orderby item.Id select item;

            return query.ToList();

        }
        public IList<DeductionRule> GetAllDeductionRule()
        {
            return AllDeductionRules.ToList();
        }
        public IList<DeductionRule> GetAllDeductionRuleExamItems()
        {
            return AllDeductionRulesExamItems.ToList();
        }
        public int UpdateDeductionRule(string ruleCode, int deductedScores)
        {
            return 0;
        }

        public int UpdateDeductionRRule(string ruleCode, int isRequired)
        {
            return 0;
        }
        //通过sql查询查询出对应的扣分规则
        //Bug
        public DeductionRule GetDeductionRule(string ruleCode, string subRuleCode)
        {
            //todo:感觉有Bug 暂时不处理 没时间了 下次再说！
            DeductionRule rule;
            if (!string.IsNullOrEmpty(subRuleCode))
            {
                //todo:下一次秀爱
                rule = AllDeductionRules.Where(S => S.SubRuleCode == subRuleCode).FirstOrDefault();
                if (rule != null)
                {
                    return rule;
                }
            }
            rule = AllDeductionRules.Where(S => S.RuleCode == ruleCode).FirstOrDefault();
            return rule;
        }

        public bool DeleteLightExamItem(LightExamItem item)
        {
            SQLiteDatabase db = CurrentDataBase;
            long result = db.Delete("LightExamItem", "Id=?", new String[] { item.Id.ToString() });
            lightExamItems = null;
            return result > 0;
        }
        public bool DeleteLightRule(LightRule item)
        {
            SQLiteDatabase db = CurrentDataBase;
            long result = db.Delete("LightRule", "Id=?", new String[] { item.Id.ToString() });
            lightRules = null;
            return result > 0;
        }

        /// <summary>
        /// 保存灯光之后移除缓存
        /// </summary>
        /// <param name="item"></param>
        public bool SaveLightExamItem(LightExamItem item)
        {
            SQLiteDatabase db = CurrentDataBase;
            //保存地图
            ContentValues cValue = new ContentValues();
            cValue.Put("Id", GetLightExamID());
            cValue.Put("GroupName", item.GroupName);
            cValue.Put("LightRules", item.LightRules);
            cValue.Put("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cValue.Put("UpdatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            long result = db.Insert("LightExamItem", null, cValue);
            //清楚缓存，重新加载
            lightRules = null;
            //清楚缓存，重新加载
            lightExamItems = null;
            return result > 0;
        }

        public bool UpdateLightExamItem(LightExamItem item)
        {
            SQLiteDatabase db = CurrentDataBase;
            //保存地图
            ContentValues cValue = new ContentValues();
            cValue.Put("Id", item.Id);
            cValue.Put("GroupName", item.GroupName);
            cValue.Put("LightRules", item.LightRules);
            cValue.Put("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cValue.Put("UpdatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            long result = db.Update("LightExamItem", cValue, "Id=?", new String[] { item.Id.ToString() });
            //清楚缓存，重新加载
            lightRules = null;
            //清楚缓存，重新加载
            lightExamItems = null;
            return result > 0;
        } 


        public bool SaveLightRule(LightRule item)
        {
            SQLiteDatabase db = CurrentDataBase;
            ContentValues cValue = new ContentValues();
            cValue.Put("Id", GetLightRuleID());
            cValue.Put("ItemCode", item.ItemCode);
            cValue.Put("ItemName", item.ItemName);
            cValue.Put("VoiceText", item.VoiceText);
            cValue.Put("VoiceFile", item.VoiceFile);
            cValue.Put("LightRuleType", item.LightRuleType);
            cValue.Put("OperDes", item.OperDes);
            cValue.Put("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            long result = db.Insert("LightRule", null, cValue);
            lightRules = null;
            lightExamItems = null;
            return result > 0;
        }

        public bool EditLightRule(LightRule item)
        {
            SQLiteDatabase db = CurrentDataBase;
            ContentValues cValue = new ContentValues();
            cValue.Put("ItemCode", item.ItemCode);
            cValue.Put("ItemName", item.ItemName);
            cValue.Put("VoiceText", item.VoiceText);
            cValue.Put("VoiceFile", item.VoiceFile);
            cValue.Put("LightRuleType", item.LightRuleType);
            cValue.Put("OperDes", item.OperDes);
            cValue.Put("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            long result = db.Update("LightRule", cValue, "Id=?", new String[] { item.Id.ToString() });
            lightRules = null;
            lightExamItems = null;
            return result > 0;
        }
        //保存灯光模拟分组
        public bool SaveLightExamItems(IList<LightExamItem> items, bool IsCover = true)
        {
            //保存灯光分组
            SQLiteDatabase db = CurrentDataBase;
            bool result = true;
            if (IsCover)
            {
                db.ExecSQL("delete from LightExamItem");
            }

            foreach (var item in items)
            {
                ContentValues cValue = new ContentValues();
                cValue.Put("Id", IsCover ? item.Id : GetLightExamID());
                cValue.Put("GroupName", item.GroupName);
                cValue.Put("LightRules", item.LightRules);
                cValue.Put("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cValue.Put("UpdatedOn", item.UpdatedOn.ToString("yyyy-MM-dd HH:mm:ss"));
                result = db.Insert("LightExamItem", null, cValue) > 0 & result;
                //清楚缓存，重新加载
            }
            lightExamItems = null;
            return result;
            //保存地

        }

        public IList<VehicleModel> GetAllVehicleModels()
        {
            return null;
        }


        /// <summary>
        /// 加载地图数据
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public IMapSet LoadMapSet(int mapId)
        {
            return null;
        }


        public bool DeleteMap(MapLine mapLine)
        {
            SQLiteDatabase db = CurrentDataBase;
            long result = db.Delete("MapLine", "Id=?", new String[] { mapLine.Id.ToString() });
            return result > 0;
        }

        /// <summary>
        /// 保存地图
        /// </summary>
        /// <param name="mapLine"></param>
        /// <returns></returns>
        public bool SaveMap(MapLine mapLine)
        {
            SQLiteDatabase db = CurrentDataBase;
            //保存地图
            ContentValues cValue = new ContentValues();
            cValue.Put("Id", GetNewMapID());
            cValue.Put("Name", mapLine.Name);
            cValue.Put("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cValue.Put("Remark", mapLine.Points.ToJson());
            cValue.Put("Distance", mapLine.Distance);
            cValue.Put("CenterLongitude", 0);
            cValue.Put("CenterLatitude", 0);
            cValue.Put("MinLongitude", 0);
            cValue.Put("MinLatitude", 0);
            cValue.Put("MaxLongitude", 0);
            cValue.Put("MaxLatitude", 0);
            long result = db.Insert("MapLine", null, cValue);
            return result > 0;

        }
        public bool SaveAddMap(List<MapLine> lstMapLine, bool IsCover = true)
        {
            SQLiteDatabase db = CurrentDataBase;
            bool result = true;
            if (IsCover)
            {
                db.ExecSQL("delete from MapLine");
            }

            foreach (var mapLine in lstMapLine)
            {
                //保存地图
                //Bug，这个不能够添加成功
                ContentValues cValue = new ContentValues();
                cValue.Put("Id", IsCover ? mapLine.Id : GetNewMapID());
                cValue.Put("Name", mapLine.Name);
                cValue.Put("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cValue.Put("Remark", mapLine.Points.ToJson());
                cValue.Put("Distance", mapLine.Distance);
                cValue.Put("CenterLongitude", 0);
                cValue.Put("CenterLatitude", 0);
                cValue.Put("MinLongitude", 0);
                cValue.Put("MinLatitude", 0);
                cValue.Put("MaxLongitude", 0);
                cValue.Put("MaxLatitude", 0);
                result = db.Insert("MapLine", null, cValue) > 0 & result;
            }
            return result;

        }
        public int GetNewMapID()
        {

            if (GetAllMapLines().Count >= 1)
            {
                int ID = GetAllMapLines().OrderByDescending(S => S.Id).FirstOrDefault().Id;
                ID += 1;
                return ID;
            }
            else
            {
                return 1;
            }
            //查询出最大的ID值
        }

        public int GetSettingID()
        {

            int ID = GetISetting().OrderByDescending(S => S.Id).FirstOrDefault().Id;
            ID += 1;
            return ID;
        }
        public int GetLightRuleID()
        {

            if (GetLightRules().Count() >= 1)
            {
                int ID = GetLightRules().OrderByDescending(S => S.Id).FirstOrDefault().Id;
                ID += 1;
                return ID;
            }
            else
            {
                return 1;
            }
            //查询出最大的ID值
        }
        public int GetLightExamID()
        {


            if (GetAllLightExamItems().Count() >= 1)
            {

                int ID = GetAllLightExamItems().OrderByDescending(S => S.Id).FirstOrDefault().Id;
                ID += 1;
                return ID;
            }
            else
            {
                return 1;
            }
            //查询出最大的ID值
        }
        public SQLiteDatabase CurrentDataBase
        {
            get
            {
                SQLiteDatabase db = this.ReadableDatabase;
                return db;
            }

        }

       

        /// <summary>
        /// 更新地图
        /// </summary>
        /// <param name="mapLine"></param>
        /// <returns></returns>
        public bool UpdateMap(MapLine mapLine)
        {
            SQLiteDatabase db = CurrentDataBase;
            //保存地图
            ContentValues cValue = new ContentValues();

            cValue.Put("Name", mapLine.Name);
            cValue.Put("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cValue.Put("Remark", mapLine.Points.ToJson());
            cValue.Put("Distance", mapLine.Distance);
            cValue.Put("CenterLongitude", 0);
            cValue.Put("CenterLatitude", 0);
            cValue.Put("MinLongitude", 0);
            cValue.Put("MinLatitude", 0);
            cValue.Put("MaxLongitude", 0);
            cValue.Put("MaxLatitude", 0);
            long result = db.Update("MapLine", cValue, "Id=?", new String[] { mapLine.Id.ToString() });
            return result > 0;
        }

        //把MapID保存进入配置文件里面这个可以暂时不用做。
        public void SaveDefaultMapId(int ExamMapId)
        {
            try
            {
                //20181012，新增，李
                GetSettings().ExamMapId = ExamMapId;


                SaveSettings(GetSettings());


            }
            catch (Exception exp)
            {
                
            }
        }

        /// <summary>
        /// 返回默认存储的地图
        /// </summary>
        /// <returns></returns>
        public int getDefaultMapId()
        {
            return GetSettings().ExamMapId;
        }


        /// <summary>
        /// 采用部分更新加快配置保存的速度
        /// </summary>
        /// <param name="listSetting"></param>
        public void SaveUpdateSettings(IList<Setting> listSetting)
        {
            //   var dbSettings = listSetting;
            SQLiteDatabase db = CurrentDataBase;
            try
            {
                //BuildPropertySettings(settings, dbSettings, GlobalSettingsGroupName);
                listSetting = GetListSettingIDByKey(listSetting, GlobalSettingsGroupName);

                //dbSetting 里面的数据已经更新 目前需要进行db操作更新数据库
                db.BeginTransaction();
                //保存地图
                ContentValues cValue = new ContentValues();
                bool Result = true;

                foreach (var item in listSetting)
                {
                    cValue = new ContentValues();
                    cValue.Put("ModifiedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cValue.Put("Value", item.Value);
                    //执行数据库更新次数太多，应该是可以进行批量更新操作
                    if (db.Update("Setting", cValue, "Id=?", new String[] { item.Id.ToString() }) < 0)
                    {
                        Result = false;
                        break;
                    }
                }
                int MaxSettingId = GetSettingID();
                //表明是新添加进来的变量
                //直接取原来里面的比较呀！
                foreach (var item in listSetting.Where(S => S.Id == DefaultNewSettingID))
                {
                    cValue = new ContentValues();
                    cValue.Put("Id", MaxSettingId++);
                    cValue.Put("ModifiedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cValue.Put("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cValue.Put("GroupName", item.GroupName);
                    cValue.Put("Remark", item.Remark);
                    cValue.Put("Value", item.Value);
                    cValue.Put("Key", item.Key);
                    //执行数据库更新次数太多，应该是可以进行批量更新操作
                    db.Insert("Setting", null, cValue);

                }
                if (Result)
                {
                    //进行事物提交
                    db.SetTransactionSuccessful();
                }
                //循环进行更新保存配置。
            }
            catch (Exception ex)
            {
                throw new Exception("SaveUpdateSettings:" + ex.Message);
            }
            finally
            {
                try
                {
                    if (null != db)
                    {
                        db.EndTransaction();
                        db.Close();
                    }
                }
                catch (Exception e)
                {

                }
            }
        }
        public void SaveAddSettings(IList<Setting> listSetting, bool IsCover = true)
        {
            //需要先删除所有的数据
            SQLiteDatabase db = CurrentDataBase;
            if (IsCover)
            {
                db.ExecSQL("delete from Setting");
            }

            try
            {
                db.BeginTransaction();
                //保存地图
                ContentValues cValue = new ContentValues();
                bool Result = true;

                foreach (var item in listSetting)
                {
                    cValue = new ContentValues();
                    cValue.Put("Id", item.Id);
                    cValue.Put("ModifiedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cValue.Put("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cValue.Put("GroupName", item.GroupName);
                    cValue.Put("Remark", item.Remark);
                    cValue.Put("Value", item.Value);
                    cValue.Put("Key", item.Key);
                    //执行数据库更新次数太多，应该是可以进行批量更新操作
                    Result = Result & db.Insert("Setting", null, cValue) > 0;
                }
                if (Result)
                {
                    //进行事物提交
                    db.SetTransactionSuccessful();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SaveAddSetting:" + ex.Message);
            }
            finally
            {
                try
                {
                    if (null != db)
                    {
                        db.EndTransaction();
                        db.Close();
                    }
                }
                catch (Exception e)
                {

                }
            }

        }
        //TODO:可以考虑修改成为拼接Sql 这样可以加快Sql执行速度。
        //如果采用部分更新技术会怎么样
        //只是需要一个IList<Setting>
        public void SaveSettings(GlobalSettings settings)
        {
            var dbSettings = GetISetting();
            SQLiteDatabase db = CurrentDataBase;
            try
            {
                BuildPropertySettings(settings, dbSettings, GlobalSettingsGroupName);


                //dbSetting 里面的数据已经更新 目前需要进行db操作更新数据库
                db.BeginTransaction();
                //保存地图
                ContentValues cValue = new ContentValues();
                bool Result = true;

                foreach (var item in dbSettings)
                {
                    cValue = new ContentValues();
                    cValue.Put("ModifiedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cValue.Put("Value", item.Value);
                    //执行数据库更新次数太多，应该是可以进行批量更新操作
                    if (db.Update("Setting", cValue, "Id=?", new String[] { item.Id.ToString() }) < 0)
                    {
                        Result = false;
                        break;
                    }
                }
                int MaxSettingId = GetSettingID();
                //表明是新添加进来的变量
                foreach (var item in dbSettings.Where(S => S.Id == DefaultNewSettingID))
                {
                    cValue = new ContentValues();
                    cValue.Put("Id", MaxSettingId++);
                    cValue.Put("ModifiedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cValue.Put("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cValue.Put("GroupName", item.GroupName);
                    cValue.Put("Remark", item.Remark);
                    cValue.Put("Value", item.Value);
                    cValue.Put("Key", item.Key);
                    //执行数据库更新次数太多，应该是可以进行批量更新操作
                    db.Insert("Setting", null, cValue);
                }
                if (Result)
                {
                    //进行事物提交
                    db.SetTransactionSuccessful();
                }
                //循环进行更新保存配置。
            }
            catch (Exception ex)
            {
                throw new Exception("SaveSetting:" + ex.Message);
            }
            finally
            {
                try
                {
                    if (null != db)
                    {
                        db.EndTransaction();
                        db.Close();
                    }
                }
                catch (Exception e)
                {

                }
            }


        }

        public Setting GetSettingByKey(string key)
        {
            return null;
        }
        public GlobalSettings GetSettings()
        {
            if (tempsettings == null)
            {
                tempsettings = BuildSettings<GlobalSettings>(null, AllSettings);
            }
            return tempsettings;

            //var v = BuildSettings<GlobalSettings>(null, AllSettings);
            //return v;
        }




    }
}
