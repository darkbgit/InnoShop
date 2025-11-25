import {
  createBrowserRouter,
  redirect,
  type ActionFunctionArgs,
  type LoaderFunctionArgs,
} from "react-router";
import RootLayout from "./components/RootLayout";
import ProductsPage from "./pages/ProductsPage";
import ProductDetailPage from "./pages/ProductDetailPage";
import { loadProductDetail, loadProducts } from "./loaders/productLoader";
import { newProductAction } from "./actions/productActions";
import CreateProductPage from "./pages/CreateProductPage";
import LoginPage from "./pages/LoginPage";
import ErrorPage from "./pages/ErrorPage";
import NotFoundPage from "./pages/NotFoundPage";
import authService from "./api/auth";
import { loginAction, registerAction } from "./actions/authActions";
import RegisterPage from "./pages/RegisterPage";

const protectedLoader = async ({ request }: LoaderFunctionArgs) => {
  const token = localStorage.getItem("jwt_token");

  if (!token) {
    return redirect("/login");
  }

  try {
    const user = await authService.getProfile();
    return user;
  } catch (error) {
    // If API rejects token, force logout
    return authService.logout();
  }
};

const AppRouter = createBrowserRouter([
  {
    id: "root",
    path: "/",
    element: <RootLayout />,
    errorElement: <ErrorPage />,
    loader: async () => {
      try {
        const user = await authService.getProfile();
        return user;
      } catch (error) {
        return null;
      }
    },
    children: [
      {
        index: true,
        Component: ProductsPage,
        loader: loadProducts,
      },
      {
        path: "product/:productId",
        loader: loadProductDetail,
        Component: ProductDetailPage,
      },
      {
        path: "product/new",
        action: newProductAction,
        loader: protectedLoader,
        Component: CreateProductPage,
      },
      {
        path: "login",
        action: loginAction,
        Component: LoginPage,
      },
      {
        path: "logout",
        action: () => authService.logout(),
      },
      {
        path: "register",
        action: registerAction,
        Component: RegisterPage,
      },
      // {
      //   path: "users",
      //   action: loginAction,
      //   Component: LoginPage,
      // },
      {
        path: "*",
        Component: NotFoundPage,
      },
    ],
  },
]);

export default AppRouter;
