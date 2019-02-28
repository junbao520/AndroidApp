using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Foundation.Providers;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Infrastructure.Triggers;
using System.Threading;

namespace TwoPole.Chameleon3.Infrastructure.Services
{
    /// <summary>
    /// 默认Provider的工厂类
    /// </summary>
    //public class DefaultProviderFactoryOld : IProviderFactory
    //{
    //    protected IDataService DataService { get; private set; }

    //    private readonly Lazy<GlobalSettings> _globalSettingsLazyLoader;
    //    protected ILog Logger { get; private set; }

    //    public GlobalSettings GlobalSettings { get { return _globalSettingsLazyLoader.Value; } }
    //    protected MasterControlBoxVersion masterControlBoxVersion { get; set; }
    //    protected OBDSource obdSource { get; set; }


    //    public DefaultProviderFactoryOld()
    //    {
    //        DataService = Singleton.GetDataService;
    //        Logger = Singleton.GetLogManager;

    //        BeforeLoadSimulationLight();
    //    }
    //    /// <summary>
    //    /// 提前加载灯光规则
    //    /// </summary>
    //    public void BeforeLoadSimulationLight()
    //    {

    //        Task.Run(() =>
    //        {
    //            IsLoading = false;

    //            LightExam = CreateExamItem(ExamItemCodes.Light);
    //            if (LightExam != null)
    //            {
    //                LightLoaded = true;
    //                IsLoading = false;
    //            }
    //        });
    //    }
    //    /// <summary>
    //    /// 是否已经异步加载灯光模拟
    //    /// </summary>
    //    private bool LightLoaded = false;
    //    /// <summary>
    //    /// 记录异步加载的灯光模拟
    //    /// </summary>
    //    private IExamItem LightExam { get; set; }
    //    /// <summary>
    //    /// 是否已经加载灯光规则
    //    /// </summary>
    //    private bool IsLoading = false;

    //    /// <summary>
    //    /// 创建 ExamItem
    //    /// </summary>
    //    /// <param name="examItemCode"></param>
    //    /// <returns></returns>
    //    public IExamItem CreateExamItem(string examItemCode)
    //    {
    //        //正在加载，灯光模拟
    //        //while (examItemCode == ExamItemCodes.Light && IsLoading)
    //        //{
    //        //    Thread.Sleep(100);
    //        //}
    //        if (examItemCode == ExamItemCodes.Light && LightLoaded)
    //        {
    //            LightLoaded = false;
    //            return LightExam;
    //        }


    //        var examItemSetting = DataService.AllExamItems.FirstOrDefault(x => x.ItemCode == examItemCode);
    //        if (examItemSetting == null)
    //        {
    //            var message = string.Format("考试项目 [{0}] 不存在", examItemCode);
    //            throw new ArgumentException(message);
    //        }

    //        var examItem = CreateExamItem(examItemSetting, DataService.AllSettings);

    //        //灯光模拟，设置规则
    //        if (examItemCode == ExamItemCodes.Light)
    //        {
    //            IsLoading = true;
    //            var query = from a in DataService.AllLightRules
    //                        let lightRule = CreateLightRule(a, DataService.AllSettings)
    //                        select lightRule;
    //            try
    //            {
    //                examItem.SetRules(query);
    //            }
    //            catch (Exception exp)
    //            {
    //                Logger.Error("CreateExamItem", exp.Message);
    //            }
    //        }

    //        return examItem;
    //    }


    //    private static ICarSignalProcessor[] _signalProcessors;
    //    public ICarSignalProcessor[] CreateSignalProcessors()
    //    {
    //        return null;
    //    }

    //    /// <summary>
    //    /// 直接创建蓝牙版本
    //    /// </summary>
    //    /// <returns></returns>
    //    public ICarSignalSeed CreateCarSignalSeed()
    //    {
    //        //ICarSignalSeed signalSeed = null;
    //        //signalSeed = new TwoPole.Chameleon3.Infrastructure.Devices.CarSignal.BluetoothCarSignalSeed();
    //        //return signalSeed;
    //        return null;
    //    }

    //    private IExamItem CreateExamItem(ExamItem examItem, IEnumerable<Setting> settings = null)
    //    {
    //        try
    //        {
    //            var type = Type.GetType(examItem.ExamItemType);
    //            //核心，如果这儿报错，有可能是对应的规则类出现问题，如，对应的构造函数
    //            var v = (IExamItem)Activator.CreateInstance(type, true);
    //            v.ItemCode = examItem.ItemCode;
    //            v.Name = examItem.ItemName;
    //            v.VoiceFile = examItem.VoiceFile;
    //            v.EndVoiceFile = examItem.EndVoiceFile;
    //            v.VoiceText = examItem.VoiceText;
    //            var provider = v as IProvider;
    //            if (provider != null)
    //            {
    //                var nameValues = settings.ToValues();
    //                provider.Init(nameValues);
    //            }
    //            return v;
    //        }
    //        catch (Exception exp)
    //        {
    //            Logger.ErrorFormat("创建 ExamItem {0}发生异常，原因：{1}", examItem.ExamItemType, exp, exp);
    //            return null;
    //        }
    //    }

    //    private ILightRule CreateLightRule(LightRule rule, IEnumerable<Setting> settings = null)
    //    {
    //        try
    //        {
    //            var type = Type.GetType(rule.LightRuleType);
    //            var v = (ILightRule)Activator.CreateInstance(type, true);
    //            //大家都是指向同一个地址的,只是VoiceFile不一样
    //            //其实这样也无所谓,我可以到时候直行的时候在进行单独的把语音文字输入进去
    //            v.Id = rule.Id;
    //            v.VoiceFile = rule.VoiceFile;
    //            v.VoiceText = rule.VoiceText;
    //            v.RuleCode = rule.ItemCode;
    //            var provider = v as IProvider;
    //            var nameValues = settings.ToValues();
    //            provider.Init(nameValues);
    //            return v;
    //        }
    //        catch (Exception exp)
    //        {
    //            Logger.ErrorFormat("创建 ILightRule {0}发生异常，原因：{1}", rule.LightRuleType, exp, exp);
    //            return null;
    //        }

    //    }

    //    public ITrigger[] CreateTriggers()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
