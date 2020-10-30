using KuberaManager.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Database
{
    // Contains information about "once in an account's lifetime" scenarios
    // eg. Tutorial complete, Able to do a certain quest,..
    // Meanin behind integers is defined in Models.Data.CompletionDataDefinitions
    public class AccountCompletionData
    {
        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public CompletionDataDefinition Definition { get; set; }

        /// <summary>
        /// Check if accoun has a specific definition
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        internal static bool AccountHasDefinition(int accountId, CompletionDataDefinition def)
        {
            using (var db = new kuberaDbContext())
            {
                return db.AccountCompletionData
                    .Where(x => x.AccountId == accountId)
                    .Where(x => x.Definition == def)
                    .Count() > 0;
            }
        }

        /// <summary>
        /// Get all definitions for an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static List<CompletionDataDefinition> GetDefinitions(int accountId)
        {
            // Get DB matches
            List<AccountCompletionData> dbMatches = null;
            using (var db = new kuberaDbContext())
            {
                dbMatches = db.AccountCompletionData
                    .Where(x => x.AccountId == accountId)
                    .ToList();
            }

            // Generate result
            List<CompletionDataDefinition> result = new List<CompletionDataDefinition>();
            dbMatches.ForEach(x => result.Add(x.Definition));

            return result;
        }

        /// <summary>
        /// Add a definition to an account if not exists
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="def"></param>
        public static void AddDefinition(int accountId, CompletionDataDefinition def)
        {
            // Do nothing if already defined
            if (AccountHasDefinition(accountId, def))
                return;

            // Create object
            AccountCompletionData acd = new AccountCompletionData()
            {
                AccountId = accountId,
                Definition = def,
            };

            // insert
            using (var db = new kuberaDbContext())
            {
                db.AccountCompletionData.Add(acd);
                db.SaveChanges();
            }
        }
    }
}
