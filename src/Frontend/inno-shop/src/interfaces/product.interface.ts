export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  isAvailable: boolean;
  isOnSale: boolean;
  salePrice: number;
  createdAt: Date;
  categoryName: string;
  createdBy: string;
}

export interface ProductDetail {
  id: number;
  name: string;
  longDescription: string;
  price: number;
  isOnSale: boolean;
  salePrice: number;
  createdAt: Date;
  categoryName: string;
  createdBy: string;
}

export interface ProductEdit {
  id: number;
  name: string;
  description: string;
  longDescription: string;
  price: number;
  isAvailable: boolean;
  isOnSale: boolean;
  salePrice: number;
  categoryName: string;
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

export interface CreateProductRequest {
  name: string;
}
