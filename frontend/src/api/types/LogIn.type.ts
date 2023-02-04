export interface LogInRequest {
    username: string;
    password: string;
}

export interface LogInResponse {
    jwtToken: string;
}