import { join } from 'path'
import { getInput, setResult, TaskResult } from 'azure-pipelines-task-lib';
import { execSync } from "child_process";
import { existsSync, writeFileSync, mkdirSync } from 'fs';
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
    const totalIssues = commandLineResult.totalIssues;
    const resultText = commandLineResult.resultText();
    
    writeResult(commandLineResult.body, commandLineResult.totalIssues);
    console.log(commandLineResult.body +
        commandLineResult.colorCode() +
        'Total Issues : ' + totalIssues);
    setResult(commandLineResult.compatibilityResult(), resultText);
}

const writeResult = (body: string, issues: number): void => {
    const fileName: string = getInput('outputFilename');
    const directory: string = getInput("outputFolder");
    const result: any = {
        issues: issues,
        body: issues === 0 ? `No issues found in ${ getInput('contractsFileName') }` : body
    }
    
    
    if (!existsSync(directory)) {
        mkdirSync(directory, { recursive: true });
    }
    
    writeFileSync(`${join(directory, fileName)}`, JSON.stringify(result, null, 2) );
}

run();
