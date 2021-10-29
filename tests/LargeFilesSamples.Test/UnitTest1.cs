using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LargeFilesSamples.Test
{
    public class UnitTest1 : IClassFixture<TestAppFactory>
    {
        private readonly HttpClient _httpClient;

        public UnitTest1(TestAppFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Test1()
        {
            await using var memoryStream = new MemoryStream();
            using var httpContent = new MultipartFormDataContent();

            TextWriter textWriter = new StreamWriter(memoryStream);
            textWriter.Write("blabla");
            textWriter.Flush();

            memoryStream.Position = 0;

            var fileContent1 = new ByteArrayContent(memoryStream.ToArray());

            httpContent.Add(fileContent1, "sample", "sample.txt");

            var response = await _httpClient.PostAsync("/FileUpload/UploadLargeFile", httpContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}