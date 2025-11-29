import { redirect, type ActionFunctionArgs } from "react-router";
import authService from "../api/auth";
import userService from "../api/users";
import type {
  LoginUser,
  RegisterUser,
  ResetPasswordRequest,
} from "../interfaces/user.interface";

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
  return redirect("/");
};

export const deleteUserAction = async ({ params }: ActionFunctionArgs) => {
  if (!params.userId) {
    throw new Response("User ID not found", { status: 404 });
  }
  await userService.deleteUser(params.userId);
  return redirect("/users");
};

export const forgotPasswordAction = async ({ request }: ActionFunctionArgs) => {
  const formData = await request.formData();
  const email = formData.get("email") as string;

  if (!email) {
    return { error: "Email is required" };
  }

  try {
    await authService.forgotPassword(email);
    return {
      message:
        "If an account with that email exists, a password reset link has been sent.",
    };
  } catch (error: any) {
    return {
      message:
        error.response?.data?.message ||
        "Failed to send password reset email. Please try again later.",
    };
  }
};

export const resetPasswordAction = async ({ request }: ActionFunctionArgs) => {
  const formData = await request.formData();
  const password = formData.get("password") as string;
  const email = formData.get("email") as string;
  const confirmPassword = formData.get("confirmPassword") as string;
  const token = formData.get("token") as string;

  if (password !== confirmPassword) {
    return { error: "Passwords do not match" };
  }

  if (!token) {
    return { error: "Invalid or expired password reset token." };
  }

  const requestData: ResetPasswordRequest = {
    token,
    email,
    password,
  };

  try {
    await authService.resetPassword(requestData);
    return {
      message:
        "Your password has been reset successfully. You can now log in with your new password.",
    };
  } catch (error: any) {
    return {
      error:
        error.response?.data?.message ||
        "Failed to reset password. The token may be invalid or expired.",
    };
  }
};
