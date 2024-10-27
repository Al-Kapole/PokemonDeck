using UnityEngine;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Extensions
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Extension for AsyncOpreation so that UnityWebRequest can work with async/await
        /// </summary>
        /// <param name="asyncOp"></param>
        /// <returns></returns>
        public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
        {
            var tcs = new TaskCompletionSource<object>();
            asyncOp.completed += obj => { tcs.SetResult(null); };
            return ((Task)tcs.Task).GetAwaiter();
        }

        /// <summary>
        /// Extension for string.
        /// It removes first and last letter from the string.
        /// Created this cause SimpleJSON returns the string with quotes.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrimSideQuotes(this string value)
        {
            value = value.Remove(0, 1);
            value = value.Remove(value.Length - 1, 1);
            return value;
        }
    }
}