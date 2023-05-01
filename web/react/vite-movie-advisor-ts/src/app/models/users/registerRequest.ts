export default interface RegisterRequest {
    firstName: string | undefined;
    lastName: string | undefined;
    email: string | undefined;
    password: string | undefined;
}


export class RegisterRequestFormValues {
    firstName?: string;
    lastName?: string;
    email?: string;
    password?: string;

    constructor(registerRequestFormValues?: RegisterRequestFormValues) {
        this.firstName = registerRequestFormValues?.firstName;
        this.lastName = registerRequestFormValues?.lastName;
        this.email = registerRequestFormValues?.email;
        this.password = registerRequestFormValues?.password;
    }
}