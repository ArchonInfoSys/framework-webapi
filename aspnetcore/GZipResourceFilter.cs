﻿using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace Archon.AspNetCore
{
	/// <summary>
	/// A filter that decompresses GZip content from HTTP requests specified as <c>Content-Encoding: gzip</c>
	/// </summary>
	public class GZipResourceFilter : IAsyncResourceFilter
	{
		public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
		{
			if (context.HttpContext.Request.Headers.TryGetValue("Content-Encoding", out StringValues values)
			    && values.Count == 1 
			    && (values[0] == "gzip" || values[0] == "x-gzip"))
				context.HttpContext.Request.Body = new GZipStream(context.HttpContext.Request.Body, CompressionMode.Decompress, false);

			await next();
		}
	}
}
