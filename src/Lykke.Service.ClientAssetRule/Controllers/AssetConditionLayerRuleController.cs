using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetConditionLayer;
using Lykke.Service.ClientAssetRule.Core.Services;
using Lykke.Service.ClientAssetRule.Core.Utils;
using Lykke.Service.ClientAssetRule.Models;
using Lykke.Service.ClientAssetRule.Models.AssetConditionLayerRule;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.ClientAssetRule.Controllers
{
    [Route("api/[controller]")]
    public class AssetConditionLayerRuleController : Controller
    {
        private readonly IAssetConditionLayerRuleService _assetConditionLayerRuleService;
        private readonly ILog _log;

        public AssetConditionLayerRuleController(
            IAssetConditionLayerRuleService assetConditionLayerRuleService,
            ILog log)
        {
            _assetConditionLayerRuleService = assetConditionLayerRuleService;
            _log = log;
        }

        /// <summary>
        /// Returns all asset condition layer rules.
        /// </summary>
        /// <returns>The list of asset condition layer rules.</returns>
        /// <response code="200">The list of asset condition layer rules.</response>
        [HttpGet]
        [SwaggerOperation("AssetConditionLayerRuleGet")]
        [ProducesResponseType(typeof(IEnumerable<AssetConditionLayerRuleModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<IAssetConditionLayerRule> rules = await _assetConditionLayerRuleService.GetAsync();

            var model = Mapper.Map<List<AssetConditionLayerRuleModel>>(rules);

            return Ok(model);
        }

        /// <summary>
        /// Adds the asset condition layer rule.
        /// </summary>
        /// <param name="model">The model that describe an asset condition layer rule.</param>
        /// <response code="204">Rule successfully added.</response>
        /// <response code="400">An error occurred during adding.</response>
        [HttpPost]
        [SwaggerOperation("AssetConditionLayerRuleAdd")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] AssetConditionLayerRuleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));

            var rule = Mapper.Map<AssetConditionLayerRule>(model);

            try
            {
                await _assetConditionLayerRuleService.AddAsync(rule);
            }
            catch (Exception exception)
            {
                await _log.WriteErrorAsync(nameof(AssetGroupRuleController), nameof(AddAsync),
                    model.ToJson(), exception);

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(AssetGroupRuleController), nameof(AddAsync),
                model.ToJson(), "Asset condition layer rule added.");

            return NoContent();
        }

        /// <summary>
        /// Updates the asset condition layer rule.
        /// </summary>
        /// <param name="model">The model that describe an asset condition layer rule.</param>
        /// <response code="204">Rule successfully updated.</response>
        /// <response code="400">An error occurred during updating.</response>
        [HttpPut]
        [SwaggerOperation("AssetConditionLayerRuleUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] AssetConditionLayerRuleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));

            var rule = Mapper.Map<AssetConditionLayerRule>(model);

            try
            {
                await _assetConditionLayerRuleService.UpdateAsync(rule);
            }
            catch (Exception exception)
            {
                await _log.WriteErrorAsync(nameof(AssetGroupRuleController), nameof(UpdateAsync),
                    model.ToJson(), exception);

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(AssetGroupRuleController), nameof(UpdateAsync),
                model.ToJson(), "Asset condition layer rule updated.");

            return NoContent();
        }

        /// <summary>
        /// Deletes the asset condition layer rule for regulation.
        /// </summary>
        /// <param name="regulationId">The regulation.</param>
        /// <response code="204">Rule successfully deleted.</response>
        /// <response code="400">An error occurred during deleting.</response>
        [HttpDelete]
        [Route("regulation/{regulationId}")]
        [SwaggerOperation("AssetConditionLayerRuleDelete")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync(string regulationId)
        {
            try
            {
                await _assetConditionLayerRuleService.DeleteAsync(regulationId);
            }
            catch (Exception exception)
            {
                await _log.WriteErrorAsync(nameof(AssetGroupRuleController), nameof(DeleteAsync),
                    regulationId.ToContext(nameof(regulationId)).ToJson(), exception);

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(AssetGroupRuleController), nameof(DeleteAsync),
                regulationId.ToContext(nameof(regulationId)).ToJson(),
                "Asset condition layer rule deleted.");

            return NoContent();
        }
    }
}
