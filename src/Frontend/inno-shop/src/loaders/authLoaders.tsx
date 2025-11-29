import { redirect, type LoaderFunctionArgs } from "react-router";
import authService from "../api/auth";
import userService from "../api/users";
import type { PaginatedList } from "../interfaces/product.interface";
import type {
  ConfirmEmailRequest,
  User,
  UserInfo,
  UsersQuery,
} from "../interfaces/user.interface";

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

export const usersLoader = async ({
  request,
}: LoaderFunctionArgs): Promise<PaginatedList<User> | Response> => {
  const url = new URL(request.url);
  const searchParams = url.searchParams;
  const query: UsersQuery = {
    pageNumber: Number(searchParams.get("pageNumber") || "1"),
    pageSize: Number(searchParams.get("pageSize") || "5"),
    searchString: searchParams.get("searchString") || undefined,
    sortBy: searchParams.get("sortBy") || undefined,
    sortOrder: searchParams.get("sortOrder") || undefined,
  };
  const result = await userService.getUsers(query);

  if (result.totalPages > 0 && result.currentPage > result.totalPages) {
    url.searchParams.set("pageNumber", result.totalPages.toString());
    return redirect(url.toString());
  }

  return result;
};

export const confirmEmailLoader = async ({ request }: LoaderFunctionArgs) => {
  const url = new URL(request.url);
  const userId = url.searchParams.get("userId");
  const token = url.searchParams.get("token");

  if (!userId || !token) {
    return {
      success: false,
      message: "User ID and token are required for email confirmation.",
    };
  }

  const requestData: ConfirmEmailRequest = {
    userId,
    token,
  };

  try {
    await authService.confirmEmail(requestData);
    return {
      success: true,
      message: "Your email has been confirmed successfully!",
    };
  } catch (error: any) {
    return {
      success: false,
      message:
        error.response?.data?.message ||
        "Failed to confirm email. The link may be invalid or expired.",
    };
  }
};
