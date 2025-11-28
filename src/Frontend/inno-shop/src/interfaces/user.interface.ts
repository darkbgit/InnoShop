export interface User {
  id: string;
  userName: string;
  email: string;
}

export interface UserInfo {
  id: string;
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

export interface UsersQuery {
  pageNumber?: number;
  pageSize?: number;
  searchString?: string;
  sortBy?: string;
  sortOrder?: string;
}
