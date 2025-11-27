import {
  redirect,
  useRouteLoaderData,
  type ActionFunctionArgs,
} from "react-router";
import productService from "../api/products";
import type {
  CreateProductRequest,
  ProductEdit,
} from "../interfaces/product.interface";
import type { UserInfo } from "../interfaces/user.interface";
import authService from "../api/auth";

export const createProductAction = async ({ request }: ActionFunctionArgs) => {
  const formData = await request.formData();

  const product: CreateProductRequest = {
    name: "",
  };
  try {
    await productService.createProduct(product);
    return redirect("/products");
  } catch (error: any) {
    return error;
  }
};

export const updateProductAction = async ({
  request,
  params,
}: ActionFunctionArgs) => {
  const id = params.productId;
  if (!id) return new Error("id undefined");

  const formData = await request.formData();

  const product: ProductEdit = {
    name: "",
  };

  try {
    productService.updateProduct(id, product);
    return redirect("/");
  } catch {
    return { error: "Failed to update product" };
  }
};

export const deleteProductAction = async ({ params }: ActionFunctionArgs) => {
  const id = params.productId;
  if (!id) return new Error("id undefined");

  try {
    await productService.deleteProduct(id);
    return null;
  } catch {
    return { error: "Failed to delete product" };
  }
};
