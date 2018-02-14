using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using Lykke.Common.Extensions;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetGroup;
using Lykke.Service.ClientAssetRule.Core.Services;
using Lykke.Service.ClientAssetRule.Models;
using Lykke.Service.ClientAssetRule.Models.AssetGroupRule;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.ClientAssetRule.Controllers
{
    [Route("api/[controller]")]
    public class AssetGroupRuleController : Controller
    {
        private readonly IAssetGroupRuleService _ruleService;
        private readonly ILog _log;

        public AssetGroupRuleController(IAssetGroupRuleService ruleService, ILog log)
        {
            _ruleService = ruleService;
            _log = log;
        }

        /// <summary>
        /// Returns all asset group rules.
        /// </summary>
        /// <returns>The list of asset group rules.</returns>
        /// <response code="200">The list of asset group rules.</response>
        [HttpGet]
        [SwaggerOperation("AssetGroupRuleGet")]
        [ProducesResponseType(typeof(IEnumerable<AssetGroupRuleModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<IAssetGroupRule> rules = await _ruleService.GetAllAsync();

            var model = Mapper.Map<IEnumerable<IAssetGroupRule>, IEnumerable<AssetGroupRuleModel>>(rules);

            return Ok(model);
        }

        /// <summary>
        /// Returns an asset group rule by specified id.
        /// </summary>
        /// <param name="ruleId">The asset group rule id.</param>
        /// <returns>The asset group rule if exists.</returns>
        /// <response code="200">The asset group rule.</response>
        /// <response code="400">Rule not found.</response>
        [HttpGet]
        [Route("{ruleId}")]
        [SwaggerOperation("AssetGroupRuleGetById")]
        [ProducesResponseType(typeof(AssetGroupRuleModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(string ruleId)
        {
            IAssetGroupRule rule = await _ruleService.GetByIdAsync(ruleId);

            if (rule == null)
            {
                await _log.WriteWarningAsync(nameof(AssetGroupRuleController), nameof(Get),
                    $"Asset group rule not found. {nameof(ruleId)}: {ruleId}. IP: {HttpContext.GetIp()}");
                return BadRequest(ErrorResponse.Create("Asset group rule not found"));
            }

            var model = Mapper.Map<AssetGroupRuleModel>(rule);

            return Ok(model);
        }

        /// <summary>
        /// Adds the asset group rule.
        /// </summary>
        /// <param name="model">The model that describe an asset group rule.</param>
        /// <response code="204">Rule successfully added.</response>
        /// <response code="400">An error occurred during adding.</response>
        [HttpPost]
        [SwaggerOperation("AssetGroupRuleAdd")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] NewAssetGroupRuleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));

            var rule = Mapper.Map<AssetGroupRule>(model);

            try
            {
                await _ruleService.InsertAsync(rule);
            }
            catch (Exception exception)
            {
                await _log.WriteWarningAsync(nameof(AssetGroupRuleController), nameof(Add),
                    $"{exception.Message}. {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(AssetGroupRuleController), nameof(Add),
                $"Asset group rule added. Model: {model.ToJson()}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }

        /// <summary>
        /// Updates the asset group rule.
        /// </summary>
        /// <param name="model">The model that describe an asset group rule.</param>
        /// <response code="204">Rule successfully updated.</response>
        /// <response code="400">An error occurred during updating.</response>
        [HttpPut]
        [SwaggerOperation("AssetGroupRuleUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] AssetGroupRuleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));

            var rule = Mapper.Map<AssetGroupRule>(model);

            try
            {
                await _ruleService.UpdateAsync(rule);
            }
            catch (Exception exception)
            {
                await _log.WriteWarningAsync(nameof(AssetGroupRuleController), nameof(Update),
                    $"{exception.Message}. {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(AssetGroupRuleController), nameof(Update),
                $"Asset group rule updated. {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }

        /// <summary>
        /// Deletes the asset group rule.
        /// </summary>
        /// <param name="ruleId">The asset group rule id.</param>
        /// <response code="204">Rule successfully deleted.</response>
        /// <response code="400">An error occurred during deleting.</response>
        [HttpDelete]
        [Route("{ruleId}")]
        [SwaggerOperation("AssetGroupRuleDelete")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(string ruleId)
        {
            try
            {
                await _ruleService.DeleteAsync(ruleId);
            }
            catch (Exception exception)
            {
                await _log.WriteWarningAsync(nameof(AssetGroupRuleController), nameof(Delete),
                    $"{exception.Message}. {nameof(ruleId)}: {ruleId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(AssetGroupRuleController), nameof(Delete),
                $"Asset group rule deleted. {nameof(ruleId)}: {ruleId}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }
    }
}
