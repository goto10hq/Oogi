using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace Oogi
{
    public static class Core
    {                        
        /// <summary>
        /// Get entity expression.
        /// </summary>        
        public static string ToEntity<T>() where T : BaseEntity
        {
            var n = (T)Activator.CreateInstance(typeof(T), null);
            return n.Entity;
        }

        /// <summary>
        /// Get sql query from sql query spec.
        /// </summary>        
        public static string ToSqlQuery(this SqlQuerySpec sqs)
        {
            if (sqs == null)
                throw new ArgumentNullException(nameof(sqs));

            var r = sqs.QueryText;

            if (sqs.Parameters != null &&
                sqs.Parameters.Any())
            {
                foreach (var p in sqs.Parameters)
                {
                    var v = Converter.Process(p.Value);
                    r = r.Replace(p.Name, v);                    
                }
            }

            return r;
        }      

        /// <summary>
        /// Execute db action with retries.
        /// </summary>
        internal static async Task<T2> ExecuteWithRetriesAsync<T2>(Func<Task<T2>> function)
        {
            while (true)
            {
                TimeSpan sleepTime;

                try
                {
                    return await function();
                }
                catch (DocumentClientException de)
                {
                    if (de.StatusCode != null &&
                        ((int)de.StatusCode != 429 &&
                        (int)de.StatusCode != 503))
                    {
                        throw;
                    }
                    sleepTime = de.RetryAfter;
                }
                catch (AggregateException ae)
                {
                    if (!(ae.InnerException is DocumentClientException))
                    {
                        throw;
                    }

                    var de = (DocumentClientException)ae.InnerException;
                    if (de.StatusCode != null &&
                        ((int)de.StatusCode != 429 &&
                        (int)de.StatusCode != 503))
                    {
                        throw;
                    }

                    sleepTime = de.RetryAfter;
                }

                await Task.Delay(sleepTime);
            }
        }
    }
}
