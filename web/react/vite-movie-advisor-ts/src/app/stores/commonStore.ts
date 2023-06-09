import { makeAutoObservable } from "mobx";

class CommonStore {
    token: string | null = localStorage.getItem("token");
    theme: string | null = "light";

    constructor() {
        makeAutoObservable(this);
    }

    setToken(token: string) {
        localStorage.setItem('token', token);
        this.token = token;
    }
}

export default CommonStore;