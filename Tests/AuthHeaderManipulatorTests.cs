﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Archon.WebApi.Tests
{
	public class AuthHeaderManipulatorTests
	{
		readonly HttpClient client;
		readonly FakeHttpHandler handler;

		public AuthHeaderManipulatorTests()
		{
			client = new HttpClient(new AuthHeaderManipulator
			{
				InnerHandler = handler = new FakeHttpHandler()
			});
		}

		[Fact]
		public void can_parse_auth_parameter_to_bearer_token_header()
		{
			bool ran = false;

			handler.Action = (req, c) =>
			{
				Assert.NotNull(req.Headers.Authorization);
				Assert.Equal("Bearer", req.Headers.Authorization.Scheme);
				Assert.Equal("my-token", req.Headers.Authorization.Parameter);
				ran = true;

				return Task.FromResult(req.CreateResponse(HttpStatusCode.OK));
			};

			var result = client.GetAsync("http://example.com/?auth=my-token").Result;
			Assert.True(ran, "Did not run fake test handler");
		}

		[Fact]
		public void can_not_parse_other_parameters_as_bearer_token_header()
		{
			bool ran = false;

			handler.Action = (req, c) =>
			{
				Assert.Null(req.Headers.Authorization);
				ran = true;
				return Task.FromResult(req.CreateResponse(HttpStatusCode.OK));
			};

			var result = client.GetAsync("http://example.com/?auths=my-token").Result;
			Assert.True(ran, "Did not run fake test handler");
		}
	}
}