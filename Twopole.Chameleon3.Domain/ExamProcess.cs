using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Twopole.Chameleon3.Domain
{
    /// <summary>
    /// 考试过程类
    /// </summary>
    public class ExamProcess
    {
        public ExamProcess()
        {
            lstExamProcess = new List<ExamItemProcess>();
        }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Score { get; set; }

       public List<ExamItemProcess> lstExamProcess { get; set; }

        /// <summary>
        /// 添加考试过程信息
        /// </summary>
        /// <param name="process"></param>
        public void AddExamItem(ExamItemProcess process)
        {

            var examItemProcess = GetExamProcess(process.ExamItemCodes);
            if (examItemProcess == null)
            {
                lstExamProcess.Add(process);
            }
            else
            {
                lstExamProcess.Remove(examItemProcess);
                lstExamProcess.Add(process);
            }
        }
        /// <summary>
        /// 获取考试过程信息
        /// </summary>
        /// <param name="ExamItemCodes"></param>
        /// <returns></returns>
        public ExamItemProcess GetExamProcess(string ExamItemCodes)
        {
            //多线程会有问题 要么就要加线程锁 才行最简单就是 直接使用for循环
            for (int i = 0; i < lstExamProcess.Count; i++)
            {
                if (lstExamProcess[i].ExamItemCodes==ExamItemCodes)
                {
                    return lstExamProcess[i];
                }
            }
            return null;
            //if (lstExamProcess.Where(s => s.ExamItemCodes == ExamItemCodes).Count() == 0)
            //{
            //    return null;
            //}
            //else
            //{
            //    return lstExamProcess.Where(s => s.ExamItemCodes == ExamItemCodes).FirstOrDefault();
            //}
        }
    }

    public class ExamItemProcess {

        public ExamItemProcess()
        {
            this.ExamItemCodes = string.Empty;
            this.ExamItemName = string.Empty;
            this.Status = ExamItemSatatus.Process;
            this.BeginTime = null;
            this.EndTime = null;
            this.IsSuccess = true;
        }

        public string ExamItemCodes { get; set; }
        public string ExamItemName { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }

        public ExamItemSatatus Status { get; set; }

        public bool IsSuccess { get; set; }
    }

    public enum ExamItemSatatus {
        None=0,
        Process=1,
        Finish=2
    }


}