using System.Threading.Tasks;
using System;
using Octokit;
using System.Collections.Generic;
using System.Linq;

namespace SDDBackend.Controllers {
    public class GitController {
        public static async Task createFile(string pushMessage, string fileContent, string path) {

            var access_token = System.Environment.GetEnvironmentVariable("SCD_Access");
            var tokenAuth = new Credentials(access_token);
            var client = new GitHubClient(new ProductHeaderValue("marshmallouws"));
            client.Credentials = tokenAuth;


            var createFileRequest = new CreateFileRequest(pushMessage, fileContent);
            var repositoryResponse = await client.Repository.Get("marshmallouws", "scdfiles");

            await client.Repository.Content.CreateFile(repositoryResponse.Id, path, createFileRequest);
        }

        public static async Task<IReadOnlyList<RepositoryContent>> getFile(string path)
        {

            var access_token = System.Environment.GetEnvironmentVariable("SCD_Access");
            var tokenAuth = new Credentials(access_token);
            var client = new GitHubClient(new ProductHeaderValue("marshmallouws"));
            client.Credentials = tokenAuth;

            return await client.Repository.Content.GetAllContents("marshmallouws", "scdfiles", path);
        }
    }
}