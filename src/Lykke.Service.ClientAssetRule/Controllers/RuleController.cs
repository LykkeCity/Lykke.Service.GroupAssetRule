using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using Lykke.Service.ClientAssetRule.Core.Domain;
using Lykke.Service.ClientAssetRule.Core.Services;
using Lykke.Service.ClientAssetRule.Extensions;
using Lykke.Service.ClientAssetRule.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.ClientAssetRule.Controllers
{
    [Route("api/[controller]")]
    public class RuleController : Controller
    {
        private readonly IRuleService _ruleService;
        private readonly ILog _log;

        public RuleController(IRuleService ruleService, ILog log)
        {
            _ruleService = ruleService;
            _log = log;
        }

        /// <summary>
        /// Returns all client asset ruls.
        /// </summary>
        /// <returns>The list of rules.</returns>
        /// <response code="200">The list of rules.</response>
        [HttpGet]
        [SwaggerOperation("GetRules")]
        [ProducesResponseType(typeof(IEnumerable<RuleModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<IRule> rules = await _ruleService.GetAllAsync();

            var model = Mapper.Map<IEnumerable<IRule>, IEnumerable<RuleModel>>(rules);

            return Ok(model);
        }

        /// <summary>
        /// Returns a client asset rule by specified id.
        /// </summary>
        /// <param name="ruleId">The rule id.</param>
        /// <returns>The client asset rule if exists.</returns>
        /// <response code="200">The client asset rule.</response>
        /// <response code="400">Rule not found.</response>
        [HttpGet]
        [Route("{ruleId}")]
        [SwaggerOperation("GetRuleById")]
        [ProducesResponseType(typeof(RuleModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(string ruleId)
        {
            IRule rule = await _ruleService.GetByIdAsync(ruleId);

            if (rule == null)
            {
                await _log.WriteWarningAsync(nameof(RuleController), nameof(Get),
                    $"Rule not found. {nameof(ruleId)}: {ruleId}. IP: {HttpContext.GetIp()}");
                return BadRequest(ErrorResponse.Create("Rule not found"));
            }

            var model = Mapper.Map<RuleModel>(rule);

            return Ok(model);
        }

        /// <summary>
        /// Adds the rule.
        /// </summary>
        /// <param name="model">The model what describe a rule.</param>
        /// <response code="204">Rule successfully added.</response>
        /// <response code="400">An error occurred during adding.</response>
        [HttpPost]
        [SwaggerOperation("AddRule")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] NewRuleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));

            var rule = Mapper.Map<Rule>(model);

            try
            {
                await _ruleService.InsertAsync(rule);
            }
            catch (Exception exception)
            {
                await _log.WriteWarningAsync(nameof(RuleController), nameof(Add),
                    $"{exception.Message}. {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(RuleController), nameof(Add),
                $"Rule added. Model: {model.ToJson()}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }

        /// <summary>
        /// Updates the rule.
        /// </summary>
        /// <param name="model">The model what describe a rule.</param>
        /// <response code="204">Rule successfully updated.</response>
        /// <response code="400">An error occurred during updating.</response>
        [HttpPut]
        [SwaggerOperation("UpdateRule")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] RuleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));

            var rule = Mapper.Map<Rule>(model);

            try
            {
                await _ruleService.UpdateAsync(rule);
            }
            catch (Exception exception)
            {
                await _log.WriteWarningAsync(nameof(RuleController), nameof(Update),
                    $"{exception.Message}. {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(RuleController), nameof(Update),
                $"Rule updated. {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }

        /// <summary>
        /// Deletes the rule by specified id.
        /// </summary>
        /// <param name="ruleId">The id of rule to delete.</param>
        /// <response code="204">Rule successfully deleted.</response>
        /// <response code="400">An error occurred during deleting.</response>
        [HttpDelete]
        [Route("{ruleId}")]
        [SwaggerOperation("DeleteRule")]
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
                await _log.WriteWarningAsync(nameof(RuleController), nameof(Delete),
                    $"{exception.Message}. {nameof(ruleId)}: {ruleId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(RuleController), nameof(Delete),
                $"Rule deleted. {nameof(ruleId)}: {ruleId}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }
    }
}
