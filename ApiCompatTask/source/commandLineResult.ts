import { TaskResult, getInput, getBoolInput } from "azure-pipelines-task-lib";

export default class CommandLineResult {
    private _totalIssues: number;
    private _body: string;
    private red: string = "\x1b[31m";
    private green: string = "\x1b[32m";
    private yellow: string = "\x1b[33m";
    private failOnIssue: boolean = getBoolInput('failOnIssue');

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
        if (this._totalIssues  === 0) {
            return TaskResult.Succeeded;
        } else if (this.failOnIssue) {
            return TaskResult.Failed;
        } else {
            return TaskResult.SucceededWithIssues;
        }
    }

    public colorCode = (): string => {
        if (this._totalIssues === 0) {
            return this.green;
        } else if (this.failOnIssue) {
            return this.red;
        } else {
            return this.yellow;
        }
    }
}
