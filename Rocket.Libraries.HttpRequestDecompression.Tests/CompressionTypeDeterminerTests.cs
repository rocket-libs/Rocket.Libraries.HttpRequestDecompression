using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Rocket.Libraries.HttpRequestDecompression.Tests
{
    public class CompressionTypeDeterminerTests
    {
        [Theory]
        [InlineData ("gzip")]
        [InlineData ("GZIP")]
        [InlineData ("gZIP")]
        [InlineData ("GZiP")]
        [InlineData ("GZIp")]
        public void IsGZippedWorksCorrectly (string sampleSpelling)
        {
            Assert.True(TestSpecificCompressionMethod (sampleSpelling, new CompressionTypeDeterminer ().IsGZipped));

        }

        [Fact]
        public void UnknownSupportedMethodReportedAsUncompressed()
        {
            Assert.False(TestSpecificCompressionMethod("blah", new CompressionTypeDeterminer().IsCompressed));
        }

        [Fact]
        public void MissingEncodingHeaderReportedAsUncompressed()
        {
            var request = new HttpRequestMockObjectsProvider ().HttpRequest;
            var headers = new Mock<IHeaderDictionary> ();
            var keys = new Mock<ICollection<string>> ();

            keys.Setup (a => a.Contains (It.Is<string> (a => a.Equals (CompressionTypeDeterminer.ContentEncodingHeader, StringComparison.InvariantCultureIgnoreCase))))
                .Returns (false);

            headers.Setup (a => a.Keys)
                .Returns (keys.Object);

           request.Setup (a => a.Headers)
                .Returns (headers.Object);

            Assert.False(new CompressionTypeDeterminer().IsCompressed(request.Object));
        }

        [Theory]
        [InlineData ("deflate")]
        [InlineData ("Deflate")]
        [InlineData ("DEflate")]
        [InlineData ("DEFlate")]
        [InlineData ("DEFlatE")]
        public void IsDeflatedWorksCorrectly (string sampleSpelling)
        {
            Assert.True(TestSpecificCompressionMethod (sampleSpelling, new CompressionTypeDeterminer ().IsDeflated));
        }

        [Theory]
        [InlineData (CompressionTypeDeterminer.ContentEncodingDeflate)]
        [InlineData (CompressionTypeDeterminer.ContentEncodingGzip)]
        public void IsCompressedEvaluatesCorrectly (string compressionMethod)
        {
            Assert.True(TestSpecificCompressionMethod(compressionMethod, new CompressionTypeDeterminer().IsCompressed));
        }

        private bool TestSpecificCompressionMethod (string sampleSpelling, Func<HttpRequest, bool> determinerFn)
        {
            var request = new HttpRequestMockObjectsProvider ().HttpRequest;
            var headers = new Mock<IHeaderDictionary> ();
            var keys = new Mock<ICollection<string>> ();

            keys.Setup (a => a.Contains (It.Is<string> (a => a.Equals (CompressionTypeDeterminer.ContentEncodingHeader, StringComparison.InvariantCultureIgnoreCase))))
                .Returns (true);

            headers.Setup (a => a.Keys)
                .Returns (keys.Object);

            headers.Setup (a => a[It.Is<string> (a => a.Equals (CompressionTypeDeterminer.ContentEncodingHeader, StringComparison.InvariantCultureIgnoreCase))])
                .Returns (sampleSpelling);

            request.Setup (a => a.Headers)
                .Returns (headers.Object);

           return determinerFn (request.Object);
        }
    }
}