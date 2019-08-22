using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEvaluationCal.Calculates
{
    public partial class DemoCode: BaseMethods
    {
        public bool Calc(IDictionary<string, IList<IDictionary<string, string>>> dataExtra, 
            ref IDictionary<string, IDictionary<string, IList<IDictionary<string, string>>>> retData,
            ref string err)
        {
            /************************ 代码开始 *********************/
            var Len =GetDataCount("抗压.S_HNT");
            bool mAllHg = false;
            var mjcjg = "不合格";
            var jsbeizhu = "";
            var jgsm = "";
            for (int i = 0; i < Len; i++)
            {
                if (i == 0)
                {
                    mAllHg = true;
                }
                var Index = i;

                var ZZRQ = GetSafeDate(GetData("抗压.S_HNT.ZZRQ", Index));
                var SYRQ = GetSafeDate(GetData("抗压.M_HNT.SYRQ"));
                var yhtj = GetData("抗压.S_HNT.YHTJ", Index);

                var LQ = 0;
                if (ZZRQ <= Date19000101 || SYRQ <= Date19000101)
                {
                    LQ = 0;
                }
                else
                {
                    LQ = (SYRQ - ZZRQ).Days;
                    if (LQ != 28 && yhtj.Trim() == "标准")
                    {
                        LQ = 28;
                    }
                }

                var a = GetData("抗压.S_HNT.KYHZ1", Index);
                CheckEmpty(a, "抗压荷重1 不能为空");
                var b = GetData("抗压.S_HNT.KYHZ2", Index);
                CheckEmpty(b, "抗压荷重2 不能为空");
                var c = GetData("抗压.S_HNT.KYHZ3", Index);
                CheckEmpty(c, "抗压荷重3 不能为空");
                var d = GetData("抗压.S_HNT.SJCC", Index);
                CheckEmpty(d, "受压面边长1 不能为空");
                var SJDJ = GetData("抗压.S_HNT.SJDJ", Index);
                CheckEmpty(SJDJ, "设计等级 不能为空");
                var e = GetInt(d);

                var HSXS = 1.0;
                if (d == "100")
                {
                    HSXS = 0.95;
                }
                else if (d == "200")
                {
                    HSXS = 1.05;
                }

                var mttjhsxs = 1.0;

                var KYQD1 = Round(GetDouble(a) * 1000 * HSXS * mttjhsxs / (e * e), 1);
                var KYQD2 = Round(GetDouble(b) * 1000 * HSXS * mttjhsxs / (e * e), 1);
                var KYQD3 = Round(GetDouble(c) * 1000 * HSXS * mttjhsxs / (e * e), 1);

                var MinKYQD = GetMin(KYQD1, KYQD2, KYQD3);
                var MiddleKYQD = GetMiddle(KYQD1, KYQD2, KYQD3);
                var MaxKYQD = GetMax(KYQD1, KYQD2, KYQD3);

                var KYPJ = "";
                var ddsjqd = "";
                var hzcase = 1;
                var jcjg = "";

                var midavg = false;

                var mSz = 0.0;
                var mQdyq = 0.0;
                if (!String.IsNullOrEmpty(SJDJ))
                {
                    if (SJDJ.ToUpper()[0] != 'c')
                    {
                        SJDJ = "C" + SJDJ;
                    }
                    var sz = GetExtraData("BZ_HNT_DJ.SZ", x => x["mc"].Trim().Equals(SJDJ, StringComparison.OrdinalIgnoreCase)).Trim();
                    var qdyq = GetExtraData("BZ_HNT_DJ.QDYQ", x => x["mc"].Trim().Equals(SJDJ, StringComparison.OrdinalIgnoreCase)).Trim();
                    if (!String.IsNullOrEmpty(sz))
                        mSz = GetDouble(sz);
                    if (!String.IsNullOrEmpty(qdyq))
                        mQdyq = GetDouble(qdyq);
                }

                var BaiFenBi1 = ((MiddleKYQD - MinKYQD) / MiddleKYQD) * 100;
                var BaiFenBi2 = ((MaxKYQD - MiddleKYQD) / MiddleKYQD) * 100;

                if ((BaiFenBi2 > 15 && BaiFenBi1 <= 15))
                {
                    hzcase = 2;
                    jsbeizhu = "最大最小强度值其中一个超出中间值的15%,试验结果取中间值";
                    KYPJ = RoundEx(MiddleKYQD, 1);
                    if (mSz > 0)
                    {
                        ddsjqd = Round(GetDouble(KYPJ) / mSz * 100, 0).ToString();
                        if (mQdyq > 0)
                        {
                            if (GetDouble(ddsjqd) > mQdyq)
                            {
                                jcjg = "合格";
                            }
                            else
                            {
                                jcjg = "不合格";
                                mAllHg = false;
                            }
                        }
                        else
                        {
                            jcjg = "----";
                            mjcjg = "----";
                            mAllHg = false;
                        }
                    }
                    else
                    {
                        jcjg = "----";
                        mjcjg = "----";
                        ddsjqd = "----";
                        mAllHg = false;
                    }
                    midavg = true;
                }
                else if (BaiFenBi1 > 15 && BaiFenBi2 <= 15)
                {
                    hzcase = 3;
                    jsbeizhu = "最大最小强度值其中一个超出中间值的15%,试验结果取中间值";
                    KYPJ = RoundEx(MiddleKYQD, 1);
                    if (mSz > 0)
                    {
                        ddsjqd = Round(GetDouble(KYPJ) / mSz * 100, 0).ToString();
                        if (mQdyq > 0)
                        {
                            if (GetDouble(ddsjqd) > mQdyq)
                            {
                                jcjg = "合格";
                            }
                            else
                            {
                                jcjg = "不合格";
                                mAllHg = false;
                            }
                        }
                        else
                        {
                            jcjg = "----";
                            mjcjg = "----";
                            mAllHg = false;
                        }
                    }
                    else
                    {
                        jcjg = "----";
                        mjcjg = "----";
                        ddsjqd = "----";
                        mAllHg = false;
                    }
                    midavg = true;
                }
                else if (BaiFenBi1 > 15 && BaiFenBi2 > 15)
                {
                    KYPJ = "试验结果无效";
                    ddsjqd = "不作评定";
                    hzcase = 1;
                    jcjg = "不合格";
                    jsbeizhu = "最大最小强度值超出中间值的15%,试验结果不作评定依据";
                    mAllHg = false;
                }
                else
                {
                    KYPJ = (RoundEx((KYQD1 + KYQD2 + KYQD3) / 3, 1)).ToString();
                    hzcase = 4;
                    if (mSz > 0)
                    {
                        ddsjqd = Round(GetDouble(KYPJ) / mSz * 100, 0).ToString();
                        if (mQdyq > 0)
                        {
                            if (GetDouble(ddsjqd) > mQdyq)
                            {
                                jcjg = "合格";
                            }
                            else
                            {
                                jcjg = "不合格";
                                mAllHg = false;
                            }
                        }
                        else
                        {
                            jcjg = "----";
                            mjcjg = "----";
                            mAllHg = false;
                        }
                    }
                    else
                    {
                        jcjg = "----";
                        mjcjg = "----";
                        ddsjqd = "----";
                        mAllHg = false;
                    }
                }

                if (KYPJ == "试验结果无效")
                {
                    jgsm = "该组试样强度代表值无效。";
                }
                else
                {
                    jgsm = "该组试样强度代表值" + KYPJ + "MPa，" + "达到设计强度" + ddsjqd + "%。";
                }

                SetData("抗压.S_HNT.KYQD1", RoundEx(KYQD1, 1), Index);
                SetData("抗压.S_HNT.KYQD2", RoundEx(KYQD2, 1), Index);
                SetData("抗压.S_HNT.KYQD3", RoundEx(KYQD3, 1), Index);
                SetData("抗压.S_HNT.KYPJ", KYPJ, Index);
                SetData("抗压.S_HNT.HSXS", HSXS.ToString(), Index);
                SetData("抗压.S_HNT.TTJHSXS", mttjhsxs.ToString(), Index);
                SetData("抗压.S_HNT.DDSJQD", ddsjqd, Index);
                SetData("抗压.S_HNT.LQ", LQ.ToString(), Index);
                SetData("抗压.S_HNT.JCJG", jcjg, Index);
            }
            if (mAllHg && mjcjg != "----")
            {
                mjcjg = "合格";
            }
            SetData("抗压.M_HNT.JCJG", mjcjg);
            SetData("抗压.M_HNT.JSBEIZHU", jsbeizhu);
            /************************ 代码结束 *********************/
            return true;
        }

    }
}
