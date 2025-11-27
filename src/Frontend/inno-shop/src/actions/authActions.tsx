import { redirect, type ActionFunctionArgs } from "react-router";
import authService from "../api/auth";
import type { LoginUser, RegisterUser } from "../interfaces/user.interface";

export const loginAction = async ({ request }: ActionFunctionArgs) => {
  const formData = await request.formData();
  const requestData: LoginUser = {
    userNameOrEmail: formData.get("email") as string,
    password: formData.get("password") as string,
  };

  try {
    await authService.login(requestData);
    return redirect("/");
  } catch (error: any) {
    return {
      error: error.response?.data?.message || "Login failed",
    };
  }
};

export const registerAction = async ({ request }: ActionFunctionArgs) => {
  const formData = await request.formData();
  const requestData: RegisterUser = {
    userName: formData.get("username") as string,
    email: formData.get("email") as string,
    password: formData.get("password") as string,
  };

  try {
    await authService.register(requestData);
    return redirect("/login");
  } catch (error: any) {
    return {
      error: error.response?.data?.message || "Registration failed",
    };
  }
};

export const logoutAction = () => {
  authService.logout();
};

export const deleteUserAction = () => {};
