import { makeAutoObservable } from "mobx";
import { CreateRatingCommand } from "../models/ratings/CreateRatingCommand";
import { agent } from "../api/agent";
import { store } from "./store";

class RatingStore {

    constructor() {
        makeAutoObservable(this);
    }

    createRating = async (createRatingCommand: CreateRatingCommand) => {
        await agent.Ratings.post(createRatingCommand);
        store.movieStore.getMovieDetails(createRatingCommand.movieId);
    }
}

export default RatingStore;