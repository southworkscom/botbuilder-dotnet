import taskLibrary = require('azure-pipelines-task-lib/task');
import gitClient = require('@octokit/rest');
import { readFileSync } from 'fs';

const clientWithAuth = new gitClient({
    auth: "token "+ taskLibrary.getInput('userToken'), 
    userAgent: 'octokit/rest.js v1.2.3',
});

async function run() {

    var bodyFilePath = readFileSync(taskLibrary.getInput('bodyFilePath'));
    var fileObject = JSON.parse(bodyFilePath.toString());

    const repoConfig = {
        owner: "southworkscom",
        repo: "botbuilder-dotnet"
    }

    const commentInfo = {
        commentText:  fileObject["body"],
        pullRequestNumber: parseInt(taskLibrary.getInput('prNumber'))
    }

    const comment: gitClient.IssuesCreateCommentParams = {
        owner: repoConfig.owner,
        repo: repoConfig.repo,
        number: commentInfo.pullRequestNumber,
        body: commentInfo.commentText
    };
    
    await clientWithAuth.issues.createComment(comment).then(res => {
        console.log(res);
    })
    .catch(err => {
        console.log(err);
    });
}

run();