export interface ProductForList {
  id: number;
  name: string;
  summary: string;
  price: number;
  isAvailable: boolean;
  isOnSale: boolean;
  salePrice: number;
  createdAt: Date;
  categoryName: string;
  createdBy: string;
}

export interface Product {
  id: number;
  name: string;
  summary: string;
  description: string;
  price: number;
  isAvailable: boolean;
  isOnSale: boolean;
  salePrice: number;
  createdAt: Date;
  categoryName: string;
  createdBy: string;
}

export interface ProductEdit {
  id: number;
  name: string;
  summary: string;
  description: string;
  price: number;
  isAvailable: boolean;
  isOnSale: boolean;
  salePrice: number;
  categoryId: number;
}

export interface CreateProductRequest {
  name: string;
  summary: string;
  description: string;
  price: number;
  isAvailable: boolean;
  isOnSale: boolean;
  salePrice: number;
  categoryId: number;
}

export interface PaginatedList<T> {
  items: T[];
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface ProductQuery {
  searchString?: string;
  sortBy?: string;
  sortOrder?: string;
  createdBy?: string;
  pageNumber: number;
  pageSize: number;
}

export interface Category {
  id: number;
  name: string;
  description: string;
}
