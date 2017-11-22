using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Client.Exceptions;
using Lykke.Service.ClientAssetRule.Client.Models;

namespace Lykke.Service.ClientAssetRule.Client
{
    /// <summary>
    /// Contains methods for work with Lykke.Service.ClientAssetRule API.
    /// </summary>
    public interface IClientAssetRuleClient
    {
        /// <summary>
        /// Returns all rules.
        /// </summary>
        /// <returns>The list of rules.</returns>
        Task<IEnumerable<RuleModel>> GetRulesAsync();

        /// <summary>
        /// Returns rule details by specified id.
        /// </summary>
        /// <param name="ruleId">The rule id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task<RuleModel> GetRuleByIdAsync(string ruleId);

        /// <summary>
        /// Adds the rule.
        /// </summary>
        /// <param name="model">The model what describe a rule.</param>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task AddRuleAsync(RuleModel model);

        /// <summary>
        /// Updates the rule.
        /// </summary>
        /// <param name="model">The model what describe a rule.</param>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task UpdateRuleAsync(RuleModel model);

        /// <summary>
        /// Deletes the rule by specified id.
        /// </summary>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task DeleteRuleAsync(string ruleId);
    }
}
