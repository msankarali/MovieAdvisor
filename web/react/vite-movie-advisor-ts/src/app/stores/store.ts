import { createContext, useContext } from "react";
import CommonStore from "./commonStore";
import UserStore from "./userStore";
import ModalStore from "./modalStore";
import MovieStore from "./movieStore";
import RatingStore from "./ratingStore";

interface Store {
    userStore: UserStore,
    commonStore: CommonStore,
    modalStore: ModalStore,
    movieStore: MovieStore,
    ratingStore: RatingStore
}

export const store: Store = {
    userStore: new UserStore(),
    commonStore: new CommonStore(),
    modalStore: new ModalStore(),
    movieStore: new MovieStore(),
    ratingStore: new RatingStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}
