import { makeAutoObservable, runInAction } from "mobx";
import { agent } from "../api/agent";
import LoginRequest from "../models/users/loginRequest";
import { store } from "./store";

class UserStore {
    email: string | undefined = "";
    password: string | undefined = "";

    constructor() {
        makeAutoObservable(this);
    }

    login = async (loginRequest: LoginRequest) => {
        try {
            const userToken = await agent.Users.login(loginRequest);
            store.commonStore.setToken(userToken);
        } catch (error) {

        }
        runInAction(() => {
            store.modalStore.close("login");
            this.email = loginRequest.email;
            this.password = loginRequest.password;
        });
    }
}

export default UserStore;