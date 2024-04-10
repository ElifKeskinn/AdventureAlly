using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CleanArchitecture.WebApi.Controllers
{
    public class MetaController : BaseApiController
    {
        public object UserVersion { get; private set; }

        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Program).Assembly;

            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            FileVersionInfo version = FileVersionInfo.GetVersionInfo(assembly.Location); _ = UserVersion;

            return Ok($"Version: {version}, Last Updated: {lastUpdate}");
        }
    }
}
