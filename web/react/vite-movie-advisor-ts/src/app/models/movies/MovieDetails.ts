export interface MovieDetails {
    title: string;
    description: string;
    releaseDate: string;
    averageScore: string;
    ratings: MovieDetailsRatingItem[];
}

export interface MovieDetailsRatingItem {
    userFullName: string;
    score: number;
    comment: string | null;
}