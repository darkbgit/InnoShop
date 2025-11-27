import { redirect, type LoaderFunctionArgs } from "react-router";
import productService from "../api/products";
import type { ProductQuery } from "../interfaces/product.interface";
import authService from "../api/auth";

export const loadProducts = async ({ request }: LoaderFunctionArgs) => {
  const url = new URL(request.url);
  const searchParams = url.searchParams;
  const query: ProductQuery = {
    pageNumber: Number(searchParams.get("pageNumber") || "1"),
    pageSize: Number(searchParams.get("pageSize") || "5"),
    searchString: searchParams.get("searchString") || undefined,
    sortBy: searchParams.get("sortBy") || undefined,
    sortOrder: searchParams.get("sortOrder") || undefined,
    createdBy: searchParams.get("createdBy") || undefined,
  };
  const result = await productService.getProducts(query);

  if (result.totalPages > 0 && result.currentPage > result.totalPages) {
    url.searchParams.set("pageNumber", result.totalPages.toString());
    return redirect(url.toString());
  }

  return result;
};

export const loadProductDetail = async ({ params }: LoaderFunctionArgs) => {
  const productId = params.productId;
  if (!productId) {
    throw new Error("Product ID is required");
  }
  const product = await productService.getProductById(productId);
  return { product };
};

export const createProductLoader = async () => {
  const token = localStorage.getItem("jwt_token");
  if (!token) {
    return redirect("/login");
  }
  try {
    const isValid = await authService.validateToken(token);
    if (!isValid) return authService.logout();
  } catch {
    return authService.logout();
  }
};

export const editProductLoader = async ({ params }: LoaderFunctionArgs) => {
  const id = params.productId;
  if (!id) throw new Error("Product ID is required");

  const token = localStorage.getItem("jwt_token");
  if (!token) {
    return redirect("/login");
  }

  try {
    const isValid = await authService.validateToken(token);
    if (!isValid) return authService.logout();
  } catch {
    return authService.logout();
  }

  const [user, product] = await Promise.all([
    authService.getUserInfo(token),
    productService.getProductById(id),
  ]);

  if (user.id !== product.createdBy) throw new Response("", { status: 403 });

  return product;
  // try {
  //   const product = await productService.getProductById(id);
  //   return product;
  // } catch (error) {
  //   return error;
  // }
};
