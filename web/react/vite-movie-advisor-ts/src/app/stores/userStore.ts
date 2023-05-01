import { makeAutoObservable, runInAction } from "mobx";
import { agent } from "../api/agent";
import { LoginRequestFormValues } from "../models/users/loginRequest";
import { store } from "./store";
import { RegisterRequestFormValues } from "../models/users/registerRequest";

class UserStore {
    email: string | undefined = "";
    password: string | undefined = "";

    firstName: string | undefined = "";
    lastName: string | undefined = "";

    constructor() {
        makeAutoObservable(this);
    }

    login = async (loginRequest: LoginRequestFormValues) => {
        const userToken = await agent.Users.login(loginRequest);
        store.commonStore.setToken(userToken);
        store.modalStore.close("login");
        this.email = loginRequest.email;
        this.password = loginRequest.password;
    }

    register = async (registerRequest: RegisterRequestFormValues) => {
        const registeredUser = await agent.Users.register(registerRequest);
        store.modalStore.close("registerPage");
        this.email = registerRequest.email;
        this.password = registerRequest.password;
    }
}

export default UserStore;