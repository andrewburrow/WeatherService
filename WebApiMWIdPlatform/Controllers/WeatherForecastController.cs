using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiMWIdPlatform.Authentication;

namespace WebApiMWIdPlatform.Controllers {
	[ApiController]
	[Route("[controller]/[Action]")]
	[AuthorizeRoles(AuthorizationRoles.ContributorRole, AuthorizationRoles.ReaderRole)]
	public class WeatherForecastController : ControllerBase {
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger) {
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<WeatherForecast> OpenGet() {
			var rng = new Random();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpGet]
		[AllowAnonymous]
		public IEnumerable<WeatherForecast> OpenAny() {
			var rng = new Random();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpGet]
		[AuthorizeRoles(AuthorizationRoles.ContributorRole, AuthorizationRoles.ReaderRole)]
		public IEnumerable<WeatherForecast> OpenRead() {
			var rng = new Random();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpGet]
		[AuthorizeRoles(AuthorizationRoles.ContributorRole)]
		public IEnumerable<WeatherForecast> OpenContribute() {
			var rng = new Random();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			})
			.ToArray();
		}
	}
}
