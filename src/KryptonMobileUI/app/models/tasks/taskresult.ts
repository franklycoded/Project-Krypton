export class TaskResult {
    constructor (public jobItemId: number, public taskResult: string, public isSuccessful: boolean, public errorMessage: string){

    }
}