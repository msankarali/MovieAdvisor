import { ResultType } from "./resultType";

export default interface ResponseModel {
    messages: string[];
    code: number;
    resultTye: ResultType;
}
