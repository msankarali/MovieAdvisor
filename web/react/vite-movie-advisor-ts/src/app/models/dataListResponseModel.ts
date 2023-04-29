import ResponseModel from "./responseModel";

export default interface DataListResponseModel<T> extends ResponseModel {
    data: T[];
}