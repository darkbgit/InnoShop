export interface User {
  id: string;
  timeZoneId: string;
  language: string;
  userName: string;
  firstName: string;
  surname: string;
  phoneNumber: string;
  email: string;
}

export interface UserInfo {
  id: string;
  email: string;
  roles: string[];
}

export interface LoginUser {
  userNameOrEmail: string;
  password: string;
  displayName?: string;
  userName?: string;
}

export interface RegisterUser {
  userName: string;
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
}

export interface LoginResponse1 {
  token: string;
  refreshToken?: string;
  user: User;
}
