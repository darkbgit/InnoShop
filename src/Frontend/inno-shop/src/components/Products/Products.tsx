import { Stack } from "@mui/material";
import ProductCard from "../ProductCard/ProductCard";
import type { ProductDetail } from "../../interfaces/product.interface";

const Products = ({ products }: { products: ProductDetail[] }) => {
  return (
    <Stack spacing={2} sx={{ alignItems: "center", marginTop: 4 }}>
      {products.map((product) => (
        <ProductCard key={product.id} product={product} />
      ))}
    </Stack>
  );
};

export default Products;
