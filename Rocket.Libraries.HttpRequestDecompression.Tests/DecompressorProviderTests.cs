using System;
using Microsoft.AspNetCore.Http;
using Moq;
using Rocket.Libraries.HttpRequestDecompression.Logging;
using Rocket.Libraries.HttpRequestDecompression.StreamProviders;
using Xunit;

namespace Rocket.Libraries.HttpRequestDecompression.Tests
{
    public class DecompressorProviderTests
    {
        [Fact]
        public void GZipStreamProviderIsReturnedCorrectly()
        {
            var gZipStreamProvider = new Mock<IGZipStreamProvider>();
            var deflateStreamProvider = new Mock<IDeflateStreamProvider>();

            SpecificProviderIsCalled(gZipStreamProvider, deflateStreamProvider,CompressionTypeDeterminer.ContentEncodingGzip);
            gZipStreamProvider.Verify(m => m.GetStream(It.IsAny<HttpRequest>()), Times.Once());
            deflateStreamProvider.Verify(m => m.GetStream(It.IsAny<HttpRequest>()), Times.Never());
        }

        [Fact]
        public void DeflateStreamProviderIsReturnedCorrectly()
        {
            var gZipStreamProvider = new Mock<IGZipStreamProvider>();
            var deflateStreamProvider = new Mock<IDeflateStreamProvider>();

            SpecificProviderIsCalled(gZipStreamProvider, deflateStreamProvider,CompressionTypeDeterminer.ContentEncodingDeflate);
            gZipStreamProvider.Verify(m => m.GetStream(It.IsAny<HttpRequest>()), Times.Never());
            deflateStreamProvider.Verify(m => m.GetStream(It.IsAny<HttpRequest>()), Times.Once());
        }

        [Fact]
        public void UnknownCompressionMethodThrowsException()
        {
            var compressionDeterminer = new Mock<ICompressionTypeDeterminer>();
            var gZipStreamProvider = new Mock<IGZipStreamProvider>();
            var deflateStreamProvider = new Mock<IDeflateStreamProvider>();
            var logWriter = new Mock<ILogWriter>();

            compressionDeterminer.Setup(
                a => a.IsCompressedWithEncodingType(
                    It.IsAny<HttpRequest>(),
                    It.Is<string>(a => a.Equals("blah"))))
                    .Returns(true);

            var decompressorProvider = new DecompressorProvider(
                compressionDeterminer.Object,
                gZipStreamProvider.Object,
                deflateStreamProvider.Object,
                logWriter.Object
            );

            var request = new HttpRequestMockObjectsProvider ().HttpRequest;
            var headers = new Mock<IHeaderDictionary> ();
            

            headers.Setup (a => a[It.Is<string> (a => a.Equals (CompressionTypeDeterminer.ContentEncodingHeader, StringComparison.InvariantCultureIgnoreCase))])
                .Returns (CompressionTypeDeterminer.ContentEncodingGzip);

            request.Setup (a => a.Headers)
                .Returns (headers.Object);

            Assert.Throws<Exception>(() => decompressorProvider.GetDecompressor(request.Object));
        }


        private void SpecificProviderIsCalled(
            Mock<IGZipStreamProvider> gZipStreamProvider, 
            Mock<IDeflateStreamProvider> deflateStreamProvider,
            string compressionMethod)
        {
            var compressionDeterminer = new Mock<ICompressionTypeDeterminer>();
            var logWriter = new Mock<ILogWriter>();
            
            compressionDeterminer.Setup(
                a => a.IsCompressedWithEncodingType(
                    It.IsAny<HttpRequest>(),
                    It.Is<string>(a => a.Equals(compressionMethod))))
                    .Returns(true);

            var decompressorProvider = new DecompressorProvider(
                compressionDeterminer.Object,
                gZipStreamProvider.Object,
                deflateStreamProvider.Object,
                logWriter.Object
            );

            var request = new HttpRequestMockObjectsProvider ().HttpRequest;
            var headers = new Mock<IHeaderDictionary> ();
            

            headers.Setup (a => a[It.Is<string> (a => a.Equals (CompressionTypeDeterminer.ContentEncodingHeader, StringComparison.InvariantCultureIgnoreCase))])
                .Returns (CompressionTypeDeterminer.ContentEncodingGzip);

            request.Setup (a => a.Headers)
                .Returns (headers.Object);

            decompressorProvider.GetDecompressor(request.Object);
        }
    }
}