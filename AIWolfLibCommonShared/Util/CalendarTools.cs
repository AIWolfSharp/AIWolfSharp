using System;

namespace AIWolf.Common.Util
{
    /// <summary>
    /// カレンダーに関するstaticメソッドを提供するクラス（抜粋）
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>

    /// </summary>
    public class CalendarTools
    {
        /// <summary>
        /// DateTimeを yyyy/MM/dd HH:mm:ss 形式の文字列に直して返す
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// longを yyyy/MM/dd HH:mm:ss 形式の文字列に直して返す
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToDateTime(long time)
        {
            return ToDateTime(new DateTime(time));
        }

        /// <summary>
        /// yyyy/MM/dd HH:mm:ss 形式の文字列で現在時刻を返す
        /// <para>
        /// Added by otsuki.
        /// </para>
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToDateTime()
        {
            return ToDateTime(DateTime.Now);
        }

        /// <summary>
        /// DateTimeを yyyyMMddHHmmss 形式の文字列に直して返す
        /// <para>
        /// Added by otsuki.
        /// </para>
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToTimeString(DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// 現在時刻を yyyyMMddHHmmss 形式の文字列に直して返す
        /// <para>
        /// Added by otsuki.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public static string ToTimeString()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
