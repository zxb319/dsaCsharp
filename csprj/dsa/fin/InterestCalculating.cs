using System;
using System.Collections.Generic;
using System.Text;

namespace csprj.dsa.fin {
    public class InterestCalculating {
        /// <summary>
        /// 根据名义利率计算年化利率
        /// </summary>
        /// <param name="named_rate">名义利率</param>
        /// <param name="payTimes">每年支付次数</param>
        /// <returns></returns>
        public static double AnnulizedRate(double named_rate, double payTimes) => 
            Math.Pow(1 + named_rate, 1 / payTimes) - 1;

        public static double Pv(IEnumerable<double> values, double rate) {
            double ret = 0;
            double discountRate = 1;
            foreach (var v in values) {
                ret += (v / discountRate);
                discountRate /= (1 + rate);
            }

            return ret;
        }
        /// <summary>
        /// 内部收益率
        /// </summary>
        /// <param name="values">现金流</param>
        /// <returns></returns>
        public static double Irr(IEnumerable<double> values) {
            return ZMath.Root(r => Pv(values, r), 1e-9, 1e9);
        }

        public static double Annuity(double pv, double rate, int payTimes) {
            return pv * rate / (1 - 1 / Math.Pow(1 + rate, payTimes));
        }

        /// <summary>
        /// 
        /// </summary>
        public static double PayTimes(double pv, double annuity, double rate) {
            return Math.Log(annuity / (pv * rate - annuity)) / Math.Log(1 + rate);
        }

        /// <summary>
        /// 计算等额本金的每期数据
        /// </summary>
        public static List<Dictionary<string, double>> Debj(double pv, double rate, int payTimes) {
            double bj = pv / payTimes;
            var ret = new List<Dictionary<string, double>>();
            for (int i = 1; i <= payTimes; ++i) {
                var cur = new Dictionary<string, double>();
                var lx = pv * rate;
                pv -= bj;
                cur["期数"] = i;
                cur["本金"] = bj;
                cur["利息"] = lx;
                cur["本息"] = bj + lx;
                cur["剩余本金"] = pv;
                ret.Add(cur);
            }

            return ret;
        }

        /// <summary>
        /// 计算等额本息的每期数据
        /// </summary>
        public static List<Dictionary<string, double>> Debx(double pv, double rate, int payTimes) {
            double bx = Annuity(pv,rate, payTimes);
            var ret = new List<Dictionary<string, double>>();
            for (int i = 1; i <= payTimes; ++i) {
                var cur = new Dictionary<string, double>();
                var lx = pv * rate;
                var bj = bx - lx;
                pv -= bj;
                cur["期数"] = i;
                cur["本金"] = bj;
                cur["利息"] = lx;
                cur["本息"] = bx;
                cur["剩余本金"] = pv;
                ret.Add(cur);
            }

            return ret;
        }
    }
}
