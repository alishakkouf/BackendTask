using System.Runtime.InteropServices;
using BackendTask.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BackendTask.Common
{
    /// <summary>
    /// Base api controller annotated with attribute [ApiController] and default
    /// base route("api/[controller]").
    /// Needs the service IStringLocalizerFactory to be injected for localization.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private readonly IStringLocalizer _localizer;

        protected BaseApiController(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(CommonResource)); // Ensure it creates from DatabaseStringLocalizer
        }

        /// <summary>
        /// Localize a message using database localization.
        /// </summary>
        protected string Localize(string message)
        {
            return _localizer[message];
        }

        /// <summary>
        /// Localize a message using database localization with arguments.
        /// </summary>
        protected string Localize(string message, params object[] arguments)
        {
            return _localizer[message, arguments];
        }
    }
}
