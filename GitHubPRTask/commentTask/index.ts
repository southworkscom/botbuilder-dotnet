import taskLibrary = require('azure-pipelines-task-lib/task');
import gitClient = require('@octokit/rest')

const clientWithAuth = new gitClient({
    auth: "token" + taskLibrary.getInput('userToken'), 
    userAgent: 'octokit/rest.js v1.2.3',
});

async function run() {

    const repoConfig = {
        owner: "southworkscom",
        repo: "botbuilder-dotnet"
    }
    const commentInfo = {
        commentText:  "A Comment created from the API",
        pullRequestNumber: 41
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