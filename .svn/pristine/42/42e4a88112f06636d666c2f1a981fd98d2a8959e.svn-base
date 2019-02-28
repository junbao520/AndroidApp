using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class ExamItemContext : INotifyPropertyChanged
    {
        private string itemCode = string.Empty;
        private string itemName = string.Empty;

        private ExamItemState examItemState = ExamItemState.None;

        public string ItemCode
        {
            set
            {
                if (itemCode != value)
                {
                    itemCode = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ItemCode"));
                    }
                }
            }
            get
            {
                return itemCode;
            }
        }

        public string ItemName
        {
            set
            {
                if (itemName != value)
                {
                    itemName = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ItemName"));
                    }
                }
            }
            get
            {
                return itemName;
            }
        }

        public ExamItemState ExamItemState
        {
            set
            {
                if (examItemState != value)
                {
                    examItemState = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ExamItemState"));
                    }
                }
            }
            get
            {
                return examItemState;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
