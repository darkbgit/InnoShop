import type { LoaderFunctionArgs } from "react-router";
import { fetchProductById, fetchProducts } from "../api/products";

export const loadUser = async () => {
  const paginatedProducts = await fetchProducts();
  return { paginatedProducts };
};

export const loadProductDetail = async ({ params }: LoaderFunctionArgs) => {
  const productId = params.productId;

  if (!productId) {
    throw new Error("Product ID is required");
  }
  const product = await fetchProductById(productId);
  return { product };
};
