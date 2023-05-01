import { CreateRatingCommand } from './../models/ratings/CreateRatingCommand';
import axios, { AxiosError, AxiosResponse } from "axios";
import { store } from "../stores/store";
import { LoginRequestFormValues } from "../models/users/loginRequest";
import DataResponseModel from "../models/dataResponseModel";
import { PagedModel } from "../models/PagedModel";
import Movie from "../models/movies/Movie";
import { Slide, ToastContent, toast } from "react-toastify";
import 'react-toastify/dist/ReactToastify.min.css';
import { RegisterRequestFormValues } from "../models/users/registerRequest";
import { MovieDetails } from "../models/movies/MovieDetails";

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

    data?.Messages?.forEach(message => {
        toast.info(message, {
            position: "top-right",
            autoClose: 5000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            theme: "light",
            transition: Slide
        });
    });

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
    login: (loginRequest: LoginRequestFormValues) => requests.post<string>("/auth/login", loginRequest),
    register: (registerRequest: RegisterRequestFormValues) => requests.post("/auth/register", registerRequest),
}

const Movies = {
    get: (pageNumber = 1, pageSize = 3) => requests.get<DataResponseModel<PagedModel<Movie>>>(`/movies?pageNumber=${pageNumber}&pageSize=${pageSize}`),
    getById: (movieId: number) => requests.get<DataResponseModel<MovieDetails>>(`/movies/${movieId}`),
}

const Ratings = {
    post: (createRatingCommand: CreateRatingCommand) => requests.post<CreateRatingCommand>('/rating', createRatingCommand),
}

export const agent = {
    Users,
    Movies,
    Ratings
}