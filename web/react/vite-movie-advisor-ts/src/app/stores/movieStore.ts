import { makeAutoObservable } from 'mobx';
import DataResponseModel from '../models/dataResponseModel';
import { PagedModel } from '../models/PagedModel';
import Movie from '../models/movies/Movie';
import { agent } from '../api/agent';
import { MovieDetails } from '../models/movies/MovieDetails';

class MovieStore {
    movies?: DataResponseModel<PagedModel<Movie>>;
    pageNumber = 1;
    pageSize = 3;

    movieDetails?: DataResponseModel<MovieDetails>

    constructor() {
        makeAutoObservable(this);
    }

    getMovies = async (pageNumber = 1, pageSize = 3) => {
        this.movies = await agent.Movies.get(pageNumber, pageSize);
    }

    getMovieDetails = async (movieId: number) => {
        this.movieDetails = await agent.Movies.getById(movieId);
    }

}

export default MovieStore;