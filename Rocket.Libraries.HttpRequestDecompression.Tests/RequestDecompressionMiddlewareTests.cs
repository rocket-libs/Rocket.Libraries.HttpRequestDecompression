using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using Rocket.Libraries.HttpRequestDecompression;
using Rocket.Libraries.HttpRequestDecompression.Tests;
using Xunit;

namespace Lattice.Services.Foundation.Tests.RequestDecompression
{
    public class RequestDecompressionMiddlewareTests
    {
        const int successHttpStatusCode = 201;

        [Theory]
        [InlineData (true)]
        [InlineData (false)]
        public void ExceptionOnlyThrownIfRequestDelegateIsNull (bool passNullRequestDelegate)
        {
            Action constructMiddleware = () =>
            {
                new RequestDecompressionMiddleware (
                    passNullRequestDelegate ? null : new RequestDelegate (RequestDel),
                    compressionDeterminer : default,
                    decompressorProvider : default
                );
            };
            if (passNullRequestDelegate)
            {
                Assert.Throws<ArgumentNullException> (
                    () => constructMiddleware ()
                );
            }
            else
            {
                constructMiddleware ();
            }

        }

        [Theory]
        [InlineData (true)]
        [InlineData (false)]
        public async Task DecompressionOnlyDoneForCompressedRequests (bool isCompressed)
        {
            var compressionDeterminer = new Mock<ICompressionTypeDeterminer> ();
            var decompressorProvider = new Mock<IDecompressorProvider> ();
            var httpRequestMockObjectsProvider = new HttpRequestMockObjectsProvider ();

            compressionDeterminer.Setup (a => a.IsCompressed (It.IsAny<HttpRequest> ()))
                .Returns (isCompressed);

            var requestDecompressionMiddleware = new RequestDecompressionMiddleware (
                RequestDel,
                compressionDeterminer.Object,
                decompressorProvider.Object
            );

            await requestDecompressionMiddleware.Invoke (httpRequestMockObjectsProvider.MockHttpContext.Object);
            var expectedCallTimes = isCompressed ? Times.Exactly (1) : Times.Never ();
            decompressorProvider.Verify (m => m.GetDecompressor (It.IsAny<HttpRequest> ()), expectedCallTimes);
        }

        private Task RequestDel (HttpContext context)
        {
            context.Response.StatusCode = successHttpStatusCode;
            return Task.CompletedTask;
        }

    }
}