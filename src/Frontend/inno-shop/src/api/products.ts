import type {
  Category,
  CreateProductRequest,
  PaginatedList,
  Product,
  ProductEdit,
  ProductForList,
  ProductQuery,
} from "../interfaces/product.interface";
import { createAxios, requests } from "./agentFactory";

const baseUrl = import.meta.env.VITE_PRODUCT_MANAGEMENT;

const productAgentInstance = createAxios(baseUrl!);

const productService = {
  getProducts: async (query: ProductQuery) => {
    const response = await requests.get<PaginatedList<ProductForList>>(
      productAgentInstance,
      "/product",
      {
        params: query,
      }
    );
    return response;
  },

  getProductById: async (id: string) => {
    const response = await requests.get<Product>(
      productAgentInstance,
      `/product/${id}`
    );
    return response;
  },

  createProduct: async (product: CreateProductRequest) => {
    await requests.post<string>(productAgentInstance, "/product", product);
  },

  updateProduct: async (id: string, product: ProductEdit): Promise<void> => {
    await requests.put<void>(productAgentInstance, `/product/${id}`, product);
  },

  deleteProduct: async (id: string): Promise<void> => {
    await requests.delete<void>(productAgentInstance, `/product/${id}`);
  },

  getCategories: async (): Promise<Category[]> => {
    const response = await requests.get<Category[]>(
      productAgentInstance,
      "/categories"
    );
    return response;
  },
};

export default productService;
