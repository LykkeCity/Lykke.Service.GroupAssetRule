using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Client.AutorestClient;
using Lykke.Service.ClientAssetRule.Client.AutorestClient.Models;
using Lykke.Service.ClientAssetRule.Client.Exceptions;
using RuleModel = Lykke.Service.ClientAssetRule.Client.Models.RuleModel;

namespace Lykke.Service.ClientAssetRule.Client
{
    /// <summary>
    /// Contains methods for work with Lykke.Service.ClientAssetRule API.
    /// </summary>
    public class ClientAssetRuleClient : IClientAssetRuleClient, IDisposable
    {
        private ClientAssetRuleAPI _service;

        /// <summary>
        /// Initializes a new instance of <see cref="ClientAssetRuleClient"/>.
        /// </summary>
        /// <param name="serviceUrl">The client assest rules service URL.</param>
        public ClientAssetRuleClient(string serviceUrl)
        {
            _service = new ClientAssetRuleAPI(new Uri(serviceUrl));
        }

        /// <summary>
        /// Returns all rules.
        /// </summary>
        /// <returns>The list of rules.</returns>
        public async Task<IEnumerable<RuleModel>> GetRulesAsync()
        {
            IEnumerable<AutorestClient.Models.RuleModel> rules =
                await _service.GetRulesAsync();

            return rules.Select(AutorestClientMapper.ToModel);
        }

        /// <summary>
        /// Returns rule details by specified id.
        /// </summary>
        /// <param name="ruleId">The rule id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task<RuleModel> GetRuleById(string ruleId)
        {
            object result = await _service.GetRuleByIdAsync(ruleId);

            if (result is AutorestClient.Models.RuleModel ruleModel)
                return ruleModel.ToModel();

            if (result is ErrorResponse errorResponse)
                throw new ErrorResponseException(errorResponse.ErrorMessage);

            throw new InvalidOperationException($"Unexpected response type: {result?.GetType()}");
        }

        /// <summary>
        /// Adds the rule.
        /// </summary>
        /// <param name="model">The model what describe a rule.</param>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task AddRuleAsync(RuleModel model)
        {
            ErrorResponse errorResponse = await _service.AddRuleAsync(new NewRuleModel
            {
                Name = model.Name,
                RegulationId = model.RegulationId,
                AllowedAssetGroups = model.AllowedAssetGroups,
                DeclinedAssetGroups = model.DeclinedAssetGroups
            });

            if(errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Updates the rule.
        /// </summary>
        /// <param name="model">The model what describe a rule.</param>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task UpdateRuleAsync(RuleModel model)
        {
            ErrorResponse errorResponse = await _service.UpdateRuleAsync(new AutorestClient.Models.RuleModel
            {
                Id = model.Id,
                Name = model.Name,
                RegulationId = model.RegulationId,
                AllowedAssetGroups = model.AllowedAssetGroups,
                DeclinedAssetGroups = model.DeclinedAssetGroups
            });

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Deletes the rule by specified id.
        /// </summary>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task DeleteRuleAsync(string ruleId)
        {
            ErrorResponse errorResponse = await _service.DeleteRuleAsync(ruleId);

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        public void Dispose()
        {
            if (_service == null)
                return;

            _service.Dispose();
            _service = null;
        }
    }
}
