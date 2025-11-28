import { createBrowserRouter } from "react-router";
import RootLayout from "./components/RootLayout";
import ProductsPage from "./pages/ProductsPage";
import ProductDetailPage from "./pages/ProductDetailPage";
import {
  createProductLoader,
  editProductLoader,
  loadProductDetail,
  loadProducts,
} from "./loaders/productLoaders";
import {
  createProductAction,
  deleteProductAction,
  editProductAction,
} from "./actions/productActions";
import CreateProductPage from "./pages/CreateProductPage";
import LoginPage from "./pages/LoginPage";
import ErrorPage from "./pages/ErrorPage";
import NotFoundPage from "./pages/NotFoundPage";
import {
  deleteUserAction,
  loginAction,
  logoutAction,
  registerAction,
  resetPasswordAction,
} from "./actions/authActions";
import RegisterPage from "./pages/RegisterPage";
import EditProductPage from "./pages/EditProductPage";
import { rootLoader, usersLoader } from "./loaders/authLoaders";
import UsersPage from "./pages/UsersPage";
import HomePage from "./pages/HomePage";
import ResetPasswordPage from "./pages/ResetPasswordPage";

const AppRouter = createBrowserRouter([
  {
    id: "root",
    path: "/",
    element: <RootLayout />,
    errorElement: <ErrorPage />,
    loader: rootLoader,
    children: [
      {
        index: true,
        Component: HomePage,
      },
      {
        path: "products",
        Component: ProductsPage,
        loader: loadProducts,
      },
      {
        path: "products/:productId",
        loader: loadProductDetail,
        Component: ProductDetailPage,
      },
      {
        path: "products/:productId/edit",
        loader: editProductLoader,
        action: editProductAction,
        Component: EditProductPage,
      },
      {
        path: "products/:productId/delete",
        action: deleteProductAction,
      },
      {
        path: "products/new",
        action: createProductAction,
        loader: createProductLoader,
        Component: CreateProductPage,
      },
      {
        path: "login",
        action: loginAction,
        Component: LoginPage,
      },
      {
        path: "logout",
        action: logoutAction,
      },
      {
        path: "register",
        action: registerAction,
        Component: RegisterPage,
      },
      {
        path: "users",
        loader: usersLoader,
        Component: UsersPage,
      },
      {
        path: "users/:userId/delete",
        action: deleteUserAction,
      },
      {
        path: "password/reset",
        action: resetPasswordAction,
        Component: ResetPasswordPage,
      },
      {
        path: "*",
        Component: NotFoundPage,
      },
    ],
  },
]);

export default AppRouter;
