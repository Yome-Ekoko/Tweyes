﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TweyesBackend.Controllers
{
    public class MetaController : ControllerBase
    {
        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {lastUpdate}");
        }

        [HttpGet("/health")]
        public ActionResult<string> Health()
        {
            return Ok("Ok");
        }
    }
}
