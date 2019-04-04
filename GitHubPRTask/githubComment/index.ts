import taskLibrary = require('azure-pipelines-task-lib/task');
import gitClient = require('@octokit/rest');
import path = require('path');
import fs = require('fs');

const extension = ".json";

const clientWithAuth = new gitClient({
    auth: "token "+ taskLibrary.getInput('userToken'),
    userAgent: 'octokit/rest.js v1.2.3',
});

async function run() {
    var files = getFilesFromDir(taskLibrary.getInput('bodyFilePath'), extension, taskLibrary.getBoolInput('getSubFolders'))
    if(validateInput(files)){ 
        var message = combineMessageBody(files);
        var repo = taskLibrary.getInput('repository').split('/');

        const comment: gitClient.IssuesCreateCommentParams = {
            owner: repo[0],
            repo: repo[1],
            number: parseInt(taskLibrary.getInput('prNumber')),
            body: "\`\`\`\r\n" + message + "\r\n\`\`\`"
        };
        
        await clientWithAuth.issues.createComment(comment).then(res => {
            console.log(res);
        })
        .catch(err => {
            console.log(err);
        });

    }
}

const getFilesFromDir = (filePath: string, extName: string, recursive: boolean): string[] => {
    
    if (!fs.existsSync(filePath)){
        console.log("File path does not exist: ",filePath);
        return new Array();
    }
    var result: string[] = new Array();
    iterateFilesFromDir(filePath, extName, recursive, result);

    return result;
}

const iterateFilesFromDir = (filePath: string, extName: string, recursive: boolean, result: string[]): string[] => {
    var files = fs.readdirSync(filePath);
    files.forEach(file => {
        var fileName = path.join(filePath,file);
        var isFolder = fs.lstatSync(fileName);
        if (recursive && isFolder.isDirectory()){
            result = iterateFilesFromDir(fileName, extName, recursive, result);
        }

        if (path.extname(fileName) == extName){
            result.push(fileName);
        } 
    });
    
    return result;
}

const combineMessageBody = (files: string[]): string => {
    var body:string = "";
    files.forEach(file => {
        var bodyFile = fs.readFileSync(file);
        var fileObject = JSON.parse(bodyFile.toString());
        body += fileObject["body"].toString() + "\r\n";
    });
    return body;
}

const logError = (message: string): void => {
    taskLibrary.setResult(taskLibrary.TaskResult.Failed, message);
}

const validateInput = (files: string[]): boolean => {
    if(!(files && files.length)) {
        console.log("no files where found on " + taskLibrary.getInput('bodyFilePath') + " with the " + extension + " extension");
        return false;
    }
    if(taskLibrary.getInput('repository') == "" || taskLibrary.getInput('repository').indexOf("/") == -1){
        console.log("The repository \"" + taskLibrary.getInput('repository') + "\" is invalid");
        return false;
    }
    if(parseInt(taskLibrary.getInput('prNumber')) === null || !parseInt(taskLibrary.getInput('prNumber')) || parseInt(taskLibrary.getInput('prNumber')) < 0){
        console.log("the PR number \"" + taskLibrary.getInput('prNumber') + "\" is invalid");
        return false;
    }
    return true;
}

run();