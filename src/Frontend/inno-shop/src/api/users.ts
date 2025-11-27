import axios from "axios";
import type {
  LoginUser,
  RegisterUser,
  User,
  UserInfo,
} from "../interfaces/user.interface";
import { createAxios, requests } from "./agentFactory";

const baseUrl = import.meta.env.VITE_USER_MANAGEMENT;

const userAgentInstance = createAxios(baseUrl!);

export const getCurrentUser = async () => {
  const response = await axios.get<User>(baseUrl);
  return response.data;
};

export const getUsers = async () => {};

export const deleteUser = async (id: string) => {
  await requests.delete<void>(userAgentInstance, `/users/${id}`);
};
