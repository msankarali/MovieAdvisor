import ResponseModel from "./responseModel";

export default interface DataResponseModel<T> extends ResponseModel {
    data: T;
}