import { redirect } from "react-router";
import type {
  LoginResponse,
  LoginUser,
  RegisterUser,
  User,
  UserInfo,
} from "../interfaces/user.interface";
import { createAxios, requests } from "./agentFactory";

const baseUrl = import.meta.env.VITE_USER_MANAGEMENT;

const userAgentInstance = createAxios(baseUrl!);

const authService = {
  async login(request: LoginUser) {
    const token = await requests.post<string>(
      userAgentInstance,
      "/auth/login",
      request
    );

    localStorage.setItem("jwt_token", token);

    return token;
  },

  async getProfile(): Promise<UserInfo> {
    const token = localStorage.getItem("jwt_token");
    const data = await requests.post<UserInfo>(
      userAgentInstance,
      "/auth/user-info",
      { token: token }
    );
    return data;
  },

  logout() {
    localStorage.removeItem("jwt_token");
    // Optional: Call API if C# needs to revoke refresh tokens
    // await api.post("/auth/logout");
    return redirect("/login");
  },
  current: () => requests.get<User>(userAgentInstance, "/users/user"),
  userInfo: (token: string) =>
    requests.post<UserInfo>(userAgentInstance, "/users/user-info", {
      token: token,
    }),
  // login: (user: UserFormValues) =>
  //   requests.post<string>(userAgentInstance, "/auth/login", user),
  register: (user: RegisterUser) =>
    requests.post<User>(userAgentInstance, "/users/register", user),
};

export default authService;
