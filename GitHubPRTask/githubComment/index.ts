import taskLibrary = require('azure-pipelines-task-lib/task');
import gitClient = require('@octokit/rest');
import path = require('path');
import fs = require('fs');

const clientWithAuth = new gitClient({
    auth: "token "+ taskLibrary.getInput('userToken'),
    userAgent: 'octokit/rest.js v1.2.3',
});



async function run() {
    var files = getFilesFromDir(taskLibrary.getInput('bodyFilePath'), ".json", taskLibrary.getBoolInput('getSubFolders'))
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

const getFilesFromDir = (filePath: string, extName: string, recursive: boolean): string[] => {
    
    if (!fs.existsSync(filePath)){
        console.log("File path does not exist: ",filePath);
        return [];
    }
    var result: string[] = [];
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

run();