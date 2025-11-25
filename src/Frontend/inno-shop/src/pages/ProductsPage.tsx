import { useLoaderData, useSearchParams } from "react-router";
import { loadProducts } from "../loaders/productLoader";
import { Pagination, Stack } from "@mui/material";
import Sidebar from "../components/Sidebar/Sidebar";
import Products from "../components/Products/Products";

const ProductsPage = () => {
  const paginatedProducts = useLoaderData<typeof loadProducts>();
  //const { items, currentPage, totalPages, totalCount } = useLoaderData() as PaginatedList<ProductDto>;
  const [searchParams, setSearchParams] = useSearchParams();

  const handlePageChange = (
    event: React.ChangeEvent<unknown>,
    value: number
  ) => {
    setSearchParams((prevParams) => {
      prevParams.set("pageNumber", value.toString());
      return prevParams;
    });
  };

  const products = paginatedProducts.items;

  return (
    <>
      <Sidebar />
      <Products products={products} />
      {paginatedProducts.totalPages > 1 && (
        <Stack spacing={2} sx={{ alignItems: "center", marginTop: 4 }}>
          <Pagination
            count={paginatedProducts.totalPages}
            page={paginatedProducts.currentPage}
            onChange={handlePageChange}
          />
        </Stack>
      )}
    </>
  );
};

export default ProductsPage;
