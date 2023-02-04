export interface ILogInRequest {
    username: string;
    password: string;
}

export interface ILogInResponse {
    jwtToken: string;
}