import { redirect, type LoaderFunctionArgs } from "react-router";
import { fetchProductById, fetchProducts } from "../api/products";

export const loadProducts = async ({ request }: LoaderFunctionArgs) => {
  const url = new URL(request.url);
  const searchParams = url.searchParams;
  const query = {
    pageNumber: Number(searchParams.get("pageNumber") || "1"),
    pageSize: Number(searchParams.get("pageSize") || "5"),
    searchString: searchParams.get("searchString") || undefined,
    sortBy: searchParams.get("sortBy") || undefined,
    sortOrder: searchParams.get("sortOrder") || undefined,
    createdBy: searchParams.get("createdBy") || undefined,
  };
  const result = await fetchProducts(query);

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
  const product = await fetchProductById(productId);
  return { product };
};
