import axios, { AxiosError, AxiosResponse } from "axios";
import { store } from "../stores/store";
import LoginRequest from "../models/users/loginRequest";
import RegisterRequest from "../models/users/registerRequest";

axios.defaults.baseURL = import.meta.env.VITE_API_URL;

axios.interceptors.request.use(config => {
    const token = store.commonStore.token;
    if (token && config.headers) config.headers.Authorization = `Bearer ${token}`;
    return config;
});

axios.interceptors.response.use(async response => {
    return response;
}, (error: AxiosError) => {
    const { data, status } = error.response as AxiosResponse;

    console.log(status, data);

    switch (status) {
        case 400:
            break;
        case 401:
            break;
        case 403:
            break;
        case 404:
            break;
        case 423:
            break;
        case 500:
            break;
        default:
            break;
    }
    return Promise.reject(error);
});

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(res => res.data),
    post: <T>(url: string, body: object) => axios.post<T>(url, body).then(res => res.data),
    put: <T>(url: string, body: object) => axios.put<T>(url, body).then(res => res.data),
    del: <T>(url: string) => axios.delete<T>(url).then(res => res.data)
};

const Users = {
    login: (loginRequest: LoginRequest) => requests.post<string>("/auth/login", loginRequest),
    register: (registerRequest: RegisterRequest) => requests.post("/auth/register", registerRequest),
}

export const agent = {
    Users
}