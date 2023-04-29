import { ResultType } from "./ResultType";

export default interface ResponseModel {
    messages: string[];
    code: number;
    resultTye: ResultType;
}
