using System.Threading.Tasks;
using System;
using Octokit;
using System.Collections.Generic;
using System.Linq;

namespace SDDBackend.Controllers {
    public class GitController {

        public static async Task<IReadOnlyList<RepositoryContent>> getFile(string path, string repo)
        {
            var access_token = System.Environment.GetEnvironmentVariable("SCD_Access");
            var tokenAuth = new Credentials(access_token);
            var client = new GitHubClient(new ProductHeaderValue("marshmallouws"));
            client.Credentials = tokenAuth;

            return await client.Repository.Content.GetAllContents("marshmallouws", repo, path);
        }

        public static async Task createFile(string pushMessage, string fileContent, string path, string repo)
        {
            var access_token = System.Environment.GetEnvironmentVariable("SCD_Access");
            var tokenAuth = new Credentials(access_token);
            var client = new GitHubClient(new ProductHeaderValue("marshmallouws"));
            client.Credentials = tokenAuth;

            var createFileRequest = new CreateFileRequest(pushMessage, fileContent);
            var repositoryResponse = await client.Repository.Get("marshmallouws", repo);

            await client.Repository.Content.CreateFile(repositoryResponse.Id, path, createFileRequest);
        }

        public static async Task<bool> removeFile(string path, string repo = "scdfiles")
        {
            try
            {
                var access_token = System.Environment.GetEnvironmentVariable("SCD_Access");
                var tokenAuth = new Credentials(access_token);
                var client = new GitHubClient(new ProductHeaderValue("marshmallouws"));
                client.Credentials = tokenAuth;

                // get sha for deletefile request
                var existingFile = await client.Repository.Content.GetAllContents("marshmallouws", repo, path);
                string sha = existingFile.ElementAt<RepositoryContent>(0).Sha;

                // create deletefilerequest
                var deleteFileRequest = new DeleteFileRequest("Remove testing installation", sha);
                // get repo
                var repositoryResponse = await client.Repository.Get("marshmallouws", repo);

                await client.Repository.Content.DeleteFile(repositoryResponse.Id, path, deleteFileRequest);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}