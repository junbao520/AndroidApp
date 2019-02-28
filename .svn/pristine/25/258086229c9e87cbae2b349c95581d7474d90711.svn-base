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
using System.Diagnostics;

namespace TwoPole.Chameleon3.Infrastructure.Services
{
    /// <summary>
    /// 默认Provider的工厂类
    /// </summary>
    public class DefaultProviderFactory : IProviderFactory
    {
        protected IDataService DataService { get; private set; }
        protected ILog Logger { get; private set; }

        private bool IsLoading = false;
        public DefaultProviderFactory(IDataService dataService,ILog log)
        {
            //DataService = Singleton.GetDataService;
            //Logger = Singleton.GetLogManager;
            this.DataService = dataService;
            this.Logger = log;

            BeforeLoadSimulationLight();
        }
        /// <summary>
        /// 提前加载灯光规则
        /// </summary>
        public  void BeforeLoadSimulationLight()
        {

            // Todo: 由于通过反射加载比较慢，暂时无法处理。故采用多个线程进行初始化
            // 每一个线程初始化5条数据
            // 开始多线程初始化添加灯光模拟
            //如果修改成为播放语音的时候顺便去加载下以条如何
            //如果是15条
            //LightExam = CreateExamItem(ExamItemCodes.Light);
            //Task.Run(() =>
            //{
            //    int LightCount = DataService.AllLightRules.Count();
            //    for (int i = 0; i < LightCount / 5; i++)
            //    {
            //        System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CreateLightExamItem));
            //        t.Start((i * 5).ToString() + "," + "5");
            //    }
            //    int Count = LightCount - (LightCount / 5) * 5;
            //    int StartIndex = (LightCount / 5) * 5;
            //    System.Threading.Thread t1 = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CreateLightExamItem));
            //    t1.Start(StartIndex.ToString() + "," + Count.ToString());
            //    //这个东西涉到最好等待多线程全部执行完成。
            //    //等待所有线程全都都加载完成
            //});


            //等待执行完成

          
            Task.Run(() =>
            {
                //不进行提前加载灯光,每次播放的时候去加载一句一句的进行加载，这样应该用户操作完成之后需要等待时间在2秒左右吧
                //或者之家在
              
                LightExam = CreateExamItem(ExamItemCodes.Light);
                LightLoaded = true;
                IsLoading = true;
                foreach (var item in DataService.AllExamItems)
                {
                    if (item.ItemCode==ExamItemCodes.Light)
                    {
                        continue;
                    }
                    var examItem = CreateExamItem(item.ItemCode);
                    KeyValuePair<string, IExamItem> TempExamItem = new KeyValuePair<string, IExamItem>(item.ItemCode, examItem);
                    lstKeyValueExamItem.Add(TempExamItem);
                }
                if (lstKeyValueExamItem.Count>2)
                {
                    ExamItemLoaded = true;
                }
            });
        }
        /// <summary>
        /// 是否已经异步加载灯光模拟
        /// </summary>
        private bool LightLoaded = false;

        //是否其他考试项目已经加载完成
        private bool ExamItemLoaded = false;
        /// <summary>
        /// 记录异步加载的灯光模拟
        /// </summary>
       private static IExamItem LightExam { get; set; }



        private IExamItem PullOverExamItem { get; set; }
        /// <summary>
        /// 是否已经加载灯光规则
        /// </summary>
      

        private List<KeyValuePair<string, IExamItem>> lstKeyValueExamItem;


        public void CreateLightExamItem(object Index)
        {
            LightLoaded = false;
            int Skip = Convert.ToInt32(Index.ToString().Split(',')[0]);
            int Take= Convert.ToInt32(Index.ToString().Split(',')[1]);
            if (LightExam==null)
            {
                return;
            }
            
            var query = from a in DataService.AllLightRules.Skip(Skip).Take(Take)
                        let lightRule = CreateLightRule(a, DataService.AllSettings)
                        select lightRule;
            try
            {
                LightExam.SetRules(query);
            }
            catch (Exception exp)
            {
                Logger.Error("CreateExamItem", exp.Message);
            }
           // LightExam.SetRulesEnd();
            LightLoaded = true;
        }


