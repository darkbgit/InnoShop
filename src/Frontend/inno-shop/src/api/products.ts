import axios from "axios";
import type {
  PaginatedProducts,
  ProductDetail,
  ProductQuery,
} from "../interfaces/product.interface";

const baseUrl = import.meta.env.VITE_PRODUCT_MANAGEMENT;

export const fetchProducts = async (query: ProductQuery) => {
  const response = await axios.get<PaginatedProducts>(`${baseUrl}`, {
    params: query,
  });
  return response.data;
};

export const fetchProductById = async (id: string) => {
  const response = await axios.get<ProductDetail>(`${baseUrl}/${id}`);
  return response.data;
};
