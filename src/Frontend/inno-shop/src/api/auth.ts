import { redirect } from "react-router";
import type {
  LoginUser,
  RegisterUser,
  User,
  UserInfo,
} from "../interfaces/user.interface";
import { createAxios, requests } from "./agentFactory";

const baseUrl = import.meta.env.VITE_USER_MANAGEMENT;

const userAgentInstance = createAxios(baseUrl!);

const authService = {
  login: async (request: LoginUser) => {
    const token = await requests.post<string>(
      userAgentInstance,
      "/auth/login",
      request
    );
    localStorage.setItem("jwt_token", token);

    return token;
  },

  logout: () => {
    localStorage.removeItem("jwt_token");
    return redirect("/login");
  },

  register: (user: RegisterUser) =>
    requests.post<User>(userAgentInstance, "/users/register", user),

  getUserInfo: async (token: string): Promise<UserInfo> => {
    const data = await requests.post<UserInfo>(
      userAgentInstance,
      "/auth/user-info",
      { token: token }
    );
    return data;
  },

  validateToken: async (request: string): Promise<boolean> => {
    const result = await requests.post<boolean>(
      userAgentInstance,
      "/auth/validate-token",
      request
    );
    return result;
  },

  //current: () => requests.get<User>(userAgentInstance, "/users/user"),

  getUsers: () => console,
  deleteUser: () => console,
};

export default authService;
