using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace StockMarket.Models.Computation
{
    public class Compute
    {
        public double Change_Percent(double? value1, double? value2)
        {
            return Math.Round(Convert.ToDouble((value1 - value2) / value2 * 100), 2);
        }
        public string Value(double money)
        {
            string number = Convert.ToString(money);
            string firstpart;
            string lastpart;

            char points = '.';
            StringBuilder stringBuilder = new StringBuilder();
            if (!number.Contains(points))
            {
                stringBuilder = stringBuilder.Append(number).Append(".00");
                number = stringBuilder.ToString();
            }
            StringBuilder builder = new StringBuilder();

            if (number.Length <= 7)
            {
                return number;
            }
            else if (number.Length > 7 && number.Length <= 8)
            {
                firstpart = number.Substring(0, 2);//Take First Two Digits
                lastpart = number.Substring(number.Length - 3);//Take last Three Values
                builder = builder.Append(firstpart).Append(lastpart).Append("K");
                return builder.ToString();
            }
            else if (number.Length > 8 && number.Length <= 9)
            {
                firstpart = number.Substring(0, 3);
                lastpart = number.Substring(number.Length - 3);
                builder = builder.Append(firstpart).Append(lastpart).Append("K");
                return builder.ToString();
            }
            else if (number.Length > 9 && number.Length <= 10)
            {
                firstpart = number.Substring(0, 1);
                lastpart = number.Substring(number.Length - 3);
                builder = builder.Append(firstpart).Append(lastpart).Append("M");
                return builder.ToString();
            }
            else if (number.Length > 10 && number.Length <= 11)
            {
                firstpart = number.Substring(0, 2);
                lastpart = number.Substring(number.Length - 3);
                builder = builder.Append(firstpart).Append(lastpart).Append("M");
                return builder.ToString();
            }
            else if (number.Length > 11 && number.Length <= 12)
            {
                firstpart = number.Substring(0, 3);
                lastpart = number.Substring(number.Length - 3);
                builder = builder.Append(firstpart).Append(lastpart).Append("M");
                return builder.ToString();
            }
            else if (number.Length > 12 && number.Length <= 13)
            {
                firstpart = number.Substring(0, 1);
                lastpart = number.Substring(number.Length - 3);
                builder = builder.Append(firstpart).Append(lastpart).Append("B");
                return builder.ToString();
            }
            else if (number.Length > 13 && number.Length <= 14)
            {
                firstpart = number.Substring(0, 2);
                lastpart = number.Substring(number.Length - 3);
                builder = builder.Append(firstpart).Append(lastpart).Append("B");
                return builder.ToString();
            }
            else if (number.Length > 14 && number.Length <= 15)
            {
                firstpart = number.Substring(0, 3);
                lastpart = number.Substring(number.Length - 3);
                builder = builder.Append(firstpart).Append(lastpart).Append("B");
                return builder.ToString();
            }
            return builder.ToString();
        }
        public string MarketCap(object sp)
        {
            //string number = Convert.ToString(Convert.ToSingle(sp));
            //StringBuilder csbuild = new StringBuilder();
            ////return csbuild.Append(number.Substring(0, 3)).Append(number.Substring(number.Length - 2)).Append("B").ToString();
            //number = Value(double.Parse(number));
            //return number;
           return string.Format("{0:#,###0}", sp);
        }
        public string Truncate(string value) // Short the Words of Some Companies ex. This is Apple  => This Is
        {
            string[] split = value.Split(' ');
            int countSpaces = value.Count(Char.IsWhiteSpace);
            if(countSpaces >= 2)
            {
                return string.Concat(split[0], " ", split[1]);
            }
            
            return value;
        }
        public string JsonNewsCreator(List<string> urldata) // For the News
        {
            StringBuilder builder = new StringBuilder();
            foreach (string items in urldata)
            {
                string new_wrod = items.Replace("]", ",").Replace("[", "");
                builder.Append(new_wrod);
            }
            builder = new StringBuilder(builder.ToString().Substring(0, builder.ToString().Length - 1));
            string finalword = builder.ToString();
            StringBuilder builders = new StringBuilder();
            return builders.Append("[").Append(finalword).Append("]").ToString();
        }
        public string JsonPortFolioCreator(List<string> urldata) // For the Companies Reterived from Database in regarding to Json Format
        {
            StringBuilder builder = new StringBuilder();
            foreach (string items in urldata)
            {
                string new_wrod = items.Replace("]", ",").Replace("[", "");
                builder.Append(new_wrod).Append(",");
            }
            builder = new StringBuilder(builder.ToString().Substring(0, builder.ToString().Length - 1));
            string finalword = builder.ToString();
            StringBuilder builders = new StringBuilder();
            return builders.Append("[").Append(finalword).Append("]").ToString();
        }
    }
}