import type { Product } from "../../interfaces/product.interface";
import Price from "../Price/Price";
import { Box, Typography } from "@mui/material";

const ProductDetailCard = ({ product }: { product: Product }) => {
  return (
    <Box>
      <Typography variant="body2" color="text.secondary">
        Category:{product.categoryName}
      </Typography>
      <Typography gutterBottom variant="h5" component="div">
        {product.name}
      </Typography>
      <Typography variant="body2" color="text.secondary">
        {product.description}
      </Typography>
      <Price
        price={product.price}
        isOnSale={product.isOnSale}
        salePrice={product.salePrice}
        isAvailable={product.isAvailable}
      />
    </Box>
  );
};

export default ProductDetailCard;
