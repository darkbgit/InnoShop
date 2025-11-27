import type {
  CreateProductRequest,
  PaginatedList,
  Product,
  ProductDetail,
  ProductEdit,
  ProductQuery,
} from "../interfaces/product.interface";
import { createAxios, requests } from "./agentFactory";

const baseUrl = import.meta.env.VITE_PRODUCT_MANAGEMENT;

const productAgentInstance = createAxios(baseUrl!);

const productService = {
  getProducts: async (query: ProductQuery) => {
    const response = await requests.get<PaginatedList<Product>>(
      productAgentInstance,
      "/",
      {
        params: query,
      }
    );
    return response;
  },

  getProductById: async (id: string) => {
    const response = await requests.get<ProductDetail>(
      productAgentInstance,
      `/${id}`
    );
    return response;
  },

  createProduct: async (product: CreateProductRequest) => {
    await requests.post<string>(productAgentInstance, "/", product);
  },

  updateProduct: async (id: string, product: ProductEdit): Promise<void> => {
    await requests.put<void>(productAgentInstance, `/${id}`, product);
  },

  deleteProduct: async (id: string): Promise<void> => {
    await requests.delete<void>(productAgentInstance, `/${id}`);
  },
};

export default productService;
