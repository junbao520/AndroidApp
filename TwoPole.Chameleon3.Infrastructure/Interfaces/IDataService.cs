using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Infrastructure
{
    public interface IDataService
    {
        string FolderName { get; set; }
        bool UpdateDeductionRules(DeductionRule rule);
        List<DeductionRule> GetDeductionRuleByExamItem(List<string> lstExamItemCode);
        List<ExamItem> GetExamItemsList(bool IsContainCommonExamItem=false);

        void Cache2();
        void Cache();
        void UpdateCache(string key, object value);

        /// <summary>
        /// 更新新规扣分规则
        /// </summary>
        /// <returns></returns>
        bool UpdateNewRules();
        string GetExamItemCode(string ItemName);
        /// <summary>
        /// 车辆配置
        /// </summary>
        Setting[] AllSettings { get; }

        /// <summary>
        /// 所有考试项目
        /// </summary>
        ExamItem[] AllExamItems { get; }

        bool UpdateExamItemsVoice(List<ExamItem> lstExamItem);

        bool UpdateExamItemsVoice(ExamItem item);
        /// <summary>
        /// 修改考试项目
        /// </summary>
        /// <param name="itemCode">编号</param>
        /// <param name="examItemType">指向路径</param>
        /// <returns></returns>
        int UpdateExamItem(string itemCode,string examItemType);

        /// <summary>
        /// 所有的灯光规则
        /// </summary>
        LightRule[] AllLightRules { get; }

        /// <summary>
        /// 所有的扣分规则
        /// </summary>
        DeductionRule[] AllDeductionRules { get; }

        int UpdateLightRule(string itemCode, string operDes, string lightRuleType);
        /// <summary>
        /// 所有的灯光分组
        /// </summary>
        LightExamItem[] AllLightExamItems { get; }


        /// <summary>
        /// 所有的地图
        /// </summary>
        IList<MapLine> ALLMapLines { get; }
        /// <summary>
        /// 获取所有的地图
        /// </summary>
        /// <returns></returns>
        IList<MapLine> GetAllMapLines();

        IList<VehicleModel> GetAllVehicleModels();

        IList<DeductionRule> GetAllDeductionRule();

        IList<DeductionRule> GetAllDeductionRuleExamItems();
        /// <summary>
        /// 修改分数
        /// </summary>
        /// <param name="ruleCode"></param>
        /// <param name="deductedScores"></param>
        /// <returns></returns>
        int UpdateDeductionRule(string ruleCode, int deductedScores);

        /// <summary>
        /// 修改是否启用语音
        /// </summary>
        /// <param name="ruleCode"></param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        int UpdateDeductionRRule(string ruleCode, int isRequired);
        /// <summary>
        /// 获取扣分项
        /// </summary>
        /// <param name="ruleCode">规则编码</param>
        /// <param name="subRuleCode">子规则编码</param>
        /// <returns></returns>
        DeductionRule GetDeductionRule(string ruleCode, string subRuleCode = null);
        

        /// <summary>
        /// 删除地图
        /// </summary>
        /// <param name="mapLine"></param>
        bool DeleteMap(MapLine mapLine);
        /// <summary>
        /// 保存地图
        /// </summary>
        /// <param name="mapLine"></param>
        bool SaveMap(MapLine mapLine);

        bool SaveAddMap(List<MapLine> lstMapLine, bool IsCover = true);
        /// <summary>
        /// 更新地图
        /// </summary>
        /// <param name="mapLine"></param>
        /// <returns></returns>
        bool UpdateMap(MapLine mapLine);

        /// <summary>
        /// 保存配置项
        /// </summary>
        /// <param name="settings"></param>
        void SaveSettings(GlobalSettings settings);

        Setting GetSettingByKey(string key);

        /// <summary>
        /// 获取全局配置项
        /// </summary>
        GlobalSettings GetSettings();
        void SaveAddSettings(IList<Setting> listSetting, bool IsCover = true);

        bool DeleteLightExamItem(LightExamItem item);

        bool DeleteLightRule(LightRule item);
        bool  SaveLightExamItem(LightExamItem item);

        bool UpdateLightExamItem(LightExamItem item);

        bool SaveLightRule(LightRule item);

        bool EditLightRule(LightRule item);

        bool SaveLightExamItems(IList<LightExamItem> items, bool IsCover = true);

        void SaveDefaultMapId(int ExamMapId);
        int getDefaultMapId();
        //部分更新策略
        void SaveUpdateSettings(IList<Setting> listSetting);

    }
}