        /// <summary>
        /// 创建 ExamItem if I have a chance 
        /// </summary>
        /// <param name="examItemCode"></param>
        /// <returns></returns>
        public IExamItem CreateExamItem(string examItemCode, string ItemVoice = "", string ItemEndVoice = "")
        {
            if (examItemCode == ExamItemCodes.Light && LightLoaded)
            {
                if (LightExam!=null)
                {
                    return LightExam;
                }
            }
            //如果不是灯光模拟的其他路考项目
            if (examItemCode!=ExamItemCodes.Light&&ExamItemLoaded)
            {
                if (lstKeyValueExamItem.Where(s => s.Key == examItemCode).Count() > 0)
                {
                    var examitem= lstKeyValueExamItem.Where(s => s.Key == examItemCode).FirstOrDefault().Value;
                    if (!string.IsNullOrEmpty(ItemVoice))
                    {
                        examitem.VoiceFile = ItemVoice;
                    }
                    if (!string.IsNullOrEmpty(ItemEndVoice))
                    {
                        examitem.EndVoiceFile = ItemEndVoice;
                    }
                    return examitem;
                }
            }
            var examItemSetting =DataService.AllExamItems.FirstOrDefault(x => x.ItemCode == examItemCode);
            //直接预先加载靠边停车考试项目
            if (examItemSetting == null)
            {
                var message = string.Format("考试项目 [{0}] 不存在", examItemCode);
                throw new ArgumentException(message);
            }

        
            var examItem = CreateExamItem(examItemSetting, DataService.AllSettings);

            //灯光模拟，设置规则
            
            //if (examItemCode == ExamItemCodes.Light)
            //{
            //    Stopwatch sw = new Stopwatch();
            //    sw.Start();
            //    IsLoading = true;
            //    var query = from a in DataService.AllLightRules
            //                let lightRule = CreateLightRule(a, DataService.AllSettings)
            //                select lightRule;
            //    sw.Stop();

            //    Logger.Error("ExamItemLoadingTime" + sw.ElapsedMilliseconds.ToString());

            //    try
            //    {
            //        examItem.SetRules(query);
            //    }
            //    catch (Exception exp)
            //    {
            //        Logger.Error("CreateExamItem", exp.Message);
            //    }
            //}

            return examItem;
        }


        private static ICarSignalProcessor[] _signalProcessors;
        public ICarSignalProcessor[] CreateSignalProcessors()
        {
            return null;
        }

        /// <summary>
        /// 直接创建蓝牙版本
        /// </summary>
        /// <returns></returns>
        public ICarSignalSeed CreateCarSignalSeed()
        {
            //ICarSignalSeed signalSeed = null;
            //signalSeed = new TwoPole.Chameleon3.Infrastructure.Devices.CarSignal.BluetoothCarSignalSeed();
            //return signalSeed;
            return null;
        }

        private IExamItem CreateExamItem(ExamItem examItem, IEnumerable<Setting> settings = null, string ItemVoice = "", string ItemEndVoice = "")
        {
            try
            {
                //核心
                var type = Type.GetType(examItem.ExamItemType);
                var v = (IExamItem)Activator.CreateInstance(type, true);
                v.ItemCode = examItem.ItemCode;
                v.Name = examItem.ItemName;
                v.VoiceFile = ItemVoice==string.Empty?examItem.VoiceFile:ItemVoice;
                v.EndVoiceFile =ItemEndVoice==string.Empty? examItem.EndVoiceFile:ItemEndVoice;
                v.VoiceText = examItem.VoiceText;
                var provider = v as IProvider;
                if (provider != null)
                {
                    var nameValues = settings.ToValues();
                    provider.Init(nameValues);
                }
                return v;
            }
            catch (Exception exp)
            {
                 Logger.ErrorFormat("创建 ExamItem {0}发生异常，原因：{1}", examItem.ExamItemType, exp, exp);
                 return null;
            }
        }

        private ILightRule CreateLightRule(LightRule rule, IEnumerable<Setting> settings = null)
        {
            try
            {
                var type = Type.GetType(rule.LightRuleType);
                //如果是一个
                var v = (ILightRule)Activator.CreateInstance(type, true);
                v.Id = rule.Id;
                v.VoiceFile = rule.VoiceFile;
                v.VoiceText = rule.VoiceText;
                v.RuleCode = rule.ItemCode;
                var provider = v as IProvider;
                var nameValues = settings.ToValues();
                provider.Init(nameValues);
                return v;
            }
            catch (Exception exp)
            {
                 Logger.ErrorFormat("创建 ILightRule {0}发生异常，原因：{1}", rule.LightRuleType, exp, exp);
                 return null;
            }
      
        }

        public ITrigger[] CreateTriggers()
        {
            throw new NotImplementedException();
        }
    }
}
