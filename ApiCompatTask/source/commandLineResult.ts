import { TaskResult, getInput } from "azure-pipelines-task-lib";

export default class CommandLineResult {
    private _totalIssues: number;
    private _body: string;
    private green: string = "\x1b[32m";
    private yellow: string = "\x1b[31m";
    private red: string = "\x1b[33m";
    private failOnIssue: string = getInput('failOnIssue');

    get totalIssues() {
        return this._totalIssues;
    }

    get body() {
        return this._body;
    }

    constructor(result: string) {
        const indexOfTotalIssues = result.indexOf("Total Issues");
        
        this._totalIssues = this.getTotalIssues(result, indexOfTotalIssues);
        this._body = this.getBody(result, indexOfTotalIssues);
    }

    private getTotalIssues = (message: string, indexOfTotalIssues: number): number => {
        return parseInt(message.substring(indexOfTotalIssues).split(':')[1].trim(), 10);
    }
    
    private getBody = (message: string, indexOfTotalIssues: number): string => {
        return message.substring(0, indexOfTotalIssues - 1);
    }

    public resultText() {
        return this.totalIssues ?
        `There were differences between the assemblies` :
        `No differences were found between the assemblies`;
    }

    public compatibilityResult = (): TaskResult => {
        return this._totalIssues  === 0
            ? TaskResult.Succeeded
            : this.failOnIssue
                ? TaskResult.SucceededWithIssues
                : TaskResult.Failed
    }

    public colorCode = (): string => {
        return this._totalIssues === 0
            ? this.green
            : this.failOnIssue
                ? this.red
                : this.yellow;
    }
}
