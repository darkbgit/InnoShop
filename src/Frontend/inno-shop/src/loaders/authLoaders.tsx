import authService from "../api/auth";
import type { UserInfo } from "../interfaces/user.interface";

// export const loadUser = async () => {
//   const paginatedProducts = await fetchProducts();
//   return { paginatedProducts };
// };

export const rootLoader = async (): Promise<UserInfo | null> => {
  const token = localStorage.getItem("jwt_token");

  if (!token) return null;

  try {
    const user = await authService.getUserInfo(token);
    return user;
  } catch (error) {
    console.error(error);
    return null;
  }
};

export const usersLoader = async (): Promise<User[]> => {};
