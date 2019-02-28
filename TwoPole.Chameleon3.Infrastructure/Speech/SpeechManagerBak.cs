using Android.Content;
using Android.Content.Res;
using Android.Media;
using Android.Speech.Tts;
using Java.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;
using static Android.Speech.Tts.TextToSpeech;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class SpeechManagerBak : Java.Lang.Object, IOnInitListener, ISpeaker
    {
        public static SpeechManagerBak Instance { get; set; }


        private static readonly object lockObj = new object();

        private TextToSpeech mTextToSpeech;//TTS对象

        private Context mContext;

        private bool IsPlay = true;
        private bool IsPlayActionVoice = true;

        string playVoiceText = string.Empty;

        string BreakePath = string.Empty;
        string LoopMediaPath = string.Empty;
        //这个还是需要生成音频我呢见
        string DongPath = string.Empty;

        private Queue<string> HighSpeechQueque = new Queue<string>();
        private Queue<string> NormalSpeechQueque = new Queue<string>();
        MediaPlayer player;
        MediaPlayer playerloop;
        Java.IO.FileInputStream fis = null;
        Java.IO.FileInputStream fisloop = null;
        Java.IO.FileInputStream dongfile = null;
        //内径,
        public SpeechManagerBak(Context context, bool _isPlayActionVoice = true)
        {
            this.mContext = context;//获取上下文
            this.mTextToSpeech = new TextToSpeech(this.mContext, this);//实例化TTS
            mTextToSpeech.SetPitch(0.5f);// 
            ///TODO:没有刹车音频文件是会报错的，所以这个是需要注意的
            InitSpeechPlayTimer();
            //是否播报操作语音
            IsPlayActionVoice = Singleton.PlayActionVoice;

            InitMedia(Singleton.dbName);
           

        }

        public void InitMedia(string dbName)
        {

            LogManager.WriteSystemLog("initMedia" + dbName);
            player = new MediaPlayer();
            playerloop = new MediaPlayer();
            var FolderName = dbName.Split('-')[2].Substring(0, dbName.Split('-')[2].Length - 3);

            LoopMediaPath = Android.OS.Environment.ExternalStorageDirectory + "/" + "loop.mp3";
            BreakePath = Android.OS.Environment.ExternalStorageDirectory + "/"+FolderName+"/" + "brake.wav";
            DongPath = Android.OS.Environment.ExternalStorageDirectory + "/" + FolderName + "/" + "dong.wav";


            Java.IO.File file = new Java.IO.File(BreakePath);
            if (File.Exists(BreakePath))
            {
                fis = new Java.IO.FileInputStream(file);
            }
            file = new Java.IO.File(LoopMediaPath);
            if (File.Exists(LoopMediaPath))
            {
                fisloop = new Java.IO.FileInputStream(file);
            }
            if (File.Exists(DongPath))
            {
                file = new Java.IO.File(DongPath);
                dongfile = new Java.IO.FileInputStream(file);
            }
        }
        static SpeechManagerBak()
        {
            Instance = new SpeechManagerBak();
        }

        public void InitSpeechPlay()
        {
            Thread thread = new Thread((new ThreadStart(StartPlay)));
            thread.Start();
        }
        public void InitSpeechPlayTimer()
        {
            System.Threading.Timer timer = new System.Threading.Timer(new System.Threading.TimerCallback(TimerStartPlay), null, 1000, 100);
        }

        //线程自动监听实现背景音乐循环
        //3秒钟之后 1秒钟查询一次
        public void InitLoopPlyaMediaTimer()
        {
            System.Threading.Timer timer = new System.Threading.Timer(new System.Threading.TimerCallback(SpeakLoopVoice), null,3000, 1000);
        }
        public void TimerStartPlay(object a)
        {
            if (!HasVoice)
            {
                if (HighSpeechQueque.Count > 0)
                {
                    playVoiceText = HighSpeechQueque.Dequeue();
                }
                else if (NormalSpeechQueque.Count > 0)
                {
                    playVoiceText = NormalSpeechQueque.Dequeue();
                }
                else
                {
                    return;
                }
                mTextToSpeech.Speak(playVoiceText, QueueMode.Add, null);
            }
        }
        public void StartPlay()
        {
            try
            {
                while (IsPlay)
                {
                    if (!HasVoice)
                    {
                        if (HighSpeechQueque.Count > 0)
                        {
                            playVoiceText = HighSpeechQueque.Dequeue();
                        }
                        else if (NormalSpeechQueque.Count > 0)
                        {
                            playVoiceText = NormalSpeechQueque.Dequeue();
                        }
                        else
                        {
                            playVoiceText = string.Empty;
                        }

                        mTextToSpeech.Speak(playVoiceText, QueueMode.Add, null);
                        //mTextToSpeech.Speak()
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteSystemLog("StartPlayError:" + ex.Message);
            }

        }

        /// <summary>
        /// 播放背景音乐循环播放
        /// </summary>
        public void SpeakLoopVoice(object a)
        {
            try
            {
                if (fisloop!= null)
                {
                    //LogManager.WriteSystemLog("fisloop is null");
                    if (!playerloop.IsPlaying)
                    {
                        playerloop.Reset();
                        playerloop.SetDataSource(fisloop.FD);
                        playerloop.Prepare();
                        playerloop.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteSystemLog("SpeakBreake:" + ex.Message + ":" + ex.Source + ":" + ex);
            }
        }

        public void SpeakBreakeVoice()
        {
            try
            {
                if (fis == null)
                {
                    Java.IO.File file = new Java.IO.File(BreakePath);
                    if (File.Exists(BreakePath))
                    {
                        fis = new Java.IO.FileInputStream(file);
                    }
                }
                if (fis != null)
                {
                    player.Reset();
                    player.SetDataSource(fis.FD);
                    player.Prepare();
                    player.Start();
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteSystemLog("SpeakBreake:" + ex.Message + ":" + ex.Source + ":" + ex);
            }

        }
        public void SpeakDongVoice()
        {
            try
            {
                if (dongfile != null)
                {
                    player.Reset();
                    player.SetDataSource(dongfile.FD);
                    player.Prepare();
                    player.Start();
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteSystemLog("SpeakDongVoice:" + ex.Message + ":" + ex.Source + ":" + ex);
            }

        }
        private SpeechManagerBak()
        {

        }
        //播放操作语音
        public void SpeakActionVoice(string VoiceText)
        {
            if (!IsPlay)
            {
                CancelAllAsync();
                StartAllSpeak();
            }
            if (IsPlayActionVoice)
            {
                PlayAudioAsync(VoiceText, SpeechPriority.High);
            }

        }
        //只有在建立一个线程来进行语音播报
        //


        public void SpeakAsync(string textToSpeak)
        {
            lock (lockObj)
            {

            }
            //Logger.InfoFormat("加入语音播报队列 - {0}", textToSpeak);
        }

        /// <summary>
        /// 播放语音存在前面的一两个字吐字不清晰的问题，可以前面加上逗号或者顿号。
        /// 这个问题已经通过升级MCU 解决。
        /// </summary>
        /// <param name="voiceText"></param>
        /// <param name="priority"></param>
        public void PlayAudioAsync(string voiceText, SpeechPriority priority = SpeechPriority.Normal)
        {
            try
            {
                if (!string.IsNullOrEmpty(voiceText))
                {
                    voiceText = ",," + voiceText;
                    if (priority == SpeechPriority.High)
                    {
                        HighSpeechQueque.Enqueue(voiceText);
                    }
                    else if (priority == SpeechPriority.Normal)
                    {
                        NormalSpeechQueque.Enqueue(voiceText);
                    }
                }
                //表示高优先级的队列有语音需要播放则优先取出高优先级的
                //if (HighSpeechQueque.Count > 0)
                //{
                //voiceText = HighSpeechQueque.Dequeue();
                //}
                //else
                //{
                //voiceText = NormalSpeechQueque.Dequeue();
                //}
                //把这个东西加入一个队列里面，然后每次取出里面的按优先级排列
                //mTextToSpeech.Speak(voiceText, QueueMode.Add, null);
                //mTextToSpeech.Speak(voiceText, QueueMode.Add, null);
                //Task.Run(
                //    () =>
                //    {
                //        mTextToSpeech.Speak(voiceText, QueueMode.Add, null);
                //         //下面这种写法android版本低了可能不支持
                //         // mTextToSpeech.Speak(voiceText, QueueMode.Add, null, string.Empty);
                //     });
            }
            //记录错误日志
            catch (Exception ex)
            {
                LogManager.WriteSystemLog("PlayAudioAsynce:Voice"+voiceText+"Exp:"+ex.Message);
            }


        }
        public bool HasVoice
        {
            get { return mTextToSpeech.IsSpeaking; }
        }

        /// <summary>
        /// 停止语音播报 //是否需要释放资源
        /// </summary>
        public void CancelAllAsync(bool IsStop=true)
        {
            // IsPlay = false;
            HighSpeechQueque.Clear();
            NormalSpeechQueque.Clear();
            if (IsStop)
            {
                mTextToSpeech.Stop(); // 不管是否正在朗读TTS都被打断
            }
          

            //mTextToSpeech.Shutdown(); // 关闭，释放资源  
        }
       
        //停止所有的语音播报
        public void StopAllSpeak()
        {
            IsPlay = false;
        }

        public void StartAllSpeak()
        {
            IsPlay = true;
        }
        public void onStop()
        {
            mTextToSpeech.Stop(); ; // 不管是否正在朗读TTS都被打断  
            mTextToSpeech.Shutdown(); //关闭，释放资源 
            player.Release();
            playerloop.Reset();
        }
        void ISpeaker.SpeakAsync(string textToSpeak)
        {
          
        }


        void ISpeaker.PlayAudioAsync(string voiceText, SpeechPriority priority, Action callback)
        {
            PlayAudioAsync(voiceText, priority);
            // mTextToSpeech.Speak(voiceText, QueueMode.Add, null);
            if (callback != null)
            {
                callback();
            }

        }
        /// <summary>
        /// 直接播放播放完成之后进行一个回调。效果应该都一样的。
        /// </summary>
        /// <param name="voiceText"></param>
        /// <param name="callback"></param>
        void ISpeaker.PlayAudioAsync(string voiceText, Action callback)
        {
            //这样会导致语音优先级出现问题
           //这样Windows,
           
            mTextToSpeech.Speak(voiceText, QueueMode.Add, null);
            //等待当前语音播放完成。
            //这个容易出现死循环
            while (HasVoice);
            if (callback != null)
            {
                callback();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="status"></param>
        void IOnInitListener.OnInit(OperationResult status)
        {
            var result = this.mTextToSpeech.SetLanguage(Locale.Default);
            if (result != LanguageAvailableResult.Available)
            {
                LogManager.WriteSystemLog("ReInitSpeakListenr");
                result = this.mTextToSpeech.SetLanguage(Locale.Default);
            }
        }
    }


}
