import axios from "axios";
import type {
  LoginUser,
  RegisterUser,
  User,
  UserInfo,
} from "../interfaces/user.interface";

const baseUrl = import.meta.env.VITE_USER_MANAGEMENT;

export const getCurrentUser = async () => {
  const response = await axios.get<User>(baseUrl);
  return response.data;
};

export const getUserInfo = async (token: string) => {
  const response = await axios.post<UserInfo>(baseUrl, {
    token: token,
  });
  return response.data;
};

export const login = async (user: LoginUser) => {
  const response = await axios.post<string>(baseUrl, {
    user: user,
  });
  return response.data;
};

export const register = async (user: RegisterUser) => {
  const response = await axios.post<User>(baseUrl, {
    user: user,
  });
  return response.data;
};

// const Account = {
//   current: () => requests.get<User>(userAgentInstance, "/users/user"),
//   userInfo: (token: string) =>
//     requests.post<UserInfo>(userAgentInstance, "/users/user-info", {
//       token: token,
//     }),
//   login: (user: UserFormValues) =>
//     requests.post<string>(userAgentInstance, "/auth/login", user),
//   register: (user: CreateUserModel) =>
//     requests.post<User>(userAgentInstance, "/users/register", user),
// };
