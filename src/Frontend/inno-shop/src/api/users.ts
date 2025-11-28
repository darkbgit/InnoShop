import type { User, UsersQuery } from "../interfaces/user.interface";
import { createAxios, requests } from "./agentFactory";
import type { PaginatedList } from "../interfaces/product.interface";

const baseUrl = import.meta.env.VITE_USER_MANAGEMENT;

const userAgentInstance = createAxios(baseUrl!);

const userService = {
  getCurrentUser: async () => {
    const response = await requests.get<User>(userAgentInstance, "/users/user");
    return response;
  },
  getUsers: async (query: UsersQuery) => {
    const response = await requests.get<PaginatedList<User>>(
      userAgentInstance,
      "/users/paginated-with-roles",
      {
        params: query,
      }
    );
    return response;
  },
  deleteUser: async (id: string) => {
    await requests.delete<void>(userAgentInstance, `/users/${id}`);
  },
};

export default userService;
