import taskLibrary = require('azure-pipelines-task-lib/task');
import gitClient = require('@octokit/rest');
import {join, extname} from 'path';
import fs = require('fs');
import {EOL} from 'os';

const extension = ".json";

const clientWithAuth = new gitClient({
    auth: `token ${taskLibrary.getInput('userToken')}`,
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
            body: "\`\`\`" + EOL + message + EOL + "\`\`\`"
        };
        
        await clientWithAuth.issues.createComment(comment).then(res => {
            console.log(res);
        })
        .catch(err => {
            switch (err.status){
                case 400:
                    taskLibrary.error(`The PR ${taskLibrary.getInput('prNumber')} was not found`);
                    break;
                case 401:
                    taskLibrary.error(`The credentials are invalid`);
                    break;
                case 404:
                    taskLibrary.error(`The PR ${taskLibrary.getInput('prNumber')} on repository ${taskLibrary.getInput('repository')} was not found`);
                    break;
                default:
                    taskLibrary.error(`Unhandled error`);
                    break;
            }
            console.log(err);
        });

    }
}

const getFilesFromDir = (filePath: string, extName: string, recursive: boolean): string[] => {
    
    if (!fs.existsSync(filePath)){
        console.log(`File path does not exist: ${filePath}`);
        return new Array();
    }
    var result: string[] = new Array();
    iterateFilesFromDir(filePath, extName, recursive, result);

    return result;
}

const iterateFilesFromDir = (filePath: string, extName: string, recursive: boolean, result: string[]): string[] => {
    var files = fs.readdirSync(filePath);
    files.forEach(file => {
        var fileName = join(filePath,file);
        var isFolder = fs.lstatSync(fileName);
        if (recursive && isFolder.isDirectory()){
            result = iterateFilesFromDir(fileName, extName, recursive, result);
        }

        if (extname(fileName) == extName){
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
        body += fileObject["body"].toString() + EOL;
    });
    return body;
}

const logError = (message: string): void => {
    taskLibrary.setResult(taskLibrary.TaskResult.Failed, message);
}

const validateInput = (files: string[]): boolean => {
    if(!(files && files.length)) {
        taskLibrary.error(`No files where found on ${taskLibrary.getInput('bodyFilePath')} with the extension ${extension}`);
        return false;
    }
    if(taskLibrary.getInput('repository') == "" || taskLibrary.getInput('repository').indexOf("/") == -1){
        taskLibrary.error(`The repository \"${taskLibrary.getInput('repository')}\" is invalid`);
        return false;
    }
    if(parseInt(taskLibrary.getInput('prNumber')) === null || !parseInt(taskLibrary.getInput('prNumber')) || parseInt(taskLibrary.getInput('prNumber')) < 0){
        taskLibrary.error(`The PR number \"${taskLibrary.getInput('prNumber')}\" is invalid`);
        return false;
    }
    return true;
}

run();