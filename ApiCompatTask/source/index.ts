import { join, parse } from 'path'
import { getInput, setResult, TaskResult } from 'azure-pipelines-task-lib';
import { execSync } from "child_process";
import { existsSync } from 'fs';
import CommandLineResult from './commandLineResult';
import ApiCompatCommand from './apiCompatCommand';

const run = (): void => {
    // Create ApiCompat path
    const ApiCompatPath = join(__dirname, 'ApiCompat', 'Microsoft.DotNet.ApiCompat.exe');
    
    // Get the binaries to compare and create the command to run
    const inputFiles: string = getInputFiles();
    const apiCompatCommands: ApiCompatCommand = new ApiCompatCommand(ApiCompatPath, inputFiles);

    // Show the ApiCompat version
    console.log(execSync(apiCompatCommands.version).toString());

    // Run the ApiCompat command
    runCommand(apiCompatCommands.command);
}

const getInputFiles = (): string => {
    const filesName: string[] = [];

    getInput('contractsFileName').split(' ').forEach(file => {
        const fullFilePath: string = join(getInput('contractsRootFolder'), file);
        if (existsSync(fullFilePath)) {
            filesName.push(fullFilePath);
        }
    });

    return filesName.join(',');
}

const runCommand = (command: string): void => {
    console.log(command);

    const result = execSync(command).toString();
    const commandLineResult = new CommandLineResult(result);
    const totalIssues =  commandLineResult.totalIssues;
    const resultText = commandLineResult.resultText();
    
    console.log(commandLineResult.body +
        commandLineResult.colorCode() +
        'Total Issues : ' + totalIssues);
    setResult(commandLineResult.compattibilityResult(), resultText);
}


run();
