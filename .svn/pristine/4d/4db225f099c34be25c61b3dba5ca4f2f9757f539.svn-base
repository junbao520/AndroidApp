using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Business.LuZhou.Modules
{

    //TODO:增加准备就绪的开始流程
    public class LocalizationExamModule : TwoPole.Chameleon3.Business.Modules.ExamModule
    {
        public override async Task ReadyStartExamAsync(ExamContext context)
        {

            //清除历史的信号记录；
            SignalSet.Clear();

            //设置考试里程
            context.ExamDistance = (context.ExamTimeMode == ExamTimeMode.Day)
                ? Settings.ExamDistance
                : Settings.NightDistance;

            await ExamManager.StartExamAsync(context);

            await Task.Run(() =>
            {
                while (!(SignalSet.Current != null && SignalSet.Current.Sensor.Brake))
                {
                    Thread.Sleep(100);
                }
            });

            await StartExamItemAutoAsync(context, ExamItemCodes.CommonExamItem);
            await StartExamItemAutoAsync(context, ExamItemCodes.Start);
        }

        /// <summary>
        /// 特殊要求：灯光完成后5秒才能报起步（泸州）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task StartExamAsync(ExamContext context)
        {
            if (!context.IsExaming)
                return;

            //启用绕车一周 to 在车上进行验证探头
            if (Settings.PrepareDrivingEnable)
            {
                var prepareTask = StartExamItemAutoAsync(context, ExamItemCodes.PrepareDriving);
                await prepareTask;

                await Task.Run(() =>
                {
                    while (context.IsExaming && prepareTask.Result.State != ExamItemState.Finished)
                    {
                        Thread.Sleep(100);
                    }
                });
            }
            await StartExamItemAutoAsync(context, ExamItemCodes.CommonExamItem);
            if (!context.IsExaming)
            {
                Logger.Debug("考试结束:context.IsExaming");
                return;
            }
            //上车准备完成之后就启动灯光模拟

          

            //吉首白天夜间都要灯光模拟
            if (context.ExamTimeMode == ExamTimeMode.Day && Settings.SimulationsLightOnDay
                || context.ExamTimeMode == ExamTimeMode.Night && Settings.SimulationsLightOnNight)
            {

                Speaker.CancelAllAsync(false);
                while (SignalSet.Current == null)
                {
                    Thread.Sleep(100);
                }
                if (SignalSet.Current != null && !SignalSet.Current.Sensor.Engine)
                {
                    if (Settings.CommonExambeforeSimionLightStartEngine)
                    {
                        Speaker.PlayAudioAsync(Settings.CommonExambeforeSimionLightStartEngineVoice);
                    }
                }
                while (!(SignalSet.Current != null && SignalSet.Current.Sensor.Engine) && Settings.CommonExambeforeSimionLightStartEngine)
                {
                    //如果启动要扣分

                    Thread.Sleep(100);
                }

                //打火黑屏延时启动灯光模拟
                if (Settings.CommonExambeforeSimionLightStartEngine && Settings.CommonStartEngineDeleyTime > 0)
                {
                    await Task.Run(() =>
                    {
                        int delaytime = Settings.CommonStartEngineDeleyTime * 1000;
                        Thread.Sleep(delaytime);
                    });
                }

                //等待关闭发动机
                var lightTask = StartExamItemAutoAsync(context, ExamItemCodes.Light);

                await lightTask;
                await Task.Run(() =>
                {
                    while (context.IsExaming && lightTask.Result.State != ExamItemState.Finished)
                    {
                        Thread.Sleep(100);
                    }
                });

            }
            if (!context.IsExaming)
            {
                Logger.Debug("考试结束:context.IsExaming");
                return;
            }

            //TODO:可以考虑做成配置 决定综合评判启动的位置
            await Task.Run(() => {
                //等待5秒才能启动起步
                Thread.Sleep(3000);
            });

            await StartExamItemAutoAsync(context, ExamItemCodes.Start);
        }
    }
}
