using System;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Infrastructure
{
    public interface ISpeaker
    {
        bool HasVoice { get; }

        /// <summary>
        /// 异步执行语音播报
        /// </summary>
        /// <param name="textToSpeak">语音文本</param>
        /// <param name="priority">优先级</param>
        /// <param name="callback">语音播报后执行的任务</param>
        void SpeakAsync(string textToSpeak);

        /// <summary>
        /// 异步播放音频
        /// </summary>
        /// <param name="voiceFile"></param>
        /// <param name="priority"></param>
        /// <param name="callback"></param>
        void  PlayAudioAsync(string voiceText, SpeechPriority priority=SpeechPriority.Normal, Action callback = null);
       
       void CancelAllAsync(bool IsStop=true);
        void SpeakBreakeVoice();
        void SpeakDongVoice();
        void onStop();
        //播放操作语音
        void SpeakActionVoice(string VoiceText);
        void PlayAudioAsync(string voiceText, Action callback);

       void  SpeakLoopVoice(object a);

       void  InitLoopPlyaMediaTimer();

        void StopAllSpeak();
    }
}
