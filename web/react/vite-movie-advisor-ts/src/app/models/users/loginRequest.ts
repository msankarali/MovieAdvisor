export interface LoginRequest {
    email?: string;
    password?: string;
}

export class LoginRequestFormValues {
    email?: string;
    password?: string;

    constructor(loginRequestFormValues?: LoginRequestFormValues) {
        this.email = loginRequestFormValues?.email;
        this.password = loginRequestFormValues?.password;
    }
}

export class LoginRequest implements LoginRequest {
    constructor(init?: LoginRequestFormValues) {
        Object.assign(this, init);
    }
}