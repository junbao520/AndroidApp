using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    public static class DeductionRuleCodes
    {

        //可以被重复计算的扣分规则
        public static IList<string> RepeatDeductionRuleCodes
        {
            get
            {
                return new List<string>()
                  {
                      RC30202,//起步时车辆后溜，但后溜距离小于30 cm
                      RC30204,//起步或行驶中挂错挡，不能及时纠正
                      RC30205,//起步、转向、变更车道、超车、停车前不使用或错误使用转向灯
                      RC30206,//起步、转向、变更车道、超车、停车前，开转向灯少于3 s即转向
                      RC30210,//因操作不当造成发动机熄火一次
                  };
            }

        }

        #region 科目二扣分规则

        public const string RC10101 = "10101";
        public const string RC10205 = "10205";
        public const string RC10206 = "10206";
        public const string RC10210 = "10210";

        #endregion

        //todo 这里需要重构每个地区的区分开来
        //恩施添加
        public const string RC30001 = "30001";
        public const string RC30002 = "30002";
        public const string RC30003 = "30003";
        public const string RC30004 = "30004";
        public const string RC30005 = "30005";
        public const string RC30006 = "30006";
        public const string RC30007 = "30007";

        public const string RC30101 = "30101";
        public const string RC30102 = "30102";
        public const string RC30103 = "30103";
        public const string RC30104 = "30104";
        public const string RC30105 = "30105";
        public const string RC30106 = "30106";
        public const string RC30107 = "30107";
        public const string RC30108 = "30108";
        public const string RC30109 = "30109";
        public const string RC30110 = "30110";
        public const string RC30111 = "30111";
        public const string RC30112 = "30112";
        public const string RC30113 = "30113";
        public const string RC30114 = "30114";
        public const string RC30115 = "30115";
        public const string RC30116 = "30116";
        public const string RC30117 = "30117";
        public const string RC30118 = "30118";
        public const string RC30119 = "30119";
        public const string RC30120 = "30120";
        public const string RC30121 = "30121";
        public const string RC30122 = "30122";
        public const string RC30123 = "30123";
        public const string RC30124 = "30124";
        public const string RC30125 = "30125";
        public const string RC30126 = "30126";
        public const string RC30127 = "30127";
        public const string RC30128 = "30128";
        public const string RC30129 = "30129";
        public const string RC30130 = "30130";
        public const string RC30131 = "30131";
        public const string RC30132 = "30132";
        public const string RC30135 = "30135";
        public const string RC30137 = "30137";      
        public const string RC30140 = "30140";
        public const string RC30141 = "30141";
        public const string RC30201 = "30201";
        public const string RC30202 = "30202";
        public const string RC30203 = "30203";
        public const string RC30204 = "30204";
        public const string RC30205 = "30205";
        public const string RC30206 = "30206";
        public const string RC30207 = "30207";
        public const string RC30208 = "30208";
        public const string RC30209 = "30209";
        public const string RC30210 = "30210";
        public const string RC30211 = "30211";
        public const string RC30212 = "30212";
        public const string RC30213 = "30213";
        public const string RC30214 = "30214";
        public const string RC30215 = "30215";
        public const string RC30221 = "30221";
        public const string RC30222 = "30222";
        public const string RC30223 = "30223";
        public const string RC30224 = "30224";
        public const string RC30225 = "30225";
        public const string RC3021205 = "3021205";
        public const string RC30156 = "30156";




        public const string RC40001 = "40001";
        public const string RC40002 = "40002";
        public const string RC40003 = "40003";
        public const string RC40004 = "40004";
        public const string RC40005 = "40005";
        public const string RC40101 = "40101";
        public const string RC40102 = "40102";
        public const string RC40201 = "40201";
        public const string RC40202 = "40202";
        public const string RC40203 = "40203";
        public const string RC40204 = "40204";
        public const string RC40205 = "40205";
        public const string RC40206 = "40206";
        public const string RC40207 = "40207";
        public const string RC40208 = "40208";
        public const string RC40209 = "40209";
        public const string RC40210 = "40210";
        public const string RC40211 = "40211";
        public const string RC40212 = "40212";
        public const string RC40213 = "40213";
        public const string RC40214 = "40214";// 起步不松手刹但能及时纠正
        public const string RC40301 = "40301";
        public const string RC40302 = "40302";
        public const string RC40303 = "40303";
        public const string RC40304 = "40304";
        public const string RC40305 = "40305";
        public const string RC40306 = "40306";
        public const string RC40307 = "40307";

        /// <summary>
        /// 加减档
        /// </summary>
        public const string RC40401 = "40401";
        public const string RC40402 = "40402";
        public const string RC40403 = "40403";
        public const string RC40404 = "40404";
        public const string RC40405 = "40405";
        public const string RC40406 = "40406";
        public const string RC40408 = "40408";

        /// <summary>
        /// 加档
        /// </summary>
        public const string RC40411 = "40411";
        public const string RC40412 = "40412";

        public const string RC40501 = "40501";
        public const string RC40502 = "40502";
        public const string RC40503 = "40503";

        public const string RC40601 = "40601";
        public const string RC40602 = "40602";
        public const string RC40603 = "40603";
        public const string RC40604 = "40604";
        public const string RC40605 = "40605";
        public const string RC40606 = "40606";
        public const string RC40607 = "40607";
        public const string RC40608 = "40608";
        public const string RC40609 = "40609";
        public const string RC40610 = "40610";
        public const string RC40611 = "40611";
        public const string RC40612 = "40612";
        public const string RC40613 = "40613";
        public const string RC40614 = "40614";
        public const string RC40615 = "40615";
        public const string RC40616 = "40616";
        public const string RC40617 = "40617";

        public const string RC40701 = "40701";
        public const string RC40702 = "40702";
        public const string RC40703 = "40703";
        public const string RC40704 = "40704";
        public const string RC40801 = "40801";
        public const string RC40802 = "40802";
        public const string RC40803 = "40803";
        public const string RC40804 = "40804";
        public const string RC40900 = "40900";

        public const string RC40901 = "40901";
        public const string RC40902 = "40902";
        public const string RC40903 = "40903";
        public const string RC40904 = "40904";
        public const string RC41001 = "41001";
        public const string RC41002 = "41002";
        public const string RC41003 = "41003";
        public const string RC41101 = "41101";
        public const string RC41102 = "41102";
        public const string RC41103 = "41103";
        public const string RC41201 = "41201";
        public const string RC41202 = "41202";
        public const string RC41203 = "41203";
        public const string RC41301 = "41301";
        public const string RC41302 = "41302";
        public const string RC41303 = "41303";
        public const string RC41304 = "41304";
        public const string RC41305 = "41305";
        public const string RC41401 = "41401";
        public const string RC41402 = "41402";
        public const string RC41403 = "41403";
        public const string RC41404 = "41404";
        public const string RC41405 = "41405";
        public const string RC41406 = "41406";
        public const string RC41407 = "41407";
        public const string RC41408 = "41408";
        public const string RC41501 = "41501";
        public const string RC41502 = "41502";
        public const string RC41503 = "41503";
        public const string RC41504 = "41504";
        public const string RC41505 = "41505";
        public const string RC41506 = "41506";
        public const string RC41508 = "41508";
        public const string RC41601 = "41601";
        public const string RC41602 = "41602";
        public const string RC41603 = "41603";
        public const string RC41604 = "41604";
        public const string RC41605 = "41605";
        public const string RC41606 = "41606";
        public const string RC41607 = "41607";
        public const string RC41608 = "41608";
        public const string RC41609 = "41609";
        public const string RC41610 = "41610";
        public const string RC41611 = "41611";
        public const string RC41701 = "41701";
        public const string RC41705 = "41705";
        public const string RC42801 = "42801";
        public const string RC400011 = "400011";
        


        /// <summary>
        /// 新增灯光模拟前未关闭所有灯光
        /// </summary>
        public const string RC41619 = "41619";

        //SubRules
        public const string SRC3010301 = "3010301";
        public const string SRC3010302 = "3010302";
        public const string SRC3010303 = "3010303";
        public const string SRC3010304 = "3010304";
        public const string SRC3010305 = "3010305";
        public const string SRC3011601 = "3011601";
        public const string SRC3010801 = "3010801";
        public const string SRC3010802 = "3010802";
        public const string SRC3011001 = "3011001";
        public const string SRC3011101 = "3011101";
        public const string SRC3011102 = "3011102";
        public const string SRC3011301 = "3011301";
        public const string SRC3011302 = "3011302";
        public const string SRC3011701 = "3011701";
        public const string SRC3011702 = "3011702";
        public const string SRC301401  = "301401";

        public const string SRC3020501 = "3020501";
        public const string SRC3020502 = "3020502";
        public const string SRC3020503 = "3020503";
        public const string SRC3020504 = "3020504";
        public const string SRC3020505 = "3020505";
        public const string SRC3020506 = "3020506";
        

        public const string SRC4040201 = "4040201";//山东济宁档位和速度不匹配扣100分
        public const string SRC4060701 = "4060701";//山东济宁停车后，未拉紧驻车制动器扣100分

        public const string SRC4021101 = "4021101";
        public const string SRC4021102 = "4021102";
        public const string SRC4021103 = "4021103";

        public const string SRC3020601 = "3020601";
        public const string SRC3020602 = "3020602";
        public const string SRC3020603 = "3020603";
        public const string SRC3020604 = "3020604";
        public const string SRC3020605 = "3020605";
        public const string SRC3020606 = "3020606";
        public const string SRC3020607 = "3020607";


        public const string SRC4160101 = "4160101";
        public const string SRC4160102 = "4160102";
        public const string SRC4160103 = "4160103";
        public const string SRC4160104 = "4160104";
        public const string SRC4160105 = "4160105";
        public const string SRC4160106 = "4160106";
        public const string SRC4160107 = "4160107";
        public const string SRC4160108 = "4160108";
        public const string SRC4160109 = "4160109";

        public const string SRC4160301 = "4160301";
        public const string SRC4160302 = "4160302"; 
        public const string SRC4160303 = "4160303";
        public const string SRC4160304 = "4160304";
        public const string SRC4160305 = "4160305"; 


    }
}
