using System.Threading.Tasks;
using System;
using Octokit;

namespace SDDBackend.Controllers {
    public class GitController {
        public async void createFile() {
            var access_token = System.Environment.GetEnvironmentVariable("SCD_Access");
            var tokenAuth = new Credentials(access_token);
            var client = new GitHubClient(new ProductHeaderValue("marshmallouws"));
            client.Credentials = tokenAuth;


            string content = "Hello";
            var createFileRequest = new CreateFileRequest("message", content);


            var repositoryResponse = await client.Repository.Get("marshmallouws", "scdfiles");
            Console.Write(repositoryResponse.Id);

            var fileCreateResponse = client.Repository.Content.CreateFile(repositoryResponse.Id, "./a/heldig", createFileRequest);
        }
    }
}