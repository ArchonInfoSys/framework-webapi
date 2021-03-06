﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Archon.Http
{
	public static class HttpClientExtensions
	{
		public static async Task<HttpResponseMessage> SendAsync(this HttpClient client, Link link)
		{
			if (link == null)
				throw new ArgumentNullException("link");

			var req = link.CreateRequest();
			return await client.SendAsync(req).ConfigureAwait(false);
		}

		public static HttpResponseMessage Send(this HttpClient client, Link link)
		{
			return client.SendAsync(link).Result;
		}

		public static async Task<TResponse> SendAsync<TResponse>(this HttpClient client, Link<TResponse> link)
		{
			var response = await client.SendAsync((Link)link).ConfigureAwait(false);
			return await link.ParseResponseAsync(response).ConfigureAwait(false);
		}

		public static TResponse Send<TResponse>(this HttpClient client, Link<TResponse> link)
		{
			return client.SendAsync(link).Result;
		}

		public static HttpResponseMessage Send(this HttpClient client, HttpRequestMessage request)
		{
			var task = client.SendAsync(request);
			task.ConfigureAwait(false);
			return task.Result;
		}
	}
}