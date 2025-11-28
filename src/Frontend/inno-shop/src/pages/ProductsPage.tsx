import { useLoaderData, useSearchParams } from "react-router";
import { loadProducts } from "../loaders/productLoaders";
import { Alert, Pagination, Snackbar, Stack } from "@mui/material";
import Sidebar from "../components/Sidebar/Sidebar";
import Products from "../components/Products/Products";
import { useEffect, useState } from "react";

const ProductsPage = () => {
  const paginatedProducts = useLoaderData<typeof loadProducts>();
  const products = paginatedProducts.items;

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

  const [openToast, setOpenToast] = useState(false);
  const [toastMessage, setToastMessage] = useState("");

  const cleanUrl = () => {
    const newParams = new URLSearchParams(searchParams);
    newParams.delete("status");
    setSearchParams(newParams, { replace: true });
  };

  useEffect(() => {
    const status = searchParams.get("status");

    if (status === "updated") {
      setToastMessage("Product successfully updated!");
      setOpenToast(true);
      cleanUrl();
    } else if (status === "created") {
      setToastMessage("Product successfully created!");
      setOpenToast(true);
      cleanUrl();
    } else if (status === "deleted") {
      setToastMessage("Product successfully deleted!");
      setOpenToast(true);
      cleanUrl();
    }
  }, [searchParams]);

  const handleClose = () => {
    setOpenToast(false);
  };

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
      <Snackbar
        open={openToast}
        autoHideDuration={4000}
        onClose={handleClose}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
      >
        <Alert
          onClose={handleClose}
          severity="success"
          variant="filled"
          sx={{ width: "100%" }}
        >
          {toastMessage}
        </Alert>
      </Snackbar>
    </>
  );
};

export default ProductsPage;
