export interface ProductDetail {
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

export interface PaginatedProducts {
  items: ProductDetail[];
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface PaginatedList<T> {
  items: T[];
  totalCount: number;
  pageSize: number;
  currentPage: number;
  totalPages: number;
}

export interface ProductQuery {
  searchString?: string;
  sortBy?: string;
  sortOrder?: string;
  pageNumber: number;
  pageSize: number;
}
