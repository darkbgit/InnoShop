import { redirect, type ActionFunctionArgs } from "react-router";
import productService from "../api/products";
import type {
  CreateProductRequest,
  ProductEdit,
} from "../interfaces/product.interface";

export const createProductAction = async ({ request }: ActionFunctionArgs) => {
  const formData = await request.formData();

  const product: CreateProductRequest = {
    name: formData.get("name") as string,
    summary: formData.get("summary") as string,
    description: formData.get("description") as string,
    price: parseFloat(formData.get("price") as string),
    isAvailable: formData.get("isAvailable") === "on",
    isOnSale: formData.get("isOnSale") === "on",
    salePrice: parseFloat(formData.get("salePrice") as string) || 0,
    categoryId: parseInt(formData.get("categoryId") as string),
  };

  try {
    await productService.createProduct(product);
    return redirect("/");
  } catch (error: any) {
    return { error: "Failed to create product" };
  }
};

export const editProductAction = async ({
  request,
  params,
}: ActionFunctionArgs) => {
  const id = params.productId;
  if (!id) return new Error("id undefined");

  const formData = await request.formData();

  const product: ProductEdit = {
    id: parseInt(id),
    name: formData.get("name") as string,
    summary: formData.get("summary") as string,
    description: formData.get("description") as string,
    price: parseFloat(formData.get("price") as string),
    isAvailable: formData.get("isAvailable") === "on",
    isOnSale: formData.get("isOnSale") === "on",
    salePrice: parseFloat(formData.get("salePrice") as string) || 0,
    categoryId: parseInt(formData.get("categoryId") as string),
  };

  try {
    await productService.updateProduct(id, product);
    return redirect("/");
  } catch (error: any) {
    return { error: "Failed to update product" };
  }
};

export const deleteProductAction = async ({ params }: ActionFunctionArgs) => {
  const id = params.productId;
  if (!id) return new Error("id undefined");

  try {
    await productService.deleteProduct(id);
    return null;
  } catch (error: any) {
    if (error.response?.status === 403) {
      return {
        error: "Permission denied. You did not create this product.",
      };
    }

    return { error: "Failed to delete product" };
  }
};
