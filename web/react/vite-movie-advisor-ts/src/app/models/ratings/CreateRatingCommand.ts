export interface CreateRatingCommand {
    movieId: number;
    score: number;
    comment?: string;
}

export class CreateRatingCommand implements CreateRatingCommand {
    constructor(movieId: number) {
        this.movieId = movieId;
    }
}